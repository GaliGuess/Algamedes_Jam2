using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game;
using Utils.Utils;

public class PlayerManager : MonoBehaviour
{
	private PlayerView _playerView;

	private PlayerState _playerState;	

	// physics related constants
	
	[SerializeField, Tooltip("The player's speed")] 
	float maxSpeed = 17;
	private float speed = 40f; // need to get rid of this but maybe some other time :)
	
	[SerializeField, Tooltip("Lower value makes the player switch direction slower")] 
	public float turnRate = 1.5f;
	
	[SerializeField, Tooltip("Lower value makes the player slide more on floors\n Value of 1 adds nothing")] 
	public float bonusFriction = 1.05f;
	
	[SerializeField]
	public int jumpHeight = 700;
	
	[SerializeField, Tooltip("Player friction with air\nnormal value is 0")]
	public float normalDrag = 0f;
	
	[SerializeField, Tooltip("Player friction with air just during falls after jumping\nThe higher the value the slower the fall")]
	public float fallingDrag = 20f;
	
	// used to change drag during fall
	private bool isJumping = false;
	
//	[SerializeField, Tooltip("Additional gravity is added when the player is in the air")] 
//	public float jumpBonusGravity = 15f;
	
	[SerializeField, Tooltip("Lower value makes shooting faster")]
	public int turnsBetweenShots = 5;
	
	[SerializeField, Tooltip("How far the player is pushed back when shooting")]
	public float recoil = 0.12f;

//	[SerializeField] bool doubleJumpEnabled = true;
	
	private Controller _controller;
	private Rigidbody2D _rigidbody2D;
	private GameManager _gameManager;

	public Vector2 direction; // public only for testing
	private Vector2 shootingDirection; // for testing
	
	private Vector2 lastNonZeroDirection;
	public bool isGrounded;
	private bool releaseJump = false;
	private bool canDoubleJump; // double jump is currently disabled but this is still used!
	private float jumpRatio = 0.2f;
	private int _timesSinceFired = 0;
	
	// Used for checking if player is grounded
	private Transform overlap_topLeft, overlap_bottomRight;
	private int overlap_layersMask;
	private Collider2D[] _overlap_colliders = new Collider2D[10];
	
	[SerializeField, Tooltip("Player can't die\n(but can fall to infinity)")] 
	public bool invincible = false;
	
	// for testing
	private Vector2 currentVelocity;
	

	void Awake()
	{
		_playerState = GetComponent<PlayerState>();
		_playerView = GetComponent<PlayerView>();
		
		_gameManager = GetComponentInParent<GameManager>();
		_controller = GetComponent<Controller>();
		_rigidbody2D = GetComponent<Rigidbody2D>();
		
		overlap_topLeft = transform.Find(Values.PLAYER_TOP_LEFT_GAMEOBJ_NAME);
		overlap_bottomRight = transform.Find(Values.PLAYER_BOT_RIGHT_GAMEOBJ_NAME);
	}
	
	
	void Start ()
	{	
		_playerView.SetSpriteColor(_playerState.player_framework);

		lastNonZeroDirection = _rigidbody2D.position.x < 0 ? Vector2.right : Vector2.left;
		canDoubleJump = false;

		// layer of platforms for checking if grounded
		if (_playerState.player_framework == Framework.BLACK) overlap_layersMask = LayerMask.GetMask("platforms_black", "floor");
		else if (_playerState.player_framework == Framework.WHITE) overlap_layersMask = LayerMask.GetMask("platforms_white", "floor"); 
	}

	private void Update()
	{
		// saving last x direction that is not zero. used for shooting.
		if (direction != Vector2.zero)
		{
			lastNonZeroDirection = direction;
		}
		
		updateDirection();
	}

	void FixedUpdate()
	{	
		if (_controller != null)
		{

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
			if (direction == Vector2.zero) shootingDirection = lastNonZeroDirection;
			_playerView.changeCrosshairDirection(shootingDirection);
			
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
		
		Vector2 pos = _rigidbody2D.position;
		_gameManager.SpawnShot(pos, _rigidbody2D.velocity, direction.GetAngle(), _playerState.player_framework);
		
		// recoil
		_rigidbody2D.position = new Vector3(pos.x - direction.x * recoil, pos.y - direction.y * recoil, transform.position.z);
	}
	
	private void slowHorizontalVelocity(float factor)
	{
		Vector2 vel = _rigidbody2D.velocity;
		vel.x /= factor;
		_rigidbody2D.velocity = vel;
	}

	private void OnCollisionEnter2D(Collision2D other)
	{
		if (other.transform.position.y < _rigidbody2D.position.y)
		{
			releaseJump = true;
		}
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		if (other.CompareTag(Values.BOUNDRIES_TAG))
		{
			if (invincible) return;
			_gameManager.PlayerKilled(gameObject);
		}
	}

	private void OnCollisionExit2D(Collision2D other)
	{
		releaseJump = false;
	}
}
