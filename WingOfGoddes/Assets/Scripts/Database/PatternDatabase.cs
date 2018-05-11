using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class PatternDatabase : ScriptableObject {

	public class PatternInfo
	{
		public string name;
		public List<PatternFrameInfo> frames = new List<PatternFrameInfo>();

		public PatternInfo(){}
		public PatternInfo(string s) {name = s;}
	}

	public List<PatternInfo> data = new List<PatternInfo>();

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
