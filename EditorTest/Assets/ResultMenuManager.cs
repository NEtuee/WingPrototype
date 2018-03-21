using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ResultMenuManager : MonoBehaviour {

	public Text obtScore;
	public Text obtExp;
	public Text obtGold;
	public Text playTime;

	public void Start()
	{
		obtScore.text = "점수 : " + StageClearInfo.instance.obtExp.ToString();
		obtExp.text = "경험치 : " + StageClearInfo.instance.obtExp.ToString();
		obtGold.text = "획득 골드 : " + StageClearInfo.instance.obtGold.ToString();
		playTime.text = "플레이 타임 : " + StageClearInfo.instance.claerTime;

		int world = StageClearInfo.instance.world;
		int stage = StageClearInfo.instance.stage;

		int min = StageClearInfo.instance.min;
		int sec = StageClearInfo.instance.sec;
		int rest = StageClearInfo.instance.rest;

		SaveDataInfo.StageSaveInfo stageSave = SaveDataContainer.instance.saveData.FindStageSave (world, stage);

        if(StageClearInfo.instance.clear)
        {
			if (stageSave == null)
			{
				SaveDataContainer.instance.saveData.AddClearTime (world, stage, min, sec, rest);
			}
			else if(stageSave != null)
			{
				if(stageSave.TimeCompare(min,sec,rest))
				{
					Debug.Log("check");
					stageSave.min = min;
					stageSave.sec = sec;
					stageSave.rest = rest;
				}
			}

            Debug.Log(StageClearInfo.instance.obtExp);
            SaveDataContainer.instance.saveData.IncreaseExp(StageClearInfo.instance.obtExp);
            SaveDataContainer.instance.saveData.IncreaseGold(StageClearInfo.instance.obtGold);
        }

        SaveDataContainer.instance.saveData.DataSave ();
	}

}
