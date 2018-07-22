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

	void OnEnable() {
		Time.timeScale = 0.0f;
	}

	void OnDisable() {
		Time.timeScale = 1.0f;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown("Cancel")) {
			Debug.Log("EndMenuManager: cancel button!");
			gameObject.SetActive(false);
		}
	}

	public void setAnimation(int playerId) {
		titleAnimator.SetInteger("winId", playerId);
	}


}
