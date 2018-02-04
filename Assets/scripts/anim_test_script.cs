using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class anim_test_script : MonoBehaviour
{

	public bool pressed;
	private float frame;
	private string curr_name;

	private Animator _animator; 
	
	// Use this for initialization
	void Start ()
	{
		_animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Input.GetKey(KeyCode.Space))
		{
			if (!pressed)
			{
				updateAnimState();
				pressed = true;
				_animator.SetBool("pressed_on", pressed);
				_animator.Play(curr_name, -1, frame);
			}
		}
		else
		{
			if (pressed)
			{
				updateAnimState();
				pressed = false;
				_animator.SetBool("pressed_on", pressed);
				_animator.Play(curr_name, -1, frame);
			}
		}
	}

	private void updateAnimState()
	{
		AnimatorClipInfo[] animationClip = _animator.GetCurrentAnimatorClipInfo(0);
		curr_name = animationClip[0].clip.name;
		frame = animationClip[0].weight;
	}
}
