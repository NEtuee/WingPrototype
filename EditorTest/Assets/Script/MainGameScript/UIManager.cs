using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : ObjectBase {

	public Text enemyCount;
	public Text bulletCount;

	public Image circulaGague;
	public Image hpbar;
	public Image feverbar;
	public Image specialGague;
	public Button attackButton;
	public Button feverButton;
	public Button specialButton;

	public override void Initialize()
	{
		
	}

	public override void Progress()
	{
		circulaGague.fillAmount = PlayerManager.instance.target.GetCurrFire() / PlayerManager.instance.target.GetFire();
		hpbar.fillAmount = PlayerManager.instance.target.GetCurrHp() / PlayerManager.instance.target.GetHp();
		feverbar.fillAmount = PlayerManager.instance.target.GetCurrFever() / PlayerManager.instance.target.GetFever();
		specialGague.fillAmount = PlayerManager.instance.target.GetCurrSpecial() / PlayerManager.instance.target.GetSpecial();

		feverButton.interactable = PlayerManager.instance.target.GetFeverCheck();
		attackButton.interactable = !PlayerManager.instance.target.GetFeverEnabled();
		specialButton.interactable = PlayerManager.instance.target.GetSpecialCheck();

		enemyCount.text = EnemyManager.instance.count + " :";
		bulletCount.text = GameObjectManager.instance.bulletManager.bulletCount + " :";
	}

	public override void Release()
	{

	}

	public void ActiveFever()
	{
		GameObjectManager.instance.PauseSec(2f);
		PlayerManager.instance.target.ActiveFever();
		feverButton.interactable = false;
		attackButton.interactable = false;
		specialButton.interactable = false;
	}

	public void SpecialMove()
	{
		GameObjectManager.instance.PauseSec(2f);
		PlayerManager.instance.target.ActiveSpecialMove();
		feverButton.interactable = false;
		attackButton.interactable = false;
		specialButton.interactable = false;
	}

	public void ChracterLoad(int value)
	{
		GameObjectManager.instance.GetComponent<DataManager>().PlayerInfoSave(value);
		SceneManager.LoadScene(0);
	}

	public void SetImmortal()
	{
		PlayerManager.instance.target.
			SetImmortal(PlayerManager.instance.target.IsImmortal() ? false : true);
	}

	public void SetAttack()
	{
		EnemyManager.instance.attack = EnemyManager.instance.attack ? false : true;
	}

	public void EnableEnemy()
	{
		//EnemyManager.instance.ObjectActive(0,new Vector3(3.3f,0f,0f),10f);
	}

}
