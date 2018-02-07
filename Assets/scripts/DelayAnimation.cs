using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayAnimation : MonoBehaviour {

	public float secondsToDelay = 2;
	private Animator animator;
	
	private void Awake()
	{
		animator = GetComponent<Animator>();
		animator.enabled = false;
	}

	void Start ()
	{
		StartCoroutine(showAnimation());
	}


	IEnumerator showAnimation()
	{
		yield return new WaitForSeconds(secondsToDelay);
		animator.enabled = true;
	}
}
