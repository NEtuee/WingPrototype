using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class PatternPresetDatabase : ScriptableObject {

	[System.Serializable]
	public class PatternPreset
	{
		public string name;
		public List<ShotInfo> shots = new List<ShotInfo>();

		public PatternPreset(){}
		public PatternPreset(string n) {name = n;}
	}

	public List<PatternPreset> data = new List<PatternPreset>();

	public string[] GetNames()
	{
		if(data.Count == 0)
			return null;

		List<string> list = new List<string>();

		for(int i = 0; i < data.Count; ++i)
		{
			list.Add(data[i].name);
		}

		return list.ToArray();
	}

	public bool DeleteShotInfo(int dataIndex,int index)
	{
		if(!ExistsCheck(dataIndex,index))
		{
			Debug.Log("out of range");
			return false;
		}

		data[dataIndex].shots.RemoveAt(index);
		return true;
	}

	public void AddShotInfo(int index, ShotInfo info)
	{
		if(!ExistsCheck(index))
		{
			Debug.Log("out of range");
			return;
		}

		data[index].shots.Add(info);
	}

	public void DeletePatternPreset(int index)
	{
		if(!ExistsCheck(index))
		{
			Debug.Log("out of range");
			return;
		}

		data.RemoveAt(index);
	}

	public void AddPatternPreset(string n)
	{
		// if(!NameOverlapCheck(n))
		// {
		// 	Debug.Log("name already exists");
		// 	return;
		// }
		data.Add(new PatternPreset(n));
	}

	public bool ExistsCheck(int dataIndex)
	{
		if(data.Count > dataIndex)
			return true;

		return false;
	}

	public bool ExistsCheck(int dataIndex, int shotIndex)
	{
		if(ExistsCheck(dataIndex))
		{
			if(data[dataIndex].shots.Count > shotIndex)
			{
				return true;
			}
		}

		return false;
	}

	public bool NameOverlapCheck(string n)
	{
		Debug.Log(n);
		for(int i = 0; i < data.Count; ++i)
		{
			if(data[i].name.CompareTo(n) == 0)
			{
				Debug.Log(data[i].name);
				Debug.Log(n);
				return false;
			}
		}

		return true;
	}

#if UNITY_EDITOR
	[MenuItem("ScriptableObjs/PatternPresetDatabase")]
	public static PatternPresetDatabase Create()
	{
		PatternPresetDatabase asset = ScriptableObject.CreateInstance<PatternPresetDatabase>();
		AssetDatabase.CreateAsset(asset,"Assets/PatternPresetDatabase.asset");
		AssetDatabase.SaveAssets();

		return asset;
	}
#endif
}
