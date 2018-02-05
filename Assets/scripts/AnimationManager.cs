using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour {

	GameObject chars;

	void Awake() {
		chars = transform.parent.Find("chars").gameObject;
	}

	// Use this for initialization
	void Start () {
		chars.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnDone() {
		chars.SetActive(true);
		gameObject.SetActive(false);
	}
}
