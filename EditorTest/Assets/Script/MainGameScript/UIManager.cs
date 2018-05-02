using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour {

	public Text enemyCount;
	public Text bulletCount;

	public Image circulaGague;
	public Image hpbar;
	public Image specialGague;
	public Button feverButton;
	public Button specialButton;

	public Transform backGague;

	public float zeroPos = -46.4f;
	public float speed = 3f;

	public void Initialize()
	{
		
	}

	public void Progress()
	{
		circulaGague.fillAmount = PlayerManager.instance.target.GetCurrFire() / PlayerManager.instance.target.GetFire();
		hpbar.fillAmount = PlayerManager.instance.target.GetCurrHp() / PlayerManager.instance.target.GetHp();
		specialGague.fillAmount = PlayerManager.instance.target.GetCurrSpecial() / PlayerManager.instance.target.GetSpecial();

		feverButton.interactable = PlayerManager.instance.target.GetFeverCheck();
		specialButton.interactable = PlayerManager.instance.target.GetSpecialCheck();

		enemyCount.text = EnemyManager.instance.count + " :";
		bulletCount.text = GameObjectManager.instance.bulletManager.bulletCount + " :";

		float gague = zeroPos + (-zeroPos) * PlayerManager.instance.target.GetCurrFeverGague();
		backGague.position = new Vector3(0f,Mathf.Lerp(backGague.position.y, gague, speed * Time.deltaTime));
	}

	public void Release()
	{

	}

	public void ActiveFever()
	{
		//GameObjectManager.instance.PauseSec(2f);
		PlayerManager.instance.FeverActive();
		feverButton.interactable = false;
		specialButton.interactable = false;
	}

	public void SpecialMove()
	{
		GameObjectManager.instance.PauseSec(2f);
		PlayerManager.instance.target.ActiveSpecialMove();
		feverButton.interactable = false;
		specialButton.interactable = false;
	}

	public void ChracterLoad(int value)
	{
		SaveDataContainer.instance.saveData.characterSelect = value;
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
