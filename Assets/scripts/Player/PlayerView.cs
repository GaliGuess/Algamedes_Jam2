using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game;

public class PlayerView : MonoBehaviour {

	private SpriteRenderer _spriteRenderer;
	private Animator _animator;
	
	private Transform crosshair;
	private SpriteRenderer _crosshair_spriteRenderer;

	[SerializeField]
	public bool showCrosshair = true;
	
	[SerializeField, Tooltip("The distance of the crosshair from the player")]
	public float crosshairDistance = 2.5f;

	public bool isJumping;
	public bool isShooting;
	public int horizontal_dir;
	public bool facingLeft;
	
	
	void Awake () {
		Init();
		

	}

	public void Init() {
		_spriteRenderer = GetComponent<SpriteRenderer>();
		_animator = GetComponent<Animator>();

		crosshair = transform.Find(Values.PLAYER_CROSSHAIR_GAMEOBJ_NAME);
		_crosshair_spriteRenderer = crosshair.GetComponent<SpriteRenderer>();
	}

	void FixedUpdate() {
		_animator.SetBool("isJumping", isJumping);
		_animator.SetBool("isShooting", isShooting);
		_animator.SetInteger("movingDir", horizontal_dir);
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
