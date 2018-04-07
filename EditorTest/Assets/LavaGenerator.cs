using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaGenerator : StageTrapBase {

	public GameObject lavaOrigin;

	public List<LavaPillar> lavaList = new List<LavaPillar>();

	public float genTime = 1f;

	private float time = 0f;

	public override void Initialize() 
	{
		for(int i = 0; i < 10; ++i)
		{
			LavaPillar lava = Instantiate(lavaOrigin).GetComponent<LavaPillar>();
			lava.Initialize();
			lava.gameObject.SetActive(false);
			lavaList.Add(lava);
		}

		genTime = Random.Range(1f,1f);
	}

	public override void Progress() 
	{
		if(GameRunningTest.instance.obstacleGen)
		{
			time += Time.deltaTime;
			if(time >= genTime)
			{
				time = 0f;
				genTime = Random.Range(1f,1f);
				ObjectActive(new Vector3(0,PlayerManager.instance.target.tp.position.y,0f));
			}
		}

		ObjectProgress();
	}

	public override void Release() 
	{

	}

	public void ObjectProgress()
	{
		int count = lavaList.Count;
		for(int i = 0; i < count; ++i)
		{
			if(lavaList[i].gameObject.activeSelf)
			{
				lavaList[i].Progress();
			}
		}
	}

	public void ObjectActive(Vector3 pos)
	{
		int count = lavaList.Count;
		for(int i = 0; i < count; ++i)
		{
			if(!lavaList[i].gameObject.activeSelf)
			{
				lavaList[i].Active(pos,1f);

				return;
			}
		}
	}
}
