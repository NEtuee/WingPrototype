using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define {

	public enum SceneInfo
    {
        Title,
        MainMenu,
        CharacterSelect,
		Shop,
        StageSelect,
        StageInfo,
        MainStage,
        Result,
    };

	public enum ColliderType
	{
		Circle,
		Box
	}

	public enum BulletTeam
	{
		Player,
		Enemy,
	}


	[System.Serializable]
	public class RectF
	{
		public Vector2 min;
		public Vector2 max;


		public bool CompareTo(RectF rect)
		{
			return min == rect.min ? (max == rect.max ? true : false) : false;
		}
		public RectF() {min = Vector2.zero; max = Vector2.zero;}
		public RectF(float left,float up,float right,float down)
		{
			min = new Vector2(left,down);
			max = new Vector2(right,up);
		}
		public RectF(Vector2 mi, Vector2 ma)
		{
			min = mi;
			max = ma;
		}

		public override string ToString()
		{
			return min + ", " + max;
		}

		public static RectF operator *(RectF a, float b)
        {
			return new RectF(a.min * b, a.max * b);
        }
	}

	[System.Serializable]
	public class LineInfo
	{
		public Vector2 start;
		public Vector2 end;

		public void Set(Vector2 value)
		{
			start = value;
			end = value;
		}

		public void Set(Vector2 value_0,Vector2 value_1)
		{
			start = value_0;
			end = value_1;
		}

		public LineInfo(){}
		public LineInfo(Vector2 s, Vector2 e)
		{
			Set(s,e);
		}
	}

	[System.Serializable]
	public class SpriteInfo
	{
		public int frame = -1;
		
		public int group;
		public int set;
		public int index;

		public float speed;
	}
}
