using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;

public class PlayerSFX : MonoBehaviour
{
	public AudioSource Jump, DoubleJump, Shoot, Death;
	
	public void PlayJump()
	{
		Jump.Play();
	}
	
	public void PlayDoubleJump()
	{
		DoubleJump.Play();
	}
	
	public void PlayShoot()
	{
		Shoot.Play();
	}
	
	public void PlayDeath()
	{
		Death.Play();
	}
}
