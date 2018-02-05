﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Controllers;
using Utils.Utils;


namespace Game{
	public class PlayerManager : MonoBehaviour
	{
		private PlayerView _playerView;

		private PlayerState _playerState;

		[Header("Controllers")]
		[SerializeField] public bool usingKeyboard;
		[SerializeField] public bool usingPS4Controller;
		private List<Controller> controllers;


		[Header("Physics")]
		[SerializeField, Tooltip("The player's speed")]
		float maxXVelocity = 17;
		public float VelocityFactor = 40f; // need to get rid of this but maybe some other time :)

		//	[SerializeField, Tooltip("The maximum speed in y axis during falling")]
		private float MaxFallingVelocity = 40;

		[SerializeField, Tooltip("Lower value makes the player switch direction slower")]
		public float turnRate = 1.5f;

		[SerializeField, Tooltip("Lower value makes the player slide more on floors\n Value of 1 adds nothing")]
		public float bonusFriction = 1.05f;

		[SerializeField]
		public int jumpHeight = 700;

		[HideInInspector] bool doubleJumpEnabled = true; // used to be relevant in inspector
		[SerializeField, Tooltip("The higher the value, the sooner the player can doubleJump")]
		public float minimalDoubleJumpVelocity = 17f;
		
		public float RegularGravityScale = 1;
		public float FallingGravityScale = 1;

		[SerializeField, Tooltip("Lower value makes shooting faster")]
		public int turnsBetweenShots = 5;

		[SerializeField, Tooltip("How far the player is pushed back when shooting")]
		public float recoil = 0.12f;

		private Rigidbody2D _rigidbody2D;
		private GameManager _gameManager;

		[Header("for Testing")]
		public Vector2 movingDirection; // public only for testing
		public Vector2 shootingDirection;

		public bool isGrounded;
		private bool canDoubleJump;
		private float jumpRatio = 0.2f;
		private int _timesSinceFired = 0;
		private float Jump_Y_Threshold = 5f;

		// Used for checking if player is grounded
		private Transform overlap_topLeft, overlap_bottomRight;
		private int overlap_layersMask;
		private Collider2D[] _overlap_colliders = new Collider2D[10];

		[SerializeField, Tooltip("Player can't die\n(but can fall to infinity)")]
		public bool invincible = false;

		// for testing
		public Vector2 currentVelocity;
		
		public bool debugMode;
		private PlayerLog eventLog;

		void Awake()
		{
			_playerState = GetComponent<PlayerState>();
			_playerView = GetComponent<PlayerView>();

			_gameManager = GetComponentInParent<GameManager>();
			_rigidbody2D = GetComponent<Rigidbody2D>();

			overlap_topLeft = transform.Find("overlap_topLeft");
			overlap_bottomRight = transform.Find("overlap_bottomRight");


			controllers = new List<Controller>();
			if (usingPS4Controller)
			{
				Controller cont = GetComponent<PS4Controller>();
				if (cont != null) controllers.Add(cont);
			}
			if (usingKeyboard)
			{
				Controller cont = GetComponent<KeyboardController>();
				if (cont != null) controllers.Add(cont);
			}

			overlap_topLeft = transform.Find(Values.PLAYER_TOP_LEFT_GAMEOBJ_NAME);
			overlap_bottomRight = transform.Find(Values.PLAYER_BOT_RIGHT_GAMEOBJ_NAME);
		}


		void Start ()
		{
			_playerView.SetSpriteColor(_playerState.player_framework);

			// layer of platforms for checking if grounded
			if (_playerState.player_framework == Framework.BLACK) overlap_layersMask = LayerMask.GetMask("platforms_black", "floor");
			else if (_playerState.player_framework == Framework.WHITE) overlap_layersMask = LayerMask.GetMask("platforms_white", "floor");

			if (debugMode)
			{
				eventLog = GetComponent<PlayerLog>();
				foreach (var controller in controllers)
				{
					controller.debugModeStatus(true);
				}
			}
		}

		private void Update()
		{
			updateDirection();
			
			// jumping moved here because it was not responsive enough in FixedUpdate (missed controller updates)
			foreach (var controller in controllers)
			{
				if (controller.jump()){
					tryToJump();
				} 
				else  {
					_playerView.isJumping = false;
					_playerView.isDoubleJumping = false;
				}	
			}
		}

		void FixedUpdate()
		{
			foreach (var controller in controllers)
			{
				if (controller != null)
				{
					updateGrounded();

					if (controller.getDown())
					{
						getOffPlatform();
					}

					move(movingDirection);

					// used so the player doesn't slide as much
					if (Mathf.Approximately(movingDirection.x, 0) && isGrounded)
					{
						slowHorizontalVelocity(bonusFriction);
					}

					if (_timesSinceFired > 0) _timesSinceFired--;

					_playerView.changeCrosshairDirection(shootingDirection);
					if (controller.shoot())
					{
						shoot(shootingDirection);
						_playerView.isShooting = true;
					} else {
						_playerView.isShooting = false;
					}

					// Different gravity scale during fall
					if (_rigidbody2D.velocity.y < 0) _rigidbody2D.gravityScale = FallingGravityScale;
					else _rigidbody2D.gravityScale = RegularGravityScale;

					// limiting y speed while falling
					if (!isGrounded && _rigidbody2D.velocity.y < -MaxFallingVelocity)
					{
						_rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, -MaxFallingVelocity);
					}
					currentVelocity = _rigidbody2D.velocity;
				}
			}
		}


		private void updateDirection()
		{
			foreach (var controller in controllers)
			{

				movingDirection = controller.moving_direction();
				shootingDirection = controller.aim_direction();
			}
		}

		private void move(Vector2 direction)
		{
			Vector2 newVelocity = _rigidbody2D.velocity;
			//		newVelocity.x = direction.x * VelocityFactor;
			newVelocity.x = Mathf.Lerp(_rigidbody2D.velocity.x, direction.x * VelocityFactor, -Mathf.Pow(Time.deltaTime, 2) + 1);
			newVelocity.x = Mathf.Clamp(newVelocity.x, -maxXVelocity, maxXVelocity);
			_rigidbody2D.velocity = newVelocity;
			if (direction.x > 0) {
				_playerView.horizontal_dir = 1;
			} 
			else if (direction.x < 0 ) {
				_playerView.horizontal_dir = -1;
			} else {
				_playerView.horizontal_dir = 0;
			}
		}

		private void updateGrounded()
		{
			isGrounded = Physics2D.OverlapAreaNonAlloc(overlap_topLeft.position, overlap_bottomRight.position,
				             _overlap_colliders, overlap_layersMask) > 0;
		}
		
		private void tryToJump()
		{
			updateGrounded();
			
			if (isGrounded)
			{
				// This is to avoid chain jumping
				if (_rigidbody2D.velocity.y > Jump_Y_Threshold)
				{
					if (debugModeOn()) eventLog.AddEvent("PlayerManager: Didn't jump. y speed: " + _rigidbody2D.velocity.y);
					return;
				}
				
				DisconnectFromPlatfrom();
				
				_rigidbody2D.velocity += new Vector2(0, jumpHeight);
				if (debugModeOn()) eventLog.AddEvent("PlayerManager: Jumped.");
				_playerView.isJumping = true;
				canDoubleJump = true;
			}

			// Double jumping
			else if (canDoubleJump)
			{
				// This is to avoid chain jumping
				if (_rigidbody2D.velocity.y > minimalDoubleJumpVelocity)
				{
					if (debugModeOn()) eventLog.AddEvent("PlayerManager: Didn't doubleJump. y speed: " + _rigidbody2D.velocity.y);
					return;
				}
				
				if (doubleJumpEnabled)
				{					
					_rigidbody2D.velocity += new Vector2(0, jumpHeight);
					if (debugModeOn()) eventLog.AddEvent("PlayerManager: DoubleJumped");
					_playerView.isDoubleJumping = true;
				}

				canDoubleJump = false;
			}

			else
			{
				if (debugModeOn()) eventLog.AddEvent("PlayerManager: Didn't jump. isGrounded=" + isGrounded + ", canDoubleJump=" + canDoubleJump);
				_playerView.isJumping = false;
				_playerView.isDoubleJumping = false;

			}
		}

		private void shoot(Vector2 direction)
		{
			if (_timesSinceFired > 0) return;
			_timesSinceFired = turnsBetweenShots;

			Vector2 pos = _rigidbody2D.position;
			_gameManager.SpawnShot(pos, _rigidbody2D.velocity, direction.GetAngle(), _playerState.player_framework);

			// recoil
			_rigidbody2D.MovePosition(new Vector3(pos.x - direction.x * recoil, pos.y - direction.y * recoil, transform.position.z));

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
				if (other.gameObject.CompareTag(Values.PLATFORM_BODY_TAG))
				{
					ConnectToPlatform(other.gameObject);
				}
			}
		}

		private void OnCollisionExit2D(Collision2D other)
		{
			if (other.gameObject.CompareTag(Values.PLATFORM_BODY_TAG))
			{
				DisconnectFromPlatfrom();
			}
		}

		private void OnTriggerExit2D(Collider2D other)
		{
			if (other.CompareTag(Values.PLATFORM_BODY_TAG))
			{
				var edgeCollider = other.gameObject.GetComponent<EdgeCollider2D>();
				if (edgeCollider != null) edgeCollider.enabled = true;
			}
			if (other.CompareTag(Values.BOUNDRIES_TAG))
			{
				if (invincible) return;
				_gameManager.PlayerKilled(gameObject);
				Debug.Log(gameObject.name + ": Killed");
			}
		}

		private void ConnectToPlatform(GameObject platform)
		{
			_playerState.currentPlatform = platform;
		}

		private void DisconnectFromPlatfrom()
		{
			_playerState.currentPlatform = null;
		}

		private void getOffPlatform()
		{
			if (_playerState.currentPlatform != null)
			{
				var edgeCollider = _playerState.currentPlatform.gameObject.GetComponent<EdgeCollider2D>();
				if (edgeCollider != null) edgeCollider.enabled = false;			
			}
		}

		private bool debugModeOn()
		{
			return debugMode && eventLog != null;
		}
	}
}

