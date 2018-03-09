using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class CharacterDatabase : ScriptableObject
{
    [System.Serializable]
    public class CharacterInfo
    {
        public Sprite portrait;
        public string name;
        public string expl;
    }

    public CharacterInfo[] data;

#if UNITY_EDITOR
    [MenuItem("ScriptableObjs/CharacterDatabase")]
    public static CharacterDatabase Create()
    {
        CharacterDatabase asset = ScriptableObject.CreateInstance<CharacterDatabase>();
        AssetDatabase.CreateAsset(asset, "Assets/CharacterDatabase.asset");
        AssetDatabase.SaveAssets();

        return asset;
    }
#endif
}