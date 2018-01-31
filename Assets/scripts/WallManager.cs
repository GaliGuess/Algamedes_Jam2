using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallManager : MonoBehaviour {

	public bool isLeft;

	// Use this for initialization
	void Start () {
		// UnityScript
		float dist = (transform.position - Camera.main.transform.position).z;
		float objectW = transform.localScale.x;
		float leftBorder = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, dist)).x - objectW/2;
		float rightBorder = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, dist)).x + objectW/2;


		float posX = Mathf.Clamp(transform.position.x, leftBorder, rightBorder);
		transform.position = new Vector3(posX,transform.position.y,transform.position.z);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
