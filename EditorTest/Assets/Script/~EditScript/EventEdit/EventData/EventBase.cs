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
	public abstract string DataToString();
	public abstract void StringToData(string s);
}
