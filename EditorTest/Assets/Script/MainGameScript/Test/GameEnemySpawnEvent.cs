using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEnemySpawnEvent : GameEventBase {

	public int enemyCode;
	public int pathCode;
	public Vector2 pos;

	public override void Progress()
	{
		EnemyManager.instance.ObjectActive(pos,enemyCode,pathCode);
	}

	public override void StringToData(string s)
	{
		string[] data = s.Split('/');

		enemyCode = int.Parse(data[0]);
		pathCode = int.Parse(data[1]);
		pos = new Vector2(float.Parse(data[2]),float.Parse(data[3]));
	}

	public GameEnemySpawnEvent(){}
	public GameEnemySpawnEvent(string s)
	{
		StringToData(s);
	}
}
