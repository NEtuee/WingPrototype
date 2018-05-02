using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteAnimation : ObjectBase {

	public SpriteAnimeInfo animeInfo = null;


	private List<Define.SpriteInfo> info;
	private SpriteRenderer spr;
	private DatabaseManager manager;

	private float mainTime = 0f;
	private float speed = 0f;

	private int index = 0;

	private bool loop = false;

	public override void Initialize()
	{
		Set(animeInfo);
	}
	
	public override void Progress(float deltaTime)
	{
		mainTime += speed * deltaTime;

		if(mainTime >= 1f)
		{
			++index;
			if(info.Count > index)
			{
				mainTime = 0f;
				GetSprite();
			}
			else
			{
				InitValue();
				if(!loop)
					progressCheck = false;
				else
					GetSprite();

				return;
			}
		}
	}

	public override void Release()
	{
		
	}

	public void Set(SpriteAnimeInfo i)
	{
		animeInfo = i;

		if(animeInfo == null || animeInfo.info == null)
		{
			this.progressCheck = false;
			return;
		}

		loop = animeInfo.loop;

		manager = DatabaseManager.instance;
		info = animeInfo.info;

		spr = GetComponent<SpriteRenderer>();

		GetSprite();
	}

	public void GetSprite()
	{
		spr.sprite = manager.GetSprite(info[index].group,info[index].set,info[index].index);
		speed = info[index].speed;
	}

	public void InitValue()
	{
		index = 0;
		mainTime = 0f;
	}
}
