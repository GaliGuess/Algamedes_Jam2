using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;

public class ButtonSFX : MonoBehaviour
{
	public AudioSource Select, Submit;

	public void PlaySelect()
	{
		Select.Play();
	}

	public void PlaySubmit()
	{
		Submit.Play();
	}
}
