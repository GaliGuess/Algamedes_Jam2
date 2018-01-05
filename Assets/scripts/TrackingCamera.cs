using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Found on:
// http://nielson.io/2014/03/making-a-target-tracking-orthographic-camera-in-unity/


public class TrackingCamera : MonoBehaviour
{	
	[SerializeField] private Transform[] _targets;

	private float boundingBoxPadding = 2f;

	[SerializeField] float minimumOrthographicSize = 8f;

	[SerializeField] float maximumOrthographicSize = 22f;
	private Vector3 MAX_CAMERA_POSITION = new Vector3(0, 17, -10);
	
	[SerializeField] float zoomSpeed = 20f;
	
	[SerializeField] float viewPercentage = 1.5f;

//	public Rect currentBoundingBox;
	private Rect maxBoundingBox;
	
	private Camera _camera;
	

	void Start()
	{
		_camera = GetComponent<Camera>();
		_camera.orthographic = true;

		maxBoundingBox = boudingBoxFromOrthographicSize(maximumOrthographicSize, MAX_CAMERA_POSITION);
	}

	void LateUpdate()
	{
		Rect boundingBox = CalculateTargetsBoundingBox();
		boundingBox = scaleRect(boundingBox, viewPercentage);

		// not 
//		boundingBox = shiftToBoundries(boundingBox);
		
		float newSize = CalculateOrthographicSize(boundingBox);
		Vector3 newPosition = CalculateCameraPosition(boundingBox);
		
//		currentBoundingBox = boundingBox;
		
		transform.position = newPosition;
		_camera.orthographicSize = newSize;
	}


	/// <summary>
	/// Shifts a bounding box so it doesn't go outside the maxBoundingBox.
	/// </summary>
	/// <param name="boundingBox">a Rect</param>
	/// <returns>a Rect</returns>
	Rect shiftToBoundries(Rect boundingBox)
	{
		if (boundingBox.x < maxBoundingBox.x)
		{
			boundingBox.x = maxBoundingBox.x;
//			Debug.Log("touched left boundry");
		}
		if (boundingBox.xMax > maxBoundingBox.xMax)
		{
			boundingBox.x -= boundingBox.xMax - maxBoundingBox.xMax;
//			Debug.Log("touched right boundry");
		}
		if (boundingBox.yMax < maxBoundingBox.yMax)
		{
			boundingBox.y += maxBoundingBox.yMax - boundingBox.yMax;
//			Debug.Log("touched bottom boundry");
		}
		return boundingBox;
	}
	
	
	Rect scaleRect(Rect rectangle, float scale)
	{
		float x = rectangle.center.x;
		float y = rectangle.center.y;
		float width = rectangle.width * scale / 2;
		float height = rectangle.height * scale / 2;
		
		return Rect.MinMaxRect(x - width, y - height, x + width, y + height);
	}
	
	
	/// <summary>
	/// Calculates a bounding box that contains all the targets.
	/// </summary>
	/// <returns>A Rect containing all the targets.</returns>
	Rect CalculateTargetsBoundingBox()
	{
		float minX = Mathf.Infinity;
		float maxX = Mathf.NegativeInfinity;
		float minY = Mathf.Infinity;
		float maxY = Mathf.NegativeInfinity;

		foreach (Transform target in _targets) {
			Vector3 position = target.position;

			minX = Mathf.Min(minX, position.x);
			minY = Mathf.Min(minY, position.y);
			maxX = Mathf.Max(maxX, position.x);
			maxY = Mathf.Max(maxY, position.y);
		}

		return Rect.MinMaxRect(minX - boundingBoxPadding, maxY + boundingBoxPadding, maxX + boundingBoxPadding, minY - boundingBoxPadding);
	}

	/// <summary>
	/// Given an orthographic size and center position, this returns the resulting bounding box.
	/// </summary>
	/// <param name="orthographicSize"> A float camera orthographic size. </param>
	/// <param name="position"> The camera's central position as Vector3. </param>
	/// <returns>A Rect containing all the targets.</returns>
	Rect boudingBoxFromOrthographicSize(float orthographicSize, Vector3 position)
	{			
		float yMin = position.y + orthographicSize;
		float yMax = position.y - orthographicSize;

		float xMin = position.x - orthographicSize * _camera.aspect;
		float xMax = position.x + orthographicSize * _camera.aspect;
		
		return Rect.MinMaxRect(xMin, yMin, xMax, yMax);
	}
	
	/// <summary>
	/// Calculates a new orthographic size for the camera based on the target bounding box.
	/// </summary>
	/// <param name="boundingBox">A Rect bounding box containg all targets.</param>
	/// <returns>A float for the orthographic size.</returns>
	float CalculateOrthographicSize(Rect boundingBox)
	{
		float orthographicSize;
		Vector3 topRight = new Vector3(boundingBox.x + boundingBox.width, boundingBox.y, -10f);
		Vector3 topRightAsViewport = _camera.WorldToViewportPoint(topRight);
       
		if (topRightAsViewport.x >= topRightAsViewport.y)
			orthographicSize = Mathf.Abs(boundingBox.width) / _camera.aspect / 2f;
		else
			orthographicSize = Mathf.Abs(boundingBox.height) / 2f;
		
		return Mathf.Clamp(Mathf.Lerp(_camera.orthographicSize, orthographicSize, Time.deltaTime * zoomSpeed), 
						   minimumOrthographicSize, maximumOrthographicSize);  // Mathf.Infinity
	}
	
	/// <summary>
	/// Calculates a camera position given the a bounding box containing all the targets.
	/// </summary>
	/// <param name="boundingBox">A Rect bounding box containg all targets.</param>
	/// <returns>A Vector3 in the center of the bounding box.</returns>
	Vector3 CalculateCameraPosition(Rect boundingBox)
	{
		Vector2 boundingBoxCenter = boundingBox.center;

		return new Vector3(boundingBoxCenter.x, boundingBoxCenter.y, _camera.transform.position.z);
	}
}
