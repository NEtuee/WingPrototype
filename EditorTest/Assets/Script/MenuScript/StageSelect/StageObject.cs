using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageObject : MonoBehaviour {

	public GameObject clearMark;

//	public SaveDataInfo.StageSaveInfo stageSave;

	public bool isClear = false;
	public Collider2D coll;
	public int stage = 0;

	public void Init()
	{
		if(isClear)
			clearMark.SetActive(true);
	}

	public void SetClear(bool value) {isClear = value;}
}
