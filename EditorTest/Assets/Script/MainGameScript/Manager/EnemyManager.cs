using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyManager : ObjectBase {

	public static EnemyManager instance;
	public GameObject[] enemyBase; //임시
	public int count = 0;

	public PathDatabase pathDatabase;
	public EnemyDatabase enemyDatabase;
	public SpriteContainer spriteContainer;

	public bool attack = false;

	private Dictionary<int,List<EnemyBase>> enemyLists = new Dictionary<int, List<EnemyBase>>();
	private List<int> keys = new List<int>();

	public override void Initialize()
	{
		instance = this;
	}

	public override void Progress()
	{
		if(GameRunningTest.instance.IsStaticEvent() || GameRunningTest.instance.dialogActive || GameRunningTest.instance.directStop)
		{
			return;
		}

		ProgressingAllEnemy();
	}

	public override void Release()
	{

	}

	public void ProgressingAllEnemy()
	{
		int[] keys = enemyLists.Keys.ToArray();
		List<EnemyBase> list;

		int len = keys.Length;
		bool near = PlayerManager.instance.target.GetNearEnemy() == null;
		
		for(int i = 0; i < len; ++i)
		{
			list = enemyLists[keys[i]];
			int c = list.Count;

			for(int j = 0; j < c; ++j)
			{
				if(list[j].progressCheck && list[j].gameObject.activeSelf)
				{
					list[j].Progress();

					if(near)
					{
						if(PlayerManager.instance.target.GetNearEnemy() == null)
						{
							PlayerManager.instance.target.SetNearEnemy(list[j]);
						}
						else	
						{
							float listEnemyDist = Vector3.Distance(PlayerManager.instance.target.tp.position,list[j].tp.position);
							float nearDist = Vector3.Distance(PlayerManager.instance.target.tp.position,
														PlayerManager.instance.target.GetNearEnemy().tp.position);

							if(listEnemyDist < nearDist)
							{
								PlayerManager.instance.target.SetNearEnemy(list[j]);
								PlayerManager.instance.target.SetNearDist(listEnemyDist);
							}
							else
							{
								PlayerManager.instance.target.SetNearDist(nearDist);
							}
						}
					}


				}
			}
		}
	}

	public void AddKey(int key)
	{
		keys.Add(key);
	}

	public void CreateObjects(int code,int count)
	{
		List<EnemyBase> list = new List<EnemyBase>();
		AddKey(code);

		for(int i = 0; i < count; ++i)
		{
			EnemyBase enemy = Instantiate(enemyBase[code],Vector3.zero,Quaternion.identity).GetComponent<EnemyBase>();
			enemy.Initialize();
			enemy.gameObject.SetActive(false);
			enemy.name = code + " : " + i;
			list.Add(enemy);
		}

		enemyLists.Add(code,list);
	}

	public void ObjectActive(Vector3 pos,int enemyCode,int pathCode)
	{
		List<EnemyBase> list = enemyLists[enemyCode];
		int c = list.Count;
		for(int i = 0; i < c; ++i)
		{
			if(!list[i].gameObject.activeSelf)
			{
				++count;
				list[i].SetEnemy(pos,pathDatabase.data[pathCode],enemyDatabase.data[enemyCode]);
				break;
			}
		}
	}

	public void CollisionCheck(BulletBase bullet)
	{
		if(!bullet.gameObject.activeSelf)
			return;
			
		int len = keys.Count;
		for(int i = 0; i < len; ++i)
		{
			List<EnemyBase> list = enemyLists[keys[i]];
			int c = list.Count;

			for(int j = 0; j < c; ++j)
			{
				if(!bullet.gameObject.activeInHierarchy)
					return;

				if(!list[j].gameObject.activeSelf)
					continue;
				if(/*bullet.Collision(list[j]))*/list[j].Collision(bullet))
				{
					list[j].CollisionCheck(bullet);
				}
			}
		}
	}

	public void LineIntersectCheck(BulletBase bullet)
	{
		int len = keys.Count;
		for(int i = 0; i < len; ++i)
		{
			List<EnemyBase> list = enemyLists[keys[i]];
			int c = list.Count;

			for(int j = 0; j < c; ++j)
			{
				if(!list[j].gameObject.activeSelf)
					continue;

				if(bullet.Collision(list[j]))
				{
					//PlayerManager.instance.target.IncreaseFever(1f);
					list[j].CollisionCheck(bullet);
				}
				
				// for(int k = 0; k < lines.Length; ++k)
				// {
				// 	if(list[j].CircleLineIntersect(start,end))
				// 	{
				// 		PlayerManager.instance.target.IncreaseFever(1f);
				// 		list[j].CollisionCheck(bullet);
				// 	}
				// }
			}
		}
	}
}
