using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour {

	public Image expBar;
	public Text userLevel;
	public Text userName;
	public Text gold;

	public void Start()
	{
		Sync();
	}

	public void Sync()
	{
		userLevel.text = SaveDataContainer.instance.saveData.level.ToString();
		userName.text = SaveDataContainer.instance.saveData.userName;
		gold.text = SaveDataContainer.instance.saveData.gold.ToString();

		float width = expBar.sprite.texture.width;
		float per = SaveDataContainer.instance.saveData.GetLevelUpPer();

		expBar.transform.localPosition = new Vector3((-width) + (width * per),0f,0f);
	}
}
