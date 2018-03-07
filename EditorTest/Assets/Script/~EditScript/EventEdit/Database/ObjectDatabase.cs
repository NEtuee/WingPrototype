using System.Collections;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif


public class ObjectDatabase : ScriptableObject {

	public GameObject[] data;

#if UNITY_EDITOR
	[MenuItem("ScriptableObjs/ObjectDatabase")]
	public static ObjectDatabase Create()
	{
		ObjectDatabase asset = ScriptableObject.CreateInstance<ObjectDatabase>();
		AssetDatabase.CreateAsset(asset,"Assets/ObjectDatabase.asset");
		AssetDatabase.SaveAssets();

		return asset;
	}
#endif
}
