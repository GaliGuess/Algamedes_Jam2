using UnityEngine;  
using System.Collections;  
using UnityEngine.EventSystems;  
using UnityEngine.UI;
 
public class MenuButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
 
	private Image _image;
	private Shader _shader;
	
	private Color currentColor;

	private int textColor;

	
	private void Awake()
	{
		_image = GetComponent<Image>();
//		Debug.Log(_image.name);
//		_image.material.shader = Shader.Find("GUI/Text Shader");
//		currentColor = _image.material.color;
		currentColor = _image.color;

	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		Invert();
	}
 
	public void OnPointerExit(PointerEventData eventData)
	{
		Invert();
	}
	
	
	void Invert() {
		
		currentColor = new Color(1 - currentColor.r,
							     1 - currentColor.g, 
							     1 - currentColor.b, 
								 1);
		
		_image.color = currentColor;
//		_image.material.color = currentColor;
	}
}
