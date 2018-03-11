using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameWaitExtinc : GameEventBase {

	public override void Progress()
	{
		GameRunningTest.instance.SetWaitExtinc();
	}
	public override void StringToData(string s)
	{

	}

	public GameWaitExtinc(){}
	public GameWaitExtinc(string s)
	{

	}
}
