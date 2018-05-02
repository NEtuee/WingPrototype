using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class SpriteDatabase : ScriptableObject {

	[System.Serializable]
	public class SpriteSet
	{
		public string name;
		public Sprite[] sprites;

        public SpriteSet() { }
        public SpriteSet(string n) { name = n; }
    }

    [System.Serializable]
    public class SpriteListWithKey
    {
        public string key;
        public List<SpriteSet> spriteSet = new List<SpriteSet>();

        public SpriteListWithKey() { }
        public SpriteListWithKey(string k) { key = k; }
    }

    //public List<SpriteSet> spriteSet = new List<SpriteSet>();

    public List<SpriteListWithKey> spriteSet = new List<SpriteListWithKey>();

    public Sprite GetSprite(int group, int set, int index)
    {
        return spriteSet[group].spriteSet[set].sprites[index];
    }

#if UNITY_EDITOR
    public int AddSpriteGroup(string groupName)
    {
        if(groupName == "")
        {
            Debug.Log("name is empty");
            return - 1;
        }

        spriteSet.Add(new SpriteListWithKey(groupName));


        EditorUtility.SetDirty(this);

        return spriteSet.Count - 1;
    }

    public int AddSpriteSet(int index, SpriteSet spr)
    {
        if (spr == null)
        {
            Debug.Log("sprites is null");
            return -1;
        }

        if(spr.name == "")
        {
            Debug.Log("name is empty");
            return -1;
        }

        List<SpriteSet> list = FindListFromIndex(index);

        if (list != null)
            list.Add(spr);

        EditorUtility.SetDirty(this);

        return list.Count - 1;
    }

    public int DeleteSpriteGroup(int index)
    {
        if(spriteSet.Count == 0)
            return -2;

        if (spriteSet.Count > index)
            spriteSet.RemoveAt(index);

        EditorUtility.SetDirty(this);
        return spriteSet.Count - 1;
    }

    public int DeleteSpriteSet(int group, int index)
    {
        if(group < 0)
            return -2;

        List<SpriteSet> list = FindListFromIndex(group);
        if (list != null)
        {
            if(list.Count > index)
                list.RemoveAt(index);
            else
                return -2;
        }
        else
        {
            Debug.Log("list is null");
            return -2;
        }

        EditorUtility.SetDirty(this);
        return list.Count - 1;
    }

    public string[] GetKeys()
    {
        List<string> keys = new List<string>();
        for (int i = 0; i < spriteSet.Count; ++i)
        {
            keys.Add(spriteSet[i].key);
        }

        return keys.ToArray();
    }

    public string[] GetNames(int index)
    {
        if(spriteSet.Count -1 < index)
            return null;

        List<string> list = new List<string>();

        for(int i = 0; i < spriteSet[index].spriteSet.Count; ++i)
        {
            list.Add(spriteSet[index].spriteSet[i].name);
        }

        return list.ToArray();
    }

    public Sprite[] GetSprites(int group, int index)
    {
        List<SpriteSet> list = FindListFromIndex(group);

        if(list != null)
        {
            if(list.Count > index)
                return list[index].sprites;
        }

        return null;
    }

    public bool KeyOverlapCheck(string key)
    {
        if (FindListFromKey(key) != null)
            return false;

        return true;
    }
    public bool SpriteSetNameOverlapCheck(int index, string n)
    {
        List<SpriteSet> list = FindListFromIndex(index);

        if (list != null)
        {
            for (int i = 0; i < list.Count; ++i)
            {
                if (list[i].name.CompareTo(n) == 0)
                    return false;
            }
        }
        else
            Debug.Log("list is null");

        return true;
    }

    public void ListClear(string key, int index)
    {
        List<SpriteSet> list = FindListFromKey(key);

        if (list != null)
            list[index].sprites = null;
    }
    public void ListClear(int group, int index)
    {
        List<SpriteSet> list = FindListFromIndex(group);

        if (list != null)
            list[index].sprites = null;
    }

    public List<SpriteSet> FindListFromKey(string key)
    {
        for(int i = 0; i < spriteSet.Count; ++i)
        {
            if(spriteSet[i].key.CompareTo(key) == 0)
            {
                return spriteSet[i].spriteSet;
            }
        }

        return null;
    }
    public List<SpriteSet> FindListFromIndex(int index)
    {
        if(index < 0)
            return null;
            
        if (spriteSet.Count > index)
            return spriteSet[index].spriteSet;

        return null;
    }

    public int GetKeyPosition(string key)
    {
        for (int i = 0; i < spriteSet.Count; ++i)
        {
            if (spriteSet[i].key.CompareTo(key) == 0)
            {
                return i;
            }
        }

        return -1;
    }

	[MenuItem("ScriptableObjs/SpriteDatabase")]
	public static SpriteDatabase Create()
	{
		SpriteDatabase asset = ScriptableObject.CreateInstance<SpriteDatabase>();
		AssetDatabase.CreateAsset(asset,"Assets/SpriteDatabase.asset");
		AssetDatabase.SaveAssets();

		return asset;
	}
#endif
}
