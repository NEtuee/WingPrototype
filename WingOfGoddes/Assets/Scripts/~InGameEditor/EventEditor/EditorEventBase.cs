using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class EditorEventBase {

	public SpriteRenderer marker;

	public abstract void OnCreate();
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

public class TestEvent : EditorEventBase
{
	public override void OnCreate(){}
	public override void Progress(){Debug.Log("progress");}
	public override void PickEvent(){Debug.Log("pick");}
	public override void ReleasePick(){Debug.Log("release");}
	public override void ButtonClicked() {}
	public override void ButtonRelease(){}
	public override string DataToString(){return "";}
	public override string DataToStringForGame(){return "";}
	public override void StringToData(string s){}
}