using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour {

	public static EffectManager instance;

	public GameObject origin;
	public SpriteContainer spriteContainer;

	public class ObjectLink
	{
		public SpriteAnimation me;
		public ObjectLink back;

		public ObjectLink(){}
		public ObjectLink(SpriteAnimation b)
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

	public void SetActiveFirst(ObjectLink link)
	{
		link.back = activeFront;
		activeFront = link;
	}

	public void CreateObjects(int count)
	{
		instance = this;

		ObjectLink prev = null;
		for(int i = 0; i < count; ++i)
		{
			SpriteAnimation b = GameObject.Instantiate(origin,Vector3.zero,Quaternion.identity).GetComponent<SpriteAnimation>();

			b.Initialize();
			b.gameObject.name = i.ToString();

			ObjectLink link = new ObjectLink(b);
			if(i == 0)
				disableFront = link;
			else
				prev.back = link;
			
			b.gameObject.SetActive(false);
			prev = link;
		}
	}

	public bool Progress()
	{
		ObjectLink link = activeFront;
		ObjectLink _front = link;

		if(link == null)
			return false;

		// int count = 0;

		while(true)
		{
			// if(++count > 100)
			// {
			// 	Debug.Log("loop err");
			// 	break;
			// }

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
			// else
			// {
			// 	break;
			// }
		}

		return true;
	}

	public void DisableAllObjects()
	{
		ObjectLink link = activeFront;

		if(link == null)
			return;

		while(true)
		{
			link.me.gameObject.SetActive(false);
			ObjectLink save = link;
			
			link = link.back;

			save.back = disableFront;
			disableFront = save;

			if(link == null)
			{
				activeFront = null;
				activeBack = null;
				break;
			}
		}
	}

	public void ValueInit(SpriteAnimation target)
	{
		target.tp.localRotation = Quaternion.identity;
	}

	public void ObjectActive(Vector2 pos,int ani, float sp = 0.0625f, float angle = -999f)
	{
		if(disableFront == null)
		{
			return;
		}

		disableFront.me.SetAnimation(pos,DatabaseContainer.instance.spriteDatabase.aniSet[ani]);
		ValueInit(disableFront.me);
		disableFront.me.speed = sp;

		ObjectLink save = disableFront.back;
		disableFront.back = null;
		if(angle != -999f)
			disableFront.me.tp.localRotation = Quaternion.Euler(0f,0f,angle);

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

	public void ObjectActive(Vector2 pos,SpriteContainer.AnimationSet ani, float sp = 0.0625f, float angle = -999f)
	{
		if(disableFront == null)
		{
			return;
		}

		disableFront.me.SetAnimation(pos,ani);
		ValueInit(disableFront.me);
		disableFront.me.speed = sp;

		ObjectLink save = disableFront.back;
		disableFront.back = null;

		if(angle != -999f)
			disableFront.me.tp.localRotation = Quaternion.Euler(0f,0f,angle);

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

	public void ObjectDisable(ObjectLink prev,ObjectLink target)
	{
		if(target == activeFront)
		{
			activeFront = target.back;
			target.back = disableFront;
			disableFront = target;
		}
		else
		{
			prev.back = target.back;
			target.back = disableFront;
			disableFront = target;
		}
	}
}
