using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndMenuManager : MonoBehaviour {

	private Animator titleAnimator;



	void Awake() {
		Transform title = transform.Find("Panel").Find("Title");
		titleAnimator = title.GetComponent<Animator>();
	}

	// Use this for initialization
	void Start () {
		SetButton();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SetButton() {
		Button firstButton = transform.Find("Panel/Replay button").gameObject.GetComponent<Button>();
		firstButton.Select();
		Debug.Log("EndMenuManager -> SetButton - First button selected: " +firstButton);
	}

	public void setAnimation(int playerId) {
		titleAnimator.SetInteger("winId", playerId);
	}


}
