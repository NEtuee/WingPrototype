using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using UnityEngine.UI;
#endif

public class BulletDatabase : ScriptableObject {

	[System.Serializable]
	public class Bullet
	{
		public string name;
		public SpriteDatabase.SpriteIndexInfo spriteInfo;
		public float attack = 1f;

		public Define.RectF bound = new Define.RectF(); // ex
		
		//hit effect
		//kill wish

		public Bullet(){}
		public Bullet(string n, SpriteDatabase.SpriteIndexInfo sp, float at)
		{
			name = n;
			spriteInfo = sp;
			attack = at;
		}
	}

	[System.Serializable]
	public class BulletGroup
	{
		public string name;
		public List<Bullet> list = new List<Bullet>();

		public BulletGroup() {}
		public BulletGroup(string n) {name = n;}
	}

	public List<BulletGroup> bulletList = new List<BulletGroup>();

	public int GetBulletIndex(Bullet bullet)
	{
		for(int i = 0; i < bulletList.Count; ++i)
		{
			for(int j = 0; j < bulletList[i].list.Count; ++j)
			{
				if(bulletList[i].list[j] == bullet)
				{
					Debug.Log(j);
					return j;
				}
			}
		}

		return -1;
	}

	public int GetGroupIndex(Bullet bullet)
	{
		for(int i = 0; i < bulletList.Count; ++i)
		{
			for(int j = 0; j < bulletList[i].list.Count; ++j)
			{
				if(bulletList[i].list[j] == bullet)
				{
					return i;
				}
			}
		}

		return -1;
	}

	public Bullet GetBullet(int group , int index)
	{
		if(bulletList.Count > group)
		{
			if(bulletList[group].list.Count > index)
			{
				return bulletList[group].list[index];
			}
		}

		return null;
	}

	public SpriteDatabase.SpriteIndexInfo GetSpriteInfo(int group, int index)
	{
		if(bulletList.Count > group)
		{
			if(bulletList[group].list.Count > index)
			{
				return bulletList[group].list[index].spriteInfo;
			}
		}

		return null;
	}

	public Dropdown.OptionData[] GetGroupNames()
	{
		if(bulletList.Count == 0)
			return null;

		List<Dropdown.OptionData> list = new List<Dropdown.OptionData>();

		for(int i = 0; i < bulletList.Count; ++i)
		{
			list.Add(new Dropdown.OptionData(bulletList[i].name));
		}

		return list.ToArray();
	}

	public Dropdown.OptionData[] GetListNames(int group)
	{
		if(bulletList.Count <= group)
			return null;
		else if(bulletList[group].list.Count == 0)
			return null;

		List<Dropdown.OptionData> list = new List<Dropdown.OptionData>();

		for(int i = 0; i < bulletList[group].list.Count; ++i)
		{
			list.Add(new Dropdown.OptionData(bulletList[group].list[i].name));
		}

		return list.ToArray();
	}

	public void AddNewBullet(int group, string n, SpriteDatabase.SpriteIndexInfo sp)
	{
		bulletList[group].list.Add(new Bullet(n,sp,1f));
	}

	public void AddNewGroup(string n)
	{
		if(GroupNameOverlapCheck(n))
		{
			bulletList.Add(new BulletGroup(n));
		}
	}

	public void DeleteBullet(int group, int index)
	{
		if(bulletList.Count > group)
		{
			if(bulletList[group].list.Count > index)
			{
				bulletList[group].list.RemoveAt(index);
			}
		}
	}

	public void DeleteGroup(int index)
	{
		if(bulletList.Count > index)
		{
			bulletList.RemoveAt(index);
		}
	}

	public bool GroupNameOverlapCheck(string n)
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

	public bool BulletNameOverlapCheck(int group, string n)
	{
		for(int i = 0; i < bulletList[group].list.Count; ++i)
		{
			if(bulletList[group].list[i].name.CompareTo(n) == 0)
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
