using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStaticEvent : GameEventBase {

	public override void Progress()
	{
		GameRunningTest.instance.SetStaticEvent(GameRunningTest.instance.IsStaticEvent() ? false : true);
	}
	public override void StringToData(string s)
	{

	}

	public GameStaticEvent(){}
	public GameStaticEvent(string s)
	{
		//StringToData(s);
	}
}
