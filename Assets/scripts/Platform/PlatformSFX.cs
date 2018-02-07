using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;

public class PlatformSFX : MonoBehaviour
{
	public AudioSource ChangeColor;

	public void PlayChangeColor()
	{
		ChangeColor.Play();
	}
}
