using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class EnemyDatabase : ScriptableObject {

	[System.Serializable]
	public class ShotPointInfo
	{
		public Vector2 pos;
		public int patternIndex;
		public float patternDelay;

		public ShotPointInfo(){}
		public ShotPointInfo(Vector2 p) {pos = p;}
	}

	[System.Serializable]
	public class EnemyInfo
	{
		public string name;
		public int hp;

		public List<ShotPointInfo> shotPointInfo = new List<ShotPointInfo>();

		public void SetPatternDelay(int shotPointIndex, float delay)
		{
			if(shotPointInfo.Count > shotPointIndex)
			{
				shotPointInfo[shotPointIndex].patternDelay = delay;
			}
		}

		public void DeleteShotPoint(int shotPointIndex)
		{
			if(shotPointInfo.Count > shotPointIndex)
			{
				shotPointInfo.RemoveAt(shotPointIndex);
			}
		}

		public void SetPattern(int shotPointIndex, int patternIndex)
		{
			if(shotPointInfo.Count > shotPointIndex)
			{
				shotPointInfo[shotPointIndex].patternIndex = patternIndex;
			}
		}

		public void AddShotPoint()
		{
			shotPointInfo.Add(new ShotPointInfo());
		}

		public EnemyInfo(){}
		public EnemyInfo(string n) {name = n;}
	}

	public List<EnemyInfo> data = new List<EnemyInfo>();

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

#if UNITY_EDITOR
	[MenuItem("ScriptableObjs/EnemyDatabase")]
	public static EnemyDatabase Create()
	{
		EnemyDatabase asset = ScriptableObject.CreateInstance<EnemyDatabase>();
		AssetDatabase.CreateAsset(asset,"Assets/EnemyDatabase.asset");
		AssetDatabase.SaveAssets();

		return asset;
	}
#endif
}
