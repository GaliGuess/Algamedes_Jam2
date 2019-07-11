using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LevelMenuController : MonoBehaviour
{

	public AudioSource click;
	public Button[] buttons;
	
	private int selectedButtonIndex;
	private float ANALOG_MOVE_THRESHOLD = 0.3f;
	private bool canInteract = true;
	
	void Start () {
		
	}
	
	void Update ()
	{
		CheckAxis("J1_PS4_LeftStick_Horizontal");
		CheckAxis("J2_PS4_LeftStick_Horizontal");

		CheckButton("J1_XBOX_X");
		CheckButton("J2_XBOX_X");
	}

	private void CheckAxis(String axis)
	{
		if (!canInteract)
			return;
		
		var axisValue = Input.GetAxis(axis);
		if (Mathf.Abs(axisValue) > ANALOG_MOVE_THRESHOLD)
		{
			canInteract = false;
			StartCoroutine(MenuChange(axisValue));
		}
	}
	
	private void CheckButton(String button)
	{
		if (Input.GetButtonDown(button))
		{
			click.Play();
			buttons[selectedButtonIndex].onClick.Invoke();
		}
	}

	IEnumerator MenuChange(float axisValue)
	{		
		selectedButtonIndex = Mod(selectedButtonIndex + (int) Mathf.Sign(axisValue), buttons.Length);
		buttons[selectedButtonIndex].Select();  
		
		yield return new WaitForSecondsRealtime(0.1f);
		canInteract = true;   // After the wait is over, the player can interact with the menu again.
	}

	int Mod(int x, int m) 
	{
		int r = x % m;
		return r < 0 ? r + m : r;
	}
}
