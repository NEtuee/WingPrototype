using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveDataInfo {

	[System.Serializable]
	public class StageSaveInfo
	{
		public int world;
		public int stage;
		public int min;
		public int sec;
		public int rest;

		public StageSaveInfo(){}
		public StageSaveInfo(int w, int s, int m , int se, int r)
		{world = w; stage = s; min = m; sec = se; rest = r;}

		public string DataToString()
		{
			return world.ToString() + " , " + stage + " , " + min + " , " + sec + " , " + rest;
		}

		public void StringToData(string data)
		{
			data = data.Replace(" ", "");
			string[] datas = data.Split(',');

			world = int.Parse(datas[0]);
			stage = int.Parse(datas[1]);
			min = int.Parse(datas[2]);
			sec = int.Parse(datas[3]);
			rest = int.Parse(datas[4]); 
		}

		public string ValueToString()
		{	
			string st = "";

			st += min < 10 ? "0" + min : min.ToString();
			st += " : ";
			st += sec < 10 ? "0" + sec : sec.ToString();
			st += " : ";
			st += rest < 10 ? "0" + rest : rest.ToString();

			return st;
		}

		public bool TimeCompare(int m ,int s, int r)
		{
			if(min > m)
			{
				Debug.Log("check1");
				return true;
			}
			else if(min == m && sec > s)
			{
				Debug.Log("check2");
				return true;
			}
			else if(min == m && sec == s && rest > r)
			{
				Debug.Log("check3");
				return true;
			}
			return false;
		}

		public bool Compare(int w, int s) {return world == w && stage == s;}
	}

	public int characterSelect = 0;
	public int level = 0;
	public int gold = 0;
	public float exp = 0f;

	public string userName = "";

	public List<StageSaveInfo> stageSave;

	float[] reqExp = 
	{
		500f, // 1
		1000f, // 2
		3000f, // 3
		5000f, // 4
		10000f, // 5
	};

    public void IncreaseExp(float value)
    {
        exp += value;
        while(exp >= reqExp[level - 1])
        {
             exp -= reqExp[level - 1];
             ++level;
        }
    }

    public void IncreaseGold(int value) { gold += value; }

	public void AddClearTime(int w,int st, int m, int s, int r)
	{
		stageSave.Add(new StageSaveInfo(w,st,m,s,r));
	}

	public StageSaveInfo FindStageSave(int world, int stage)
	{
		int count = stageSave.Count;
		for(int i = 0; i < count; ++i)
		{
			if(stageSave[i].Compare(world,stage))
			{
				return stageSave[i];
			}
		}

		return null;
	}

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

//clearTime : 
//world , stage , min , sec , rest
//world , stage , min , sec , rest
//world , stage , min , sec , rest
//world , stage , min , sec , rest
//)
//dddd : 
		dataList.Add("character : " + characterSelect);
		dataList.Add("level : " + level);
		dataList.Add("exp : " + exp);
		dataList.Add("username : " + userName);

		dataList.Add("gold : " + gold);


		int count = stageSave.Count;
		if(count != 0)
		{
			dataList.Add("clearTime : ");
			for(int i = 0; i < count; ++i)
			{
				dataList.Add(stageSave[i].DataToString());
			}
			dataList.Add (")");
		}

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
			else if(splitData[0] == "clearTime")
			{
				++i;
				while(dataArray[i] != ")")
				{
					StageSaveInfo stSave = new StageSaveInfo();
					stSave.StringToData(dataArray[i++]);


					Debug.Log (dataArray [i]);
					stageSave.Add(stSave);
				}
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
