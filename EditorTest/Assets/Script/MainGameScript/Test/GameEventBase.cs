using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class GameEventBase {

	public abstract void Progress();
	public abstract void StringToData(string s);
}
