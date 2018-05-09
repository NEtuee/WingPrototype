using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : SingletonBase<PlayerManager> {

	public PlayerBase target;

	public Vector3 GetDirection(Vector3 pos)
	{
		return new Vector3();
	}
}
