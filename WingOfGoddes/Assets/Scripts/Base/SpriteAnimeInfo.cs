using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif


public class SpriteAnimeInfo : ScriptableObject {

	[HideInInspector]
	public List<Define.SpriteInfo> info;

	public float fps;

	public bool loop = false;

	[HideInInspector]
	public AnimationCurve curveInfo;

#if UNITY_EDITOR
	public static SpriteAnimeInfo Create(List<Define.SpriteInfo> i, AnimationCurve curve,float fps,string name, bool l)
	{
		SpriteAnimeInfo asset = ScriptableObject.CreateInstance<SpriteAnimeInfo>();

		asset.info = i;
		asset.curveInfo = curve;
		asset.fps = fps;
		asset.loop = l;

		string path = "Assets/" + name + ".asset";

		AssetDatabase.CreateAsset(asset,path);
		AssetDatabase.SaveAssets();

		return asset;
	}
#endif
}
