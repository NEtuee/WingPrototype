using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTestEvent : GameEventBase {

	public override void Progress()
	{
		Debug.Log("check ");
	}
	public override void StringToData(string s)
	{
//		Debug.Log(s);
	}

	public GameTestEvent(){}
	public GameTestEvent(string s)
	{
		StringToData(s);
	}

}
