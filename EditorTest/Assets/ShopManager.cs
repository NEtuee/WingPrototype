using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ShopManager : MonoBehaviour {

	public Text msg;

	public Text atkLevel;
	public Text feverLevel;

	public void Start()
	{
		ValueUpdate();
	}

	public void ValueUpdate()
	{
		atkLevel.text = "atk : " + SaveDataContainer.instance.saveData.GetCurrCharInfo().GetAttackLevel().ToString();
		feverLevel.text = "fever : " + SaveDataContainer.instance.saveData.GetCurrCharInfo().GetFeverLevel().ToString();
	}

	public void AttackLevelUp()
	{
		bool b = SaveDataContainer.instance.saveData.AttackLevelUp();
		msg.text = b ? "공격 레벨 업" : "돈 부족 or 만렙";
		ValueUpdate();
	}

	public void FeverAttackUp()
	{
		bool b = SaveDataContainer.instance.saveData.FeverLevelUp();
		msg.text = b ? "피버 레벨 업" : "돈 부족 or 만렙";
		ValueUpdate();
	}
}
