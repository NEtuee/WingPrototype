using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class WorldDatabase : ScriptableObject {

	[System.Serializable]
	public class StageData
	{
		public TextAsset stageInfo;
		public TextAsset stageEventData;
	}

	[System.Serializable]
	public class WorldData
	{
		public string name;
		public StageData[] stageData;
	}

	public List<WorldData> data;

	#if UNITY_EDITOR
	[MenuItem("ScriptableObjs/WorldDatabase")]
	public static WorldDatabase Create()
	{
		WorldDatabase asset = ScriptableObject.CreateInstance<WorldDatabase>();
		AssetDatabase.CreateAsset(asset,"Assets/WorldDatabase.asset");
		AssetDatabase.SaveAssets();

		return asset;
	}
#endif
}
