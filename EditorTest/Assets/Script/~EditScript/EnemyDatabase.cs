using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif


public class EnemyDatabase : ScriptableObject {

	[System.Serializable]
	public class BulletInfo
	{
		public int shotPoint;
		public float speed;
		public float angle;
		public bool guided;

		public BulletInfo(){}
		public BulletInfo(int sp,float s,float a,bool g)
		{
			shotPoint = sp;
			speed = s;
			angle = a;

			guided = g;
		}
	}

	[System.Serializable]
	public class BulletFrameInfo
	{
		public List<BulletInfo> bulletInfo;
	}
	
	[System.Serializable]
	public class EnemyInfo
	{
		public string name;
		public int hp;
		public float fps;
		public float patternRate;
		public bool isScoreObj;
		
		public Sprite sprite; // test
		public List<Vector2> shotPoint;
		public BulletFrameInfo[] bullet = new BulletFrameInfo[12];
	}

	[SerializeField]
	public List<EnemyInfo> data = new List<EnemyInfo>();

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
