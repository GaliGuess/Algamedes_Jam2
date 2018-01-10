using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class DontDestroyOnReload : MonoBehaviour
{
	private static DontDestroyOnReload instance;
	
	void Awake() {
		if (instance == null)
		{
			instance = this;
			DontDestroyOnLoad(gameObject);
		}
		else if (instance != this)
		{
			Destroy(gameObject);
		}
		
	}
}
