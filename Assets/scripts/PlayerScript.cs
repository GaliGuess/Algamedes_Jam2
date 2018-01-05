using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;
using UnityEngine.Collections;
using Utils.Utils;


public class PlayerScript : MonoBehaviour {

	[SerializeField] GameColor gameColor = GameColor.BLACK;

	[SerializeField] float maxSpeed = 17;
	private float speed = 40f;
	[SerializeField] float jumpHeight = 350f;
	[SerializeField] float jumpBonusGravity = 15f;
	[SerializeField] float recoil = 0.15f;
//	[SerializeField] bool doubleJumpEnabled = true;
	
	private Controller _controller;
	private Rigidbody2D _rigidbody2D;
	private GameManager _gameManager;
	private SpriteRenderer _spriteRenderer;

	public Vector2 direction, lastHorizontalDirection;
	public bool isGrounded, canDoubleJump;
	private float jumpRatio = 0.2f;
	
	// for testing
	public Vector2 currentVelocity;
	
	
	void Start ()
	{
		_gameManager = GetComponentInParent<GameManager>();
		_controller = GetComponent<Controller>();
		_rigidbody2D = GetComponent<Rigidbody2D>();
		_spriteRenderer = GetComponent<SpriteRenderer>();
		SetSpriteColor(gameColor);

		lastHorizontalDirection = transform.position.x < 0 ? Vector2.right : Vector2.left;
		canDoubleJump = false;
	}

	
	void FixedUpdate()
	{	
		if (_controller != null)
		{
			if (!Mathf.Approximately(direction.x, 0))
			{
				lastHorizontalDirection = new Vector2(direction.x, 0);
			}
			updateDirection();

			if (_controller.jump())
			{
				jump();
			}
			
			move(new Vector2(direction.x, 0));

			if (_controller.shoot())
			{
				if (direction == Vector2.zero)  // Making sure player doesn't shoot at direction (0,0)
				{
					direction = lastHorizontalDirection;
				}
			
				shoot(direction);
			}

			// fake gravity for when airborne
			if (!isGrounded)
			{
				Vector2 vel = _rigidbody2D.velocity;
				
				vel.y -= jumpBonusGravity * Time.deltaTime;
				_rigidbody2D.velocity = vel;
				
				slowHorizontalVelocity(1.01f);
			}
			
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
		newVelocity.x = Mathf.Lerp(_rigidbody2D.velocity.x, newVelocity.x, Time.deltaTime);
		newVelocity.x = Mathf.Clamp(newVelocity.x, -maxSpeed, maxSpeed); 
		_rigidbody2D.velocity = newVelocity;
	}

	private void jump()
	{
		if (isGrounded && !canDoubleJump)
		{
			_rigidbody2D.velocity += new Vector2(direction.x * jumpHeight * jumpRatio * Time.deltaTime, 
												 jumpHeight * Time.deltaTime);
//			_rigidbody2D.velocity += new Vector2(0, jumpHeight * Time.deltaTime);
//			canDoubleJump = true;
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
		Vector2 pos = transform.position;
		_gameManager.SpawnShot(pos, _rigidbody2D.velocity, direction.GetAngle(), gameColor);
		
		// recoil
		transform.position = new Vector3(pos.x - direction.x * recoil, pos.y - direction.y * recoil, transform.position.z);
	}
	

	private void OnCollisionEnter2D(Collision2D other)
	{
		if (other.transform.position.y < transform.position.y)
		{
			isGrounded = true;
		}
	}
	
	private void OnCollisionExit2D(Collision2D other)
	{
		isGrounded = false;
	}


	private void slowHorizontalVelocity(float factor)
	{
		Vector2 vel = _rigidbody2D.velocity;
		vel.x /= factor;
		_rigidbody2D.velocity = vel;
	}
	
	
	// currently compied for the demo from PlatformMangager
	private void SetSpriteColor(GameColor color)
	{
		switch (color)
		{
			case GameColor.BLACK:
				_spriteRenderer.material.color = Color.black;
				break;
			case GameColor.GREY:
				_spriteRenderer.material.color = Color.grey;
				break;
			case GameColor.WHITE:
				_spriteRenderer.material.color = Color.white;
				break;
		}
	}
}
