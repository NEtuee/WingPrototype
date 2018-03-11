using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDialog : GameEventBase {

	public int start = 0;
	public int end = 0;

	public override void Progress()
	{
		DialogScript.instance.Active(start,end);
	}
	public override void StringToData(string s)
	{
		string[] data = s.Split('/');
		start = int.Parse(data[0]);
		end = int.Parse(data[1]);
	}

	public GameDialog(){}
	public GameDialog(string s)
	{
		StringToData(s);
	}
}
