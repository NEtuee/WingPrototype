using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class BulletDatabase : ScriptableObject {

	public class Bullet
	{
		public string name;
		Sprite sprite;
		float attack = 1f;
		
		//hit effect
		//kill wish

		public Bullet(){}
		public Bullet(string n, Sprite sp, float at)
		{
			name = n;
			sprite = sp;
			attack = at;
		}
	}

	public List<Bullet> bulletList = new List<Bullet>();

	public void DeleteBullet(int index)
	{
		bulletList.RemoveAt(index);
	}

	public void AddBullet(string n, Sprite sp, float at)
	{
		bulletList.Add(new Bullet(n,sp,at));
	}

	public bool NameOverlapCheck(string n)
	{
		for(int i = 0; i < bulletList.Count; ++i)
		{
			if(bulletList[i].name.CompareTo(n) == 0)
			{
				return false;
			}
		}

		return true;
	}

#if UNITY_EDITOR
	[MenuItem("ScriptableObjs/BulletDatabase")]
	public static BulletDatabase Create()
	{
		BulletDatabase asset = ScriptableObject.CreateInstance<BulletDatabase>();
		AssetDatabase.CreateAsset(asset,"Assets/BulletDatabase.asset");
		AssetDatabase.SaveAssets();

		return asset;
	}
#endif
}
