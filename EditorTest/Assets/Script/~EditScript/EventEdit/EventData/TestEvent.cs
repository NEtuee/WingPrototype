using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TestEvent : EventBase
{
	public override void Progress()
	{
		Debug.Log(name);
	}
	public override void PickEvent()
	{
		
	}
	public override void ReleasePick()
	{
		
	}
	public override string DataToString()
	{
		return 0 + ">" + name;	
	}

	public override string DataToStringForGame()
	{
		return DataToString();
	}

	public override void StringToData(string s)
	{
		//Debug.Log(s);
	}
	public TestEvent()
	{
		code = 0;
		name = "test";
	}
}