using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : ObjectBase {

	public static ItemManager instance;
	public GameObject origin;

	public delegate void ItemEffect();

	public class ObjectLink
	{
		public ItemBase me;
		public ObjectLink back;

		public ObjectLink(){}
		public ObjectLink(ItemBase b)
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

	// private ItemEffect[] effects = 
	// {

	// };

	private ItemEffectBase[] effects =
	{
		new Effect_Test(),
	};


	public void CreateObjects(int count)
	{
		ObjectLink prev = null;
		for(int i = 0; i < count; ++i)
		{
			ItemBase b = GameObject.Instantiate(origin,Vector3.zero,Quaternion.identity).GetComponent<ItemBase>();

			b.Initialize();
			b.gameObject.name = "item " + i;

			ObjectLink link = new ObjectLink(b);
			if(i == 0)
				disableFront = link;
			else
				prev.back = link;
			
			b.gameObject.SetActive(false);
			prev = link;
		}
	}

	public void ObjectActive(int num,Vector3 pos)
	{
		if(disableFront == null)
		{
			return;
		}

		disableFront.me.Active(effects[num],pos);

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

		disableFront = save;
	}

	public override void Initialize()
	{
		instance = this;

		CreateObjects(20);
	}

	public override void Progress()
	{
		if(Input.GetKeyDown(KeyCode.A))
		{
			ObjectActive(0,Vector3.zero);
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

	}

	public override void Release()
	{

	}

	public void CollisionCheck(BulletBase obj)
	{
		ObjectLink link = activeFront;

		if(!obj.gameObject.activeSelf)
			return;

		if(link == null)
			return;

		while(true)
		{
			if(link.me.gameObject.activeSelf && link.me.collisionReady)
			{
				if(!obj.gameObject.activeInHierarchy)
					return;

				if(!link.me.gameObject.activeSelf)
					continue;

				if(obj.Collision(link.me))
				{
					link.me.CollisionCheck(obj);
				}
			}

			link = link.back;

			if(link == null)
			{
				break;
			}
		}

	}

	public void Effect_Test()
	{
		MobileDebugger.instance.AddLine("아이템 발동");
	}
}
