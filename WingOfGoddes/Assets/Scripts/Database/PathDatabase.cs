using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class PathDatabase : ScriptableObject {

	[System.Serializable]
	public class PatternEvent
	{
		public bool trigger = false;
		public float time = .5f;
	}

	[System.Serializable]
	public class LineInfo
	{
		public MathEx.EaseType type = MathEx.EaseType.Linear;
		public Vector2 point;
		public Vector2[] bezierPoint = new Vector2[2];

		public float stayTime = 0f;
		public float speed = 0f;

		public List<PatternEvent> PatternEvent = new List<PathDatabase.PatternEvent>();

		public LineInfo(){}
		public LineInfo(Vector2 p)
		{
			point = p;
			speed = 1f;

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
		public int lineCode = -1;

		public PathInfo(){lineCode = -1;}
		public PathInfo(string n){name = n; lineCode = -1;}
		public PathInfo(string s,LineInfo[] ls)
		{
			name = s;
			lines = new List<LineInfo>(ls);
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

	public bool PathNameCheck(string name)
	{
		for(int i = 0; i < data.Count; ++i)
		{
			if(data[i].name.CompareTo(name) == 0)
				return false;
		}

		return true;
	}

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
