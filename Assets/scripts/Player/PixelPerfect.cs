using UnityEngine;
using System.Collections;
using System;
 
[ExecuteInEditMode]
public class PixelPerfect : MonoBehaviour {
     
	private int pixelsPerUnit = 5;
	private float snapValue = 1;
     
	// Use this for initialization
	void Start () {
		snapValue = 1f / pixelsPerUnit;
	}
     
	// Update is called once per frame
	void LateUpdate () {
		if(Application.isPlaying == false) {
			snapValue = 1f / pixelsPerUnit;
		}
		Vector3 pos = transform.parent.position;
		pos = new Vector3((float)Math.Round(pos.x / snapValue) * snapValue, (float)Math.Round(pos.y / snapValue) * snapValue, (float)Math.Round(pos.z / snapValue) * snapValue);
		transform.position = pos;
	}
}