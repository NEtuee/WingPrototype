using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public abstract class ButtonPressing : UIBehaviour, IPointerDownHandler, IPointerUpHandler {

	public void OnPointerDown( PointerEventData eventData ) 
    {
        ButtonDown();
    }
 
    public void OnPointerUp( PointerEventData eventData ) 
    {
        ButtonUp();
    }

	public abstract void ButtonDown();
	public abstract void ButtonUp();
 
}
