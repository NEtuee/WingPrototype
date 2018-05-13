using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class PatternDatabase : ScriptableObject {

	[System.Serializable]
	public class PatternInfo
	{
		public string name;
		public List<PatternFrameInfo> frames = new List<PatternFrameInfo>();

		public PatternInfo(){}
		public PatternInfo(string s) {name = s;}
	}

	public List<PatternInfo> data = new List<PatternInfo>();

	public void FrameSort(int dataIndex)
	{
		if(!ExistsCheck(dataIndex))
		{
			PopupWindow.instance.Active("Pattern does not Exists",Color.white);
			return;
		}

		data[dataIndex].frames.Sort(delegate(PatternFrameInfo a, PatternFrameInfo b)
		{
			if(a.frame > b.frame)
				return 1;
			else if(a.frame < b.frame)
				return -1;
			
			return 0;
		});
	}

	public void AddPatternPreset(int dataIndex, int frameIndex, PatternFrameInfo.Preset preset)
	{
		if(!ExistsCheck(dataIndex))
		{
			PopupWindow.instance.Active("Pattern does not Exists",Color.white);
			return;
		}

		PatternFrameInfo info = FindFrame(dataIndex,frameIndex);

		if(info == null)
		{
			info = AddFrame(dataIndex,frameIndex);
			info.presets.Add(preset);
		}
		else
		{
			info.presets.Add(preset);
		}
	}

	public PatternFrameInfo GetFrame(int dataIndex, int frameIndex)
	{
		if(!ExistsCheck(dataIndex,frameIndex))
		{
			PopupWindow.instance.Active("Pattern does not Exists",Color.white);
			return null;
		}

		return data[dataIndex].frames[frameIndex];
	}

	public PatternFrameInfo FindFrame(int dataIndex, int frame)
	{
		if(!ExistsCheck(dataIndex))
		{
			PopupWindow.instance.Active("Pattern does not Exists",Color.white);
			return null;
		}

		for(int i = 0; i < data[dataIndex].frames.Count; ++i)
		{
			if(data[dataIndex].frames[i].frame == frame)
			{
				return data[dataIndex].frames[i];
			}
		}

		return null;
	}

	public PatternFrameInfo AddFrame(int dataIndex, int frameIndex)
	{
		if(!ExistsCheck(dataIndex))
		{
			PopupWindow.instance.Active("Pattern does not Exists",Color.white);
			return null;
		}

		PatternFrameInfo info = new PatternFrameInfo(frameIndex);
		
		data[dataIndex].frames.Add(info);
		FrameSort(dataIndex);

		return info;
	}

	public void AddPattern(string n)
	{
		data.Add(new PatternInfo(n));
	}

	public void DeleteFrame(int index, int frameIndex)
	{
		if(!ExistsCheck(index,frameIndex))
		{
			PopupWindow.instance.Active("does not Exists",Color.white);
			return;
		}

		data[index].frames.RemoveAt(frameIndex);
	}

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

	public bool NameOverlapCheck(string n)
	{
		for(int i = 0; i < data.Count; ++i)
		{
			if(data[i].name.CompareTo(n) == 0)
			{
				return false;
			}
		}

		return true;
	}

	public bool ExistsCheck(int dataIndex, int frameIndex)
	{
		if(ExistsCheck(dataIndex))
		{
			if(data[dataIndex].frames.Count > frameIndex)
				return true;
		}

		return false;
	}

	public bool ExistsCheck(int dataIndex)
	{
		if(data.Count > dataIndex)
			return true;
		
		return false;
	}

#if UNITY_EDITOR
	[MenuItem("ScriptableObjs/PatternDatabase")]
	public static PatternDatabase Create()
	{
		PatternDatabase asset = ScriptableObject.CreateInstance<PatternDatabase>();
		AssetDatabase.CreateAsset(asset,"Assets/PatternDatabase.asset");
		AssetDatabase.SaveAssets();

		return asset;
	}
#endif
}
