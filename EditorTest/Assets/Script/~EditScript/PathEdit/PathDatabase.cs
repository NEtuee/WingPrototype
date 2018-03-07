using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class PathDatabase : ScriptableObject {

	[System.Serializable]
	public class LineInfo
	{
		public Vector2 point;
		public Vector2[] bezierPoint = new Vector2[2];
		public float speed = 1f;
		public float stayTime = 0f;
		public bool patternStart = false;
		public MathEx.EaseType type = MathEx.EaseType.Linear;

		public LineInfo(){}
		public LineInfo(Vector2 p)
		{
			point = p;

			bezierPoint[0] = new Vector2();
			bezierPoint[1] = new Vector2();
		}
		public LineInfo(Vector2 p, MathEx.EaseType t, float s)
		{
			point = p;
			type = t;
			speed = s;

			bezierPoint[0] = new Vector2(point.x,point.y);
			bezierPoint[1] = new Vector2(point.x,point.y);
		}
	}

	[System.Serializable]
	public class PathInfo
	{
		public string name;
		public LineInfo startPoint;
		public List<LineInfo> lines = new List<LineInfo>();
		public float speed;
		public int lineCode = -1;
		public bool constantSpeed = false;

		public PathInfo(){lineCode = -1;}
		public PathInfo(string s,LineInfo[] ls, float sp)
		{
			name = s;
			lines = new List<LineInfo>(ls);
			speed = sp;
			lineCode = -1;
		}

		public void AddLine(LineInfo ls){lines.Add(ls);}
		public Vector3[] InfoToVectorArray()
		{
			List<Vector3> vecList = new List<Vector3>();

			int count = lines.Count;

			vecList.Add(startPoint.point);

			for(int i = 0; i < count; ++i)
			{
				if(lines[i].type == MathEx.EaseType.BezierCurve)
				{
					for(int j = 0; j <= 20; ++j)
					{
						Vector2 pt = new Vector2();
						if(i == 0)
						{
							pt = MathEx.GetPointOnBezierCurve(startPoint.point,startPoint.bezierPoint[1],
								lines[i].bezierPoint[0],lines[i].point, ((float)j) / 20);
						}
						else
						{
							pt = MathEx.GetPointOnBezierCurve(lines[i - 1].point,lines[i - 1].bezierPoint[1],
								lines[i].bezierPoint[0],lines[i].point, ((float)j) / 20);
						}
						
						vecList.Add(pt);
					}
				}
				else
				{
					vecList.Add(lines[i].point);
				}
			}

			return vecList.ToArray();
		}
	}

	public List<PathInfo> data = new List<PathInfo>();

	#if UNITY_EDITOR
	[MenuItem("ScriptableObjs/PathDatabase")]
	public static PathDatabase Create()
	{
		PathDatabase asset = ScriptableObject.CreateInstance<PathDatabase>();
		AssetDatabase.CreateAsset(asset,"Assets/PathDatabase.asset");
		AssetDatabase.SaveAssets();

		return asset;
	}
#endif
}
