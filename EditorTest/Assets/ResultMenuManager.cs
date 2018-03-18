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
	}

}
