using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageClearInfo : MonoBehaviour {

	public static StageClearInfo instance;

	public int obtGold = 0;
	public float obtExp = 0f;

	public bool clear = false;

	public string claerTime = "";

	public void Start()
	{
		instance = this;
	}

	public void SetInfo(bool c, float t)
	{
		clear = c;

		int min = (int)t / 60;
		int sec = (int)t - (min * 60);
		int rest = (int)((t - (int)t) * 100f);

		claerTime = ValueToString(min,sec,rest);
	}

	public string ValueToString(int m,int s,int r)
	{
		string st = "";

		st += m < 10 ? "0" + m : m.ToString();
		st += " : ";
		st += s < 10 ? "0" + s : s.ToString();
		st += " : ";
		st += r < 10 ? "0" + r : r.ToString();

		return st;
	}

	public void increaseObtainGold(int value) {obtGold += value;}
	public void IncreaseObtainExp(float value) {obtExp += value;}

}
