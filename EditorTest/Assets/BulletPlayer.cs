using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BulletPlayer : MonoBehaviour {

	public static BulletPlayer instance;

	public BulletManager bulletManager;
	public PlayerManager playerManager;
	public EffectManager effectManager;

	public FakeHpScript bossScript;

	public Button feverButton;

	public int bulletCount = 1000;

	public int charCount = 0;

	public bool stop = false;

	public void Start()
	{
		instance = this;
		bulletManager.CreateObjects(bulletCount);

		Instantiate(DatabaseContainer.instance.characterDatabase.
					data[charCount].objectSet,
					Vector3.zero,Quaternion.identity);

		playerManager.Initialize();
		playerManager.target.Initialize();

		bossScript.Initialize();

		effectManager.CreateObjects(100);

		feverButton.interactable = false;
	}

	public void ActiveFever()
	{
		playerManager.FeverActive();
		feverButton.interactable = false;
	}

	public void Update()
	{
		if(Input.GetKeyDown(KeyCode.A))
		{
			effectManager.ObjectActive(Vector3.zero,21,0.02f);
			//bulletManager.InitAngle(270);
			// bulletManager.InitAccel(20f);
			// bulletManager.InitAngleAccel(0f);
		}


		if(PlayerManager.instance.target.feverDirect.IsProgressing())
		{
			PlayerManager.instance.FeverDirectProgress();

			return;
		}
		else if(PlayerManager.instance.IsFever())
		{
			PlayerManager.instance.target.MoveCenter();
			
			PlayerManager.instance.target.feverBase.Progress();

			return;
		}
		else if(PlayerManager.instance.target.feverBase.feverEndDirect.IsProgressing())
		{
			PlayerManager.instance.target.feverBase.feverEndDirect.Progress();

			return;
		}

		if(stop)
			return;

		feverButton.interactable = playerManager.target.GetFeverCheck();

		playerManager.PlayerProgress();
		bossScript.Progress();

		bulletManager.Progress();
		bulletManager.CollisionCheck();

		effectManager.Progress();
	}
}
