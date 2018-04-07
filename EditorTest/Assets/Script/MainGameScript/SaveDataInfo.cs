using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveDataInfo {

    [System.Serializable]
    public class CharacterStatSaveInfo
    {
        private int atkLevel;
        private int feverAtkLevel;

        private int atkMax = 10;
        private int feverMax = 10;

        public CharacterStatSaveInfo() { atkLevel = 1; feverAtkLevel = 1; }
        public CharacterStatSaveInfo(int al, int fl) { atkLevel = al; feverAtkLevel = fl; }

        public bool AtkLevelUp()
        {
            if (atkLevel >= atkMax)
                return false;
            else
                atkLevel++;

            return true;
        }

        public bool FeverLevelUp()
        {
            if (feverAtkLevel >= feverMax)
                return false;
            else
                feverAtkLevel++;

            return true;
        }
        // public float GetAttack() { return atk[atkLevel]; }
        // public float GetFever() { return fever[feverAtkLevel]; }

		public int GetAttackLevel() {return atkLevel;}
		public int GetFeverLevel() {return feverAtkLevel;}

        public string DataToString()
        {
            return atkLevel.ToString() + ", " + feverAtkLevel.ToString(); 
        }

        public void StringToData(string data)
        {
            data = data.Replace(" ", "");
            string[] datas = data.Split(',');

            atkLevel = int.Parse(datas[0]);
            feverAtkLevel = int.Parse(datas[1]);
        }
    }

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
				return true;
			}
			else if(min == m && sec > s)
			{
				return true;
			}
			else if(min == m && sec == s && rest > r)
			{
				return true;
			}
			return false;
		}

		public bool Compare(int w, int s) {return world == w && stage == s;}
	}

	[System.Serializable]
	private class RequiredAmount
	{
		public int gold;
		public int some;

		public bool Compare(int g,int s) {return g >= gold ? (s >= some ? true : false) : false;}

		public RequiredAmount(){}
		public RequiredAmount(int g,int s) { gold = g; some = s;}
	}

	private static RequiredAmount[] atkAmount = 
	{
		new RequiredAmount(100,0), //1
		new RequiredAmount(500,0), //2
		new RequiredAmount(1000,0), //3
		new RequiredAmount(1200,10), //4 
		new RequiredAmount(1500,50), //5
		new RequiredAmount(2000,100), //6
		new RequiredAmount(2400,120), //7 
		new RequiredAmount(3000,150), //8
		new RequiredAmount(3500,180), //9
		new RequiredAmount(4000,2000), //10
	};

	private static RequiredAmount[] feverAmount = 
	{
		new RequiredAmount(100,0), //1
		new RequiredAmount(500,0), //2
		new RequiredAmount(1000,0), //3
		new RequiredAmount(1200,10), //4 
		new RequiredAmount(1500,50), //5
		new RequiredAmount(2000,100), //6
		new RequiredAmount(2400,120), //7 
		new RequiredAmount(3000,150), //8
		new RequiredAmount(3500,180), //9
		new RequiredAmount(4000,2000), //10
	};

	public int characterSelect = 0;
	public int level = 0;
	public int gold = 0;
	public int zem = 0;
	public float exp = 0f;

	public string userName = "";

	public List<StageSaveInfo> stageSave = new List<StageSaveInfo>();
    public List<CharacterStatSaveInfo> charSave = new List<CharacterStatSaveInfo>();

	private float[] reqExp = 
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

	public bool AttackLevelUp()
	{
		CharacterStatSaveInfo st = charSave[characterSelect];

		if(atkAmount[st.GetAttackLevel()].Compare(gold,zem))
		{
			bool b = st.AtkLevelUp();
			if(b)
			{
				SubReq(atkAmount[st.GetAttackLevel() - 1]);
				DataSave();
			}
			else
			{
				Debug.Log("max Level");
				return false;
			}

			return true;
		}

		return false;
	}

	public bool FeverLevelUp()
	{
		CharacterStatSaveInfo st = charSave[characterSelect];

		if(atkAmount[st.GetFeverLevel()].Compare(gold,zem))
		{
			bool b = st.FeverLevelUp();
			if(b)
			{
				SubReq(atkAmount[st.GetFeverLevel() - 1]);
				DataSave();
			}
			else
			{
				Debug.Log("max Level");
				return false;
			}

			return true;
		}

		return false;
	}

	private void SubReq(RequiredAmount req)
	{
		gold -= req.gold;
		zem -= req.some;
	}

	private void PlusReq(RequiredAmount req)
	{
		gold += req.gold;
		zem += req.some;
	}

	public void CreateSaveData()
	{
		characterSelect = 0;
		level = 1;
		exp = 0;
        gold = 0;

		userName = "";

		int count = DatabaseContainer.instance.characterDatabase.data.Length;
		for(int i = 0; i < count; ++i)
			charSave.Add(new CharacterStatSaveInfo());

		DataManager.WriteStringToFile_NoMark(DataToString(),"save.dat");
	}

	public void DataSave()
	{
		DataManager.WriteStringToFile_NoMark(DataToString(),"save.dat");
	}

	public float GetLevelUpPer() {return exp / reqExp[level - 1];}

	public CharacterStatSaveInfo GetCurrCharInfo() {return charSave[characterSelect];}

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
		dataList.Add("zem : " + zem);


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

        count = charSave.Count;
        if(count != 0)
        {
            dataList.Add("charInfo : ");
            for(int i = 0; i < count; ++i)
            {
                dataList.Add(charSave[i].DataToString());
            }
            dataList.Add(")");
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
			else if(splitData[0] == "zem")
			{
				zem = int.Parse(splitData[1]);
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
			else if(splitData[0] == "charInfo")
			{
				++i;
				while(dataArray[i] != ")")
				{
					CharacterStatSaveInfo chSave = new CharacterStatSaveInfo();
					chSave.StringToData(dataArray[i++]);

					Debug.Log (dataArray [i]);
					charSave.Add(chSave);
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
