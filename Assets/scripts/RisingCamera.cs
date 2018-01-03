using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RisingCamera : MonoBehaviour
{

	[SerializeField] public float cameraSpeed = .5f; 
	
	private Camera _camera;
	
	void Start ()
	{
		_camera = GetComponent<Camera>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		transform.position += Vector3.up * Time.deltaTime * cameraSpeed;
	}
}
