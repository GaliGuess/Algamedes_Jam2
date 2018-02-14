using UnityEngine;  
using System.Collections;  
using UnityEngine.EventSystems;  
using UnityEngine.UI;
 
public class MenuButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler, IPointerClickHandler, ISubmitHandler {
 
	private Image _image;
	private ButtonSFX _sfx;
	
	private Color currentColor;

	private int textColor;

	
	private void Awake()
	{
		_sfx = GetComponent<ButtonSFX>();
		
		_image = GetComponent<Image>();
		currentColor = _image.color;

	}

	
	// mouse control 
	
	public void OnPointerEnter(PointerEventData eventData)
	{
		_sfx.PlaySelect();
		Invert();
	}
 
	public void OnPointerExit(PointerEventData eventData)
	{
		Invert();
	}
	
	public void OnPointerClick(PointerEventData eventData)
	{
		_sfx.PlaySubmit();
	}

	
	// joystick control 
	
	public void OnSelect(BaseEventData eventData)
	{
		_sfx.PlaySelect();
		Invert();
	}
	
	public void OnDeselect(BaseEventData eventData)
	{
		Invert();
	}

	void ISubmitHandler.OnSubmit(BaseEventData eventData)
	{
		_sfx.PlaySubmit();
	}
	

	void Invert() {
		
		currentColor = new Color(1 - currentColor.r,
							     1 - currentColor.g, 
							     1 - currentColor.b, 
								 currentColor.a);
		
		_image.color = currentColor;
	}
}
