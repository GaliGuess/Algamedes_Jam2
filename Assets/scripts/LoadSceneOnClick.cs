using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;

public class LoadSceneOnClick : MonoBehaviour
{

	public AudioSource audioSource;
	public int sceneToLoad;
	public KeyCode[] triggerButtons;
	
	private String[] controllerButtons = {"J1_XBOX_Start", "J2_XBOX_Start"};
	private SceneLoader sceneLoader;
	
	void Start ()
	{
		sceneLoader = GetComponent<SceneLoader>();
	}
	
	void Update () {
		if ((triggerButtons == null || triggerButtons.Length == 0) && 
		    (controllerButtons == null || controllerButtons.Length == 0))
			return;

		foreach (var key in triggerButtons)
		{
			if (Input.GetKeyDown(key))
			{
				LoadScene();
			}
		}
		
		foreach (var key in controllerButtons)
		{
			if (Input.GetButtonDown(key))
			{
				LoadScene();
			}
		}
	}

	private void LoadScene()
	{
		audioSource.Play();
		sceneLoader.LoadScene(sceneToLoad);
	}
}
