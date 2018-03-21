using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldManager : ObjectBase {

	public static GoldManager instance;
	public GameObject origin;

	public Transform goldCollect;

	public class ObjectLink
	{
		public GoldBase me;
		public ObjectLink back;

		public ObjectLink(){}
		public ObjectLink(GoldBase b)
		{
			me = b;
			back = null;
		}

		public void InitObjects()
		{
			me.Initialize();
		}
	}

	private ObjectLink disableFront;
	private ObjectLink activeFront;
	private ObjectLink activeBack;

	private float activeTime = -0.5f;
	private ObjectLink activeLink = null;

	public void CreateObjects(int count)
	{
		ObjectLink prev = null;
		for(int i = 0; i < count; ++i)
		{
			GoldBase b = GameObject.Instantiate(origin,Vector3.zero,Quaternion.identity).GetComponent<GoldBase>();

			b.Initialize();
			b.gameObject.name = "gold_" + i;

			ObjectLink link = new ObjectLink(b);
			if(i == 0)
				disableFront = link;
			else
				prev.back = link;
			
			b.gameObject.SetActive(false);
			prev = link;
		}
	}

	public void ObjectActive(Vector3 pos,int g = 10,bool m = false)
	{
		if(disableFront == null)
		{
			return;
		}

		Vector3 target = goldCollect.position;
		disableFront.me.Active(pos,target,g,m);

		ObjectLink save = disableFront.back;
		disableFront.back = null;

		if(activeFront == null)
		{
			activeFront = disableFront;
			activeBack = activeFront;
		}
		else
		{
			activeBack.back = disableFront;
			activeBack = disableFront;
		}

		if(activeLink == null)
			activeLink = disableFront;

		StageClearInfo.instance.increaseObtainGold(disableFront.me.gold);

		disableFront = save;
	}

	public override void Initialize()
	{
		instance = this;

		CreateObjects(100);
	}

	public override void Progress()
	{
		if(GameRunningTest.instance.IsStaticEvent() && GameRunningTest.instance.dialogActive)
		{
			return;
		}

		ObjectLink link = activeFront;
		ObjectLink _front = link;

		if(link == null)
			return;

		while(true)
		{
			if(link.me.progressCheck)
				link.me.Progress();
				
			if(!link.me.gameObject.activeSelf)
			{

				if(link == activeFront)
				{
					ObjectLink save = link;
					activeFront = link.back;
					_front = link.back;
					link = link.back;

					save.back = disableFront;
					disableFront = save;
				}
				else
				{
					ObjectLink save = link;
					_front.back = link.back;

					if(_front.back == null)
						activeBack = _front;
					link = link.back;

					save.back = disableFront;
					disableFront = save;
				}

				if(link == null)
					break;
				else
					continue;

				
			}
			else if(link != null)
			{
				if(link.back == null)
				{
					break;
				}
				_front = link;
				link = link.back;
			}
		}

		GoldActive();
	}

	public override void Release()
	{

	}

	public void GoldActive()
	{
		if(activeLink == null)
		{
			activeTime = -0.5f;
			return;
		}
		activeTime += Time.deltaTime;

		if(activeTime >= 0.05f)
		{
			activeTime = 0f;

			activeLink.me.Move();
			activeLink = activeLink.back;
		}
	}
}
