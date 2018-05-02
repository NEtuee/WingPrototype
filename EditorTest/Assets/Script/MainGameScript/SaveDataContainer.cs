using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveDataContainer : MonoBehaviour {

	public static SaveDataContainer instance;
	public SaveDataInfo saveData;

	public void Start()
	{
		instance = this;
		
		string s = DataManager.ReadStringFromFile_NoSplit("save.dat");
		if(s == null)
		{
			saveData.CreateSaveData();
		}
		else
			saveData.StringToData(s);
	}
}
