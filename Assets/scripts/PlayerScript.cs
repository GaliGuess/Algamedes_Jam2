using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;
using UnityEngine.Collections;
using Utils.Utils;


public class PlayerScript : MonoBehaviour {

	[SerializeField] GameColor gameColor = GameColor.BLACK;

	[SerializeField] float maxSpeed = 3;
	[SerializeField] float speed = .5f;
	[SerializeField] float jumpHeight = 1f;
	
	private Controller _controller;
	private Rigidbody2D _rigidbody2D;
	private GameManager _gameManager;
	private SpriteRenderer _spriteRenderer;

	public Vector2 direction, lastHorizontalDirection;
	public bool isGrounded;
	
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

			if (_controller.jump() && isGrounded)
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
//		_rigidbody2D.velocity += direction * Time.deltaTime * speed;
		_rigidbody2D.velocity = Vector2.Lerp(_rigidbody2D.velocity, direction * speed, Time.deltaTime); 
		
		if (_rigidbody2D.velocity.x > maxSpeed)
		{
			_rigidbody2D.velocity = new Vector2(maxSpeed, _rigidbody2D.velocity.y);
		}
		
		if (_rigidbody2D.velocity.x < -maxSpeed)
		{
			_rigidbody2D.velocity = new Vector2(-maxSpeed, _rigidbody2D.velocity.y);
		}
	}

	private void jump()
	{
		_rigidbody2D.velocity += new Vector2(0, jumpHeight);
	}
	
	private void shoot(Vector2 direction)
	{
		Vector2 pos = transform.position;
		_gameManager.SpawnShot(pos, _rigidbody2D.velocity, direction.GetAngle(), gameColor);
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
