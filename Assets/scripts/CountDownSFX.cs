using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;

public class CountDownSFX : MonoBehaviour
{
	public AudioSource Beep, Go;

	public void PlayGo()
	{
		Go.Play();
	}

	public void PlayBeep()
	{
		Beep.Play();
	}
}
