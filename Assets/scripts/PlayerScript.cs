using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;
using UnityEngine.Collections;
using Utils.Utils;


public class PlayerScript : MonoBehaviour {

	[SerializeField] Framework player_framework = Framework.BLACK;

	[SerializeField, Tooltip("The player's speed")] 
	float maxSpeed = 17;
	private float speed = 40f; // need to get rid of this but maybe some other time :)
	
//	Range(1f, 10f)
	[SerializeField, Tooltip("Lower value makes the player switch direction slower")] 
	public float turnRate = 1.5f;
	
//	Range(0.5f, 2f)
	[SerializeField, Tooltip("Lower value makes the player slide more on floors\n Value of 1 adds nothing")] 
	public float bonusFriction = 1.05f;
	
//	[Range(350, 1000)]
	[SerializeField]
	public int jumpHeight = 700;
	
	
	[SerializeField, Tooltip("Player friction with air\nnormal value is 0")]
	public float normalDrag = 0f;
	
	[SerializeField, Tooltip("Player friction with air just during falls after jumping\nThe higher the value the slower the fall")]
	public float fallingDrag = 20f;
	
	// used to change drag during fall
	private bool isJumping = false;
	
//	Range(0f, 30f)
//	[SerializeField, Tooltip("Additional gravity is added when the player is in the air")] 
//	public float jumpBonusGravity = 15f;
	
//	Range(0, 300)
	[SerializeField, Tooltip("Lower value makes shooting faster")]
	public int turnsBetweenShots = 5;
	
//	Range(0f, 2f)
	[SerializeField, Tooltip("How far the player is pushed back when shooting")]
	public float recoil = 0.12f;

//	[SerializeField] bool doubleJumpEnabled = true;
	
	private Controller _controller;
	private Rigidbody2D _rigidbody2D;
	private GameManager _gameManager;
	private SpriteRenderer _spriteRenderer;

	public Vector2 direction;
	public Vector2 shootingDirection; // for testing
	
	private Vector2 lastHorizontalDirection;
	private bool isGrounded, releaseJump = false;
	private bool canDoubleJump; // double jump is currently disabled but this is still used!
	private float jumpRatio = 0.2f;
	private int _timesSinceFired = 0;
	
	// Used for checking if player is grounded
	private Transform overlap_topLeft, overlap_bottomRight;
	private int overlap_layersMask;
	private Collider2D[] _overlap_colliders = new Collider2D[10];

	private Transform crosshair;

	[SerializeField]
	public bool showCrosshair = true;
	
//	Range(1f, 5f)
	[SerializeField, Tooltip("The distance of the crosshair from the player")]
	public float crosshairDistance = 2.5f;
	
	[SerializeField, Tooltip("Player can't die\n(but can fall to infinity)")] 
	public bool invincible = false;
	
	// for testing
	private Vector2 currentVelocity;
	

	void Awake()
	{
		_gameManager = GetComponentInParent<GameManager>();
		_controller = GetComponent<Controller>();
		_rigidbody2D = GetComponent<Rigidbody2D>();
		_spriteRenderer = GetComponent<SpriteRenderer>();
		
		overlap_topLeft = transform.Find("overlap_topLeft");
		overlap_bottomRight = transform.Find("overlap_bottomRight");
		crosshair = transform.Find("crosshair");
	}
	
	
	void Start ()
	{	
		SetSpriteColor(player_framework);

		lastHorizontalDirection = transform.position.x < 0 ? Vector2.right : Vector2.left;
		canDoubleJump = false;

		// layer of platforms for checking if grounded
		if (player_framework == Framework.BLACK) overlap_layersMask = LayerMask.GetMask("platforms_black", "floor");
		else if (player_framework == Framework.WHITE) overlap_layersMask = LayerMask.GetMask("platforms_white", "floor"); 
	}
	
//	void FixedUpdate()
	void Update()
	{	
		if (_controller != null)
		{
			// saving last x direction that is not zero. used for shooting.
			if (!Mathf.Approximately(direction.x, 0))
			{
				lastHorizontalDirection = new Vector2(direction.x, 0);
			}
			
			updateDirection();

			isGrounded = Physics2D.OverlapAreaNonAlloc(overlap_topLeft.position, overlap_bottomRight.position,
				             _overlap_colliders, overlap_layersMask) > 0;
			if (isGrounded) isJumping = false;
			
			if (_controller.jump())
			{ 
				jump();
			}
			
			move(new Vector2(direction.x, 0));
			
			// used so the player doesn't slide as much
			if (Mathf.Approximately(direction.x, 0) && isGrounded)
			{
				slowHorizontalVelocity(bonusFriction);
			}
			
			if (_timesSinceFired > 0) _timesSinceFired--;
			
			// Making sure player doesn't shoot at direction (0,0)
			shootingDirection = direction;
			if (direction == Vector2.zero) shootingDirection = lastHorizontalDirection;
			crosshair.localPosition = new Vector2(shootingDirection.x / transform.localScale[0], 
												  shootingDirection.y / transform.localScale[1]) * crosshairDistance;			
			if (_controller.shoot())
			{
				shoot(shootingDirection);
			}

			if (isJumping && _rigidbody2D.velocity.y < 0)
			{
				_rigidbody2D.drag = fallingDrag;
			}
			else _rigidbody2D.drag = normalDrag;
			
			// for testing
			currentVelocity = _rigidbody2D.velocity;
		}
	}

	private void updateDirection()
	{
		direction = Vector2.zero;
			
		if (_controller.look_up()) direction += Vector2.up;
		if (_controller.look_down()) direction += Vector2.down;
		if (_controller.turn_left()) direction += Vector2.left;
		if (_controller.turn_right()) direction += Vector2.right;
	}
	
	private void move(Vector2 direction)
	{
		Vector2 newVelocity = _rigidbody2D.velocity;
		newVelocity.x = direction.x * speed;
		newVelocity.x = Mathf.Lerp(_rigidbody2D.velocity.x, newVelocity.x, Time.deltaTime * turnRate);
		newVelocity.x = Mathf.Clamp(newVelocity.x, -maxSpeed, maxSpeed); 
		_rigidbody2D.velocity = newVelocity;
	}

	private void jump()
	{
		if (isGrounded && !canDoubleJump && releaseJump)
		{
			_rigidbody2D.velocity += new Vector2(direction.x * jumpHeight * jumpRatio * Time.deltaTime, 
												 jumpHeight * Time.deltaTime);
//			_rigidbody2D.velocity += new Vector2(0, jumpHeight * Time.deltaTime);
//			canDoubleJump = true;
			isJumping = true;
		}
//		else if (canDoubleJump)
//		{
//			if (doubleJumpEnabled)
//			{
//				_rigidbody2D.velocity += new Vector2(direction.x * jumpHeight * jumpRatio * Time.deltaTime, 
//												  	 jumpHeight * Time.deltaTime);
//				
////				Debug.Log("Double Jumped.");
//			}
//			canDoubleJump = false;
//		}
	}
	
	private void shoot(Vector2 direction)
	{
		if (_timesSinceFired > 0) return;
		_timesSinceFired = turnsBetweenShots;
		
		Vector2 pos = transform.position;
		_gameManager.SpawnShot(pos, _rigidbody2D.velocity, direction.GetAngle(), player_framework);
		
		// recoil
		transform.position = new Vector3(pos.x - direction.x * recoil, pos.y - direction.y * recoil, transform.position.z);
	}

//	private void fallSlower()
//	{
//		Vector2 vel = _rigidbody2D.velocity;
//		vel.y -= fallSlowingFactor * Time.deltaTime;
//		_rigidbody2D.velocity = vel;
//	}
	
	private void slowHorizontalVelocity(float factor)
	{
		Vector2 vel = _rigidbody2D.velocity;
		vel.x /= factor;
		_rigidbody2D.velocity = vel;
	}

	private void OnCollisionEnter2D(Collision2D other)
	{
		if (other.transform.position.y < transform.position.y)
		{
//			isGrounded = true;
			releaseJump = true;
		}
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		if (other.CompareTag("gameBoundry"))
		{
			if (invincible) return;
			_gameManager.PlayerKilled(gameObject);
		}
	}

	private void OnCollisionExit2D(Collision2D other)
	{
//		isGrounded = false;
		releaseJump = false;
	}
	
	// currently compied for the demo from PlatformMangager
	private void SetSpriteColor(Framework framework)
	{
		SpriteRenderer crosshair_spriteRenderer = crosshair.GetComponent<SpriteRenderer>();
		if (!showCrosshair) crosshair_spriteRenderer.enabled = false;
		
		switch (framework)
		{
			case Framework.BLACK:
				_spriteRenderer.material.color = Color.black;
				crosshair_spriteRenderer.material.color = Color.black;
				break;
			
			case Framework.GREY:
				_spriteRenderer.material.color = Color.grey;
				crosshair_spriteRenderer.material.color = Color.grey;
				break;
			
			case Framework.WHITE:
				_spriteRenderer.material.color = Color.white;
				crosshair_spriteRenderer.material.color = Color.white;
				break;
		}
	}
}
