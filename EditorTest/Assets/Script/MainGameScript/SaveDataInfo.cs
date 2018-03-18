using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveDataInfo {
	public int characterSelect = 0;
	public int level = 0;
	public int gold = 0;
	public float exp = 0f;

	public string userName = "";

	float[] reqExp = 
	{
		500f, // 1
		1000f, // 2
		3000f, // 3
	};

	public void CreateSaveData()
	{
		characterSelect = 0;
		level = 1;
		exp = 0;

		userName = "";

		DataManager.WriteStringToFile_NoMark(DataToString(),"save.dat");
	}

	public void DataSave()
	{
		DataManager.WriteStringToFile_NoMark(DataToString(),"save.dat");
	}

	public float GetLevelUpPer() {return exp / reqExp[level - 1];}

	public string[] DataToString()
	{
		List<string> dataList = new List<string>();

//clearTime : world , stage , 00:00:00
		dataList.Add("character : " + characterSelect);
		dataList.Add("level : " + level);
		dataList.Add("exp : " + exp);
		dataList.Add("username : " + userName);

		dataList.Add("gold : " + gold);

		return dataList.ToArray();
	}

	public void StringToData(string data)
	{
		data = data.Trim();
		data = data.Replace(" ","");
		data = data.Replace("\r","");

		string[] dataArray = data.Split('\n');

		int count = dataArray.Length;
		for(int i = 0; i < count; ++i)
		{
			string[] splitData = dataArray[i].Split(':');
			
			if(splitData[0] == "character")
			{
				characterSelect = int.Parse(splitData[1]);
			}
			else if(splitData[0] == "level")
			{
				level = int.Parse(splitData[1]);
			}
			else if(splitData[0] == "exp")
			{
				exp = float.Parse(splitData[1]);
			}
			else if(splitData[0] == "username")
			{
				userName = splitData[1];
			}
			else if(splitData[0] == "gold")
			{
				gold = int.Parse(splitData[1]);
			}
			// else if(splitData[0] == "")
			// {

			// }
			// else if(splitData[0] == "")
			// {

			// }
			// else if(splitData[0] == "")
			// {

			// }
			// else if(splitData[0] == "")
			// {

			// }
		}
	}
}
