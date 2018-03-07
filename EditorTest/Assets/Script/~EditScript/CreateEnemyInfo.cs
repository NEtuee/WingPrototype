using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateEnemyInfo : MonoBehaviour {

	public InputField enemyCode;
	public InputField pathCode;

	public string GetInfo()
	{
		return (enemyCode.text == "" ? "0" : enemyCode.text) + "/" + 
				(pathCode.text == "" ? "0" : pathCode.text);
	}
}
