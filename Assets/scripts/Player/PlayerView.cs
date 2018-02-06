using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game {
	public class PlayerView : MonoBehaviour
	{
		[SerializeField] 
		public GameObject _animationGameObject;
		
		private SpriteRenderer _spriteRenderer;
		private Animator _animator;

		private Transform crosshair;
		private SpriteRenderer _crosshair_spriteRenderer;

		[SerializeField]
		public bool showCrosshair = true;

		[SerializeField, Tooltip("The distance of the crosshair from the player")]
		public float crosshairDistance = 2.5f;

		[HideInInspector] public bool isJumping;
		[HideInInspector] public bool isDoubleJumping;
		[HideInInspector] public bool isShooting;
		[HideInInspector] public int horizontal_dir;
		[HideInInspector] public bool facingLeft;
		[HideInInspector] public bool isLanding;

		Dictionary<string, int> anim_layers = new Dictionary<string, int>();

		private string anim_idle = "idle_", 
					   anim_idle_shoot = "idle_shoot_", 
					   anim_run = "run_",
					   anim_run_shoot = "run_shoot_";

		private int ANIM_DIR_NUMBER = 5;
		private enum AnimDirections
		{
			UP = 4, 
			UP_DIAG = 3, 
			RIGHT = 2, 
			DOWN_DIAG = 1, 
			DOWN = 0
		}
		
		void Awake () {
			Init();


		}

		private void updateAnimLayerDictionary()
		{
			for (int i = 0; i < ANIM_DIR_NUMBER; i++)
			{
				_animator.GetLayerIndex(anim_idle + i);
				_animator.GetLayerIndex(anim_idle_shoot + i);
				_animator.GetLayerIndex(anim_run + i);
				_animator.GetLayerIndex(anim_run_shoot + i);
			}
		}

		public void Init() {
			_spriteRenderer = _animationGameObject.GetComponent<SpriteRenderer>();
			_animator = _animationGameObject.GetComponent<Animator>();

			crosshair = transform.Find(Values.PLAYER_CROSSHAIR_GAMEOBJ_NAME);
			_crosshair_spriteRenderer = crosshair.GetComponent<SpriteRenderer>();
		}

		private void Start()
		{
			facingLeft = GetComponent<Rigidbody2D>().position.x > 0;
			_spriteRenderer.flipX = facingLeft;
		}

		void Update() {
			_animator.SetBool("isJumping", isJumping);
			_animator.SetBool("isDoubleJumping", isDoubleJumping);
			_animator.SetBool("isShooting", isShooting);
			_animator.SetBool("isLanding", isLanding);
			_animator.SetInteger("movingDir", horizontal_dir);
		}

		private int currentLayer;
		
		private void changeAnimationLayer(Vector2 aimDirection, bool isShooting)
		{
			_animator.SetLayerWeight(_animator.GetLayerIndex("Base Layer"), 0);
			_animator.SetLayerWeight(currentLayer, 0);
		}

		void FixedUpdate() {
			if (facingLeft && horizontal_dir > 0) { //it was facing left and now moving right
				facingLeft = false; // it should face right
			}
			else if (!facingLeft && horizontal_dir < 0) { //it was facing right and now moving left
				facingLeft = true; // it should face left
			}

			_spriteRenderer.flipX = facingLeft;
		}


		public void changeCrosshairDirection(Vector2 direction)
		{
			crosshair.localPosition = new Vector2(direction.x / transform.localScale[0], 
				direction.y / transform.localScale[1]) * crosshairDistance;	
		}

		// currently copied for the demo from PlatformMangager maybe should be in one place?
		public void SetSpriteColor(Framework framework)
		{
			if (!showCrosshair) _crosshair_spriteRenderer.enabled = false;

			switch (framework)
			{
			case Framework.BLACK:
				//				_spriteRenderer.material.color = Color.black;
				_crosshair_spriteRenderer.material.color = Color.black;
				break;

			case Framework.GREY:
				//				_spriteRenderer.material.color = Color.grey;
				_crosshair_spriteRenderer.material.color = Color.grey;
				break;

			case Framework.WHITE:
				//				_spriteRenderer.material.color = Color.white;
				_crosshair_spriteRenderer.material.color = Color.white;
				break;
			}
		}
	}
}

