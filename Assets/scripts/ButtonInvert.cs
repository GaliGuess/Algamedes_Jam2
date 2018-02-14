using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonInvert : MonoBehaviour
{
	private Image _image;
	
	
	void Awake ()
	{
		_image = GetComponent<Image>();
	}
	
	void Invert () {
		
		_image.color = new Color(1 - _image.color.r, 1 - _image.color.g, 1 - _image.color.b, _image.color.a);
	}

	private void OnMouseEnter()
	{
		Invert();
	}

	private void OnMouseExit()
	{
		Invert();
	}
}
