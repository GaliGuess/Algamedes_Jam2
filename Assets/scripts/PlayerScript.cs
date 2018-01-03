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
	
	[SerializeField] PlayerController controller;
	
	private Rigidbody2D _rigidbody2D;
	private GameManager _gameManager;
	private SpriteRenderer _spriteRenderer;
	
	private bool grounded;


	public Vector2 lookingDir; ////
	
	// Use this for initialization
	void Start ()
	{
		controller = GetComponent<PlayerController>();
		_gameManager = GetComponentInParent<GameManager>();
		_rigidbody2D = GetComponent<Rigidbody2D>();
		_spriteRenderer = GetComponent<SpriteRenderer>();
		SetSpriteColor(gameColor);
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		if (controller != null)
		{
			controller.updatePlayerAction();
			Vector2 lookDir = controller.getLookingDirection();
			lookingDir = lookDir;
			Vector2 movementDir = controller.getMovementDirection();
		
			movePlayer(movementDir, controller.isJumping());

			if (controller.isShooting())
			{
				Vector2 pos = transform.position;
				_gameManager.SpawnShot(pos, _rigidbody2D.velocity, lookDir.GetAngle(), gameColor);
			}
		}
	}

	private void movePlayer(Vector2 direction, bool addJump)
	{
		if (addJump && grounded)
		{
			_rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, 0);
			_rigidbody2D.AddForce(Vector2.up * jumpHeight);
		}
		_rigidbody2D.AddForce(direction * speed);
		
		if (_rigidbody2D.velocity.x > maxSpeed)
		{
			_rigidbody2D.velocity = new Vector2(maxSpeed, _rigidbody2D.velocity.y);
		}
		
		if (_rigidbody2D.velocity.x < -maxSpeed)
		{
			_rigidbody2D.velocity = new Vector2(-maxSpeed, _rigidbody2D.velocity.y);
		}
	}

	private void OnCollisionEnter2D(Collision2D other)
	{
		grounded = true;
	}
	
	private void OnCollisionExit2D(Collision2D other)
	{
		grounded = false;
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
