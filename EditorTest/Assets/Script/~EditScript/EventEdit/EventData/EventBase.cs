using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class EventBase
{
	public string name;
	public int code;
	public int frameCode;
	
	public abstract void Progress();
	public abstract void PickEvent();
	public abstract void ReleasePick();
	public virtual void ButtonClicked() 
	{
//		Debug.Log("click");
	}
	public virtual void ButtonRelease()
	{
//		Debug.Log("release");
	}
	public abstract string DataToString();
	public abstract string DataToStringForGame();
	public abstract void StringToData(string s);
}
