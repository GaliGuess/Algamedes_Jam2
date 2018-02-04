using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class anim_test_script : MonoBehaviour
{

	public bool pressed;
	private float frame;
	private string curr_name;

	private int runningLayer, shootingLayer;

	private Animator _animator; 
	
	// Use this for initialization
	void Start ()
	{
		_animator = GetComponent<Animator>();

		runningLayer = _animator.GetLayerIndex("Base Layer");
		shootingLayer = _animator.GetLayerIndex("shooting");
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Input.GetKey(KeyCode.Space))
		{
//			_animator.SetLayerWeight(runningLayer, 0);
			_animator.SetLayerWeight(shootingLayer, 1);
//				updateAnimState();
//				pressed = true;
//				_animator.SetBool("pressed_on", pressed);
		}
		else
		{
//			_animator.SetLayerWeight(runningLayer, 1);
			_animator.SetLayerWeight(shootingLayer, 0);
//				updateAnimState();
//				pressed = false;
//				_animator.SetBool("pressed_on", pressed);
//				_animator.Play(curr_name, -1, frame);
		}
	}

	private void updateAnimState()
	{
		AnimatorClipInfo[] animationClip = _animator.GetCurrentAnimatorClipInfo(0);
		curr_name = animationClip[0].clip.name;
		frame = animationClip[0].weight;
	}
}
