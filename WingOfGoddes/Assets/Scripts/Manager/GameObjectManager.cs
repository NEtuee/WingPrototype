using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectManager : LinkProgressManagerBase<GameObjectManager> {

	public int objectCount = 0;

	

	public TouchInputManager touchInputManager;

	public BulletManager bulletManager;

	private ObjectArrayLink<ObjectBase> frontLink;

	private float deltaTime = 0f;

	public void Start()
	{
		Initialize();

		touchInputManager.Initialize();
		bulletManager.Initialize();
	}

	public void Update()
	{
		deltaTime = Time.deltaTime;
		touchInputManager.Progress();

		Progress();

		if(Input.GetMouseButtonDown(0))
		{
			bulletManager.ObjectActive(null,Vector2.zero,5f,1f,180f,false).SetPenetrate(true);
		}

		bulletManager.Progress(deltaTime);



		bulletManager.DeleteAllExitObjects();
	}

	public override void Initialize()
	{
		instance = this;

		FindObjects();
	}
	public override void Progress(float deltaTime = 0f)
    {
        ObjectArrayLink<ObjectBase> link = frontLink;
		ObjectArrayLink<ObjectBase> _front = link;

		if(link == null)
			return;

		while(true)
		{
			int c = link.me.Length;

			for(int i = 0; i < c; ++i)
			{
                if(!link.me[i].gameObject.activeInHierarchy)
                    continue;

				if(link.me[i].ProgressCheck())
					link.me[i].Progress(this.deltaTime);
				
				if(link.me[i].IsDestroy())
				{
					GameObject save = link.me[0].gameObject;
					if(_front != null)
					{
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
		objectCount = 0;

		Transform[] objs = GameObject.FindObjectsOfType(typeof(Transform)) as Transform[];
        ObjectArrayLink<ObjectBase> old = new ObjectArrayLink<ObjectBase>();
		ObjectBase[] bases;

		for(int i = 0; i < objs.Length; ++i)
		{
			bases = objs[i].GetComponents<ObjectBase>() as ObjectBase[];

			if(bases.Length > 0)
			{
				ObjectArrayLink<ObjectBase> link = new ObjectArrayLink<ObjectBase>(bases);
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
    }

	public override void Release()
	{

	}


}
