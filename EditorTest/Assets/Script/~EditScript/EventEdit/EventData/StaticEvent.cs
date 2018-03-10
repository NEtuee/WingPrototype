using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticEvent : EventBase {

	public override void Progress()
	{
		Test.instance.staticEvent = Test.instance.staticEvent ? false : true;
	}
	public override void PickEvent()
	{
		
	}
	public override void ReleasePick()
	{
		
	}
	public override string DataToString()
	{
		return code + ">";	
	}

	public override string DataToStringForGame()
	{
		return DataToString();
	}

	public override void StringToData(string s)
	{
		//Debug.Log(s);
	}

	public StaticEvent()
	{
		code = 4;
	}
}
