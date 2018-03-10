using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitExtinc : EventBase {

	public override void Progress()
	{
		Test.instance.waitExtinc = true;
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

	public WaitExtinc()
	{
		code = 5;
	}
}
