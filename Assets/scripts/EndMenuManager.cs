using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndMenuManager : MonoBehaviour {

	private Animator titleAnimator;

	void Awake() {
		Transform title = transform.Find("Panel").Find("Title");
		titleAnimator = title.GetComponent<Animator>();
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void setAnimation(int playerId) {
		titleAnimator.SetInteger("winId", playerId);
	}


}
