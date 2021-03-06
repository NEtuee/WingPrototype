﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectManager : MonoBehaviour {
	
    public static GameObjectManager instance;

	public GameObject[] players;

	public UIManager uIManager;
	public BulletManager bulletManager;
	public EffectManager effectManager;

	public int bulletCount = 100;

    public int objectCount = 0;

	public bool gamePause_Master = false;
	public bool gamePause = false;
	public float pauseTime = 0f;

    public class ObjectLink
	{
    	public ObjectBase[] me;
    	public ObjectLink back = null;
    	public ObjectLink() { }
    	public ObjectLink(ObjectBase[] m, ObjectLink b = null)
    	{
       		me = m;
        	back = b;
    	}
	}

    private ObjectLink frontLink;
    private ObjectLink backLink;

    private void Start()
    {
		Instantiate(DatabaseContainer.instance.characterDatabase.
					data[SaveDataContainer.instance.saveData.characterSelect].objectSet,
					Vector3.zero,Quaternion.identity);
		// Instantiate(players[data],Vector3.zero,Quaternion.identity);

        FindObjects();

		//Screen.SetResolution(1280,720,true);

		bulletManager.CreateObjects(bulletCount);
		effectManager.CreateObjects(100);
		EnemyManager.instance.CreateObjects(0,30);
		EnemyManager.instance.CreateObjects(1,10);
		EnemyManager.instance.CreateObjects(2,10);
		uIManager.Initialize();
		//EnemyManager.instance.ObjectActive(0,Vector3.zero,10f);
		//bulletManager.ObjectActive(Vector2.zero,3f,1f,0f);
    }

    private void Update()
    {
		if(Input.GetKey(KeyCode.Z))
		{
			return;
		}

		if(gamePause_Master)
			return;

		if(PlayerManager.instance.target.feverDirect.IsProgressing())
		{
			PlayerManager.instance.FeverDirectProgress();
		}
		else if(PlayerManager.instance.IsFever())
		{
			PlayerManager.instance.target.MoveCenter();
			
			effectManager.Progress();
			
			PlayerManager.instance.target.feverBase.Progress();
		}
		else if(PlayerManager.instance.target.feverBase.feverEndDirect.IsProgressing())
		{
			PlayerManager.instance.target.feverBase.feverEndDirect.Progress();
		}

		uIManager.Progress();

		if(gamePause)
			return;

		if(pauseTime != 0f)
		{
			pauseTime -= Time.deltaTime;
			if(pauseTime <= 0)
			{
				pauseTime = 0f;
				//gamePause = false;
			}
			else
				return;
		}


		if(Input.GetKeyDown(KeyCode.S))
		{
			DialogScript.instance.Active(0,3);
		}
		if(Input.GetKeyDown(KeyCode.D))
		{
			bulletManager.DisableAllObjects();
		}

		Progress();

		if(GameRunningTest.instance.IsStaticEvent() || GameRunningTest.instance.dialogActive)
		{
			return;
		}

		if(!GameRunningTest.instance.directStop)
		{
			PlayerManager.instance.PlayerProgress();
			bulletManager.Progress();
		}

		effectManager.Progress();
		//EnemyManager.instance.Progress();
		bulletManager.CollisionCheck();
    }

    private void OnDestroy()
	{
        instance = null;
    }

    public void CreateObject(GameObject origin,Vector3 pos, Quaternion qtr)
	{
		ObjectBase[] objs = Instantiate(origin,pos,qtr).GetComponents<ObjectBase>() as ObjectBase[];
        int len = objs.Length;

        ObjectLink link = new ObjectLink(objs);
        
		for(int i = 0; i < len; ++i)
			link.me[i].Initialize();

		link.back = frontLink;
		frontLink = link;

		++objectCount;

		// if(frontLink == null)
		// 	frontLink = link;
		// else
		// 	backLink.back = link;

        // ++objectCount;

        // backLink = link;

	}

    public void Progress()
    {
        ObjectLink link = frontLink;
		ObjectLink _front = link;

		if(link == null)
			return;
		
//		int a = 0;

		while(true)
		{
			int c = link.me.Length;
			// ++a;

			// if(a > 100)
			// {
			// 	Debug.Log("loop Err");
			// 	break;
			// }
			for(int i = 0; i < c; ++i)
			{
                if(!link.me[i].gameObject.activeInHierarchy)
                    continue;

				if(link.me[i].progressCheck)
					link.me[i].Progress();
				
				if(link.me[i].destroy)
				{
					GameObject save = link.me[0].gameObject;
					if(_front != null)
					{
						if(link.back == null)
							backLink = _front;
						_front.back = link.back;
					}
					if(frontLink == link)
						frontLink = link.back;
					for(int j = 0; j < c; ++j)
					{
						link.me[j].Release();
						GameObject.Destroy(link.me[j]);
					}
					link.me = null;
					link = link.back;
					GameObject.Destroy(save);

					--objectCount;
					break;
				}
			}

			if(link != null)
			{
				if(link.back == null)
					break;
				_front = link;
				link = link.back;
			}
			else
				break;
		}

    }

    public void FindObjects()
    {
        instance = this;

		frontLink = null;
		backLink = null;
		objectCount = 0;

		Transform[] objs = GameObject.FindObjectsOfType(typeof(Transform)) as Transform[];
        ObjectLink old = new ObjectLink();
		ObjectBase[] bases;

		for(int i = 0; i < objs.Length; ++i)
		{
			if(objs[i].tag == "Player")
				continue;

			bases = objs[i].GetComponents<ObjectBase>() as ObjectBase[];

			if(bases.Length > 0)
			{
				ObjectLink link = new ObjectLink(bases);
				for(int j = 0; j < bases.Length; ++j)
				{
					bases[j].Initialize();
				}
				if(frontLink == null)
					frontLink = link;
				else
					old.back = link;
				old = link;

				++objectCount;
			}
			bases = null;
		}
		backLink = old;
    }

	public void PauseSec(float time)
	{
	//	gamePause = true;
		pauseTime = time;
	}

	public void GamePause(bool value){gamePause = value;}
	public void GamePause(){gamePause = true;}
	public void GameContinue(){gamePause = false;}
	public void GamePause_Master(bool value){gamePause_Master = value;}
	public void GamePause_Master(){gamePause_Master = true;}
	public void GameContinue_Master(){gamePause_Master = false;}

}
