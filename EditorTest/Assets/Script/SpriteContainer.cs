using System.Collections;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class SpriteContainer : ScriptableObject {

	[System.Serializable]
	public class AniInfo
	{
		public string name;
		public int start;
		public int end;
		public bool loop;
	}

	[System.Serializable]
	public class AnimationSet
	{
		public string name;
		public Sprite[] sprites;
		public AniInfo[] aniInfo;
	}

	public AnimationSet[] aniSet;

#if UNITY_EDITOR
	[MenuItem("ScriptableObjs/SpriteContainer")]
	public static SpriteContainer Create()
	{
		SpriteContainer asset = ScriptableObject.CreateInstance<SpriteContainer>();
		AssetDatabase.CreateAsset(asset,"Assets/SpriteContainer.asset");
		AssetDatabase.SaveAssets();

		return asset;
	}
#endif

}
