using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define {

	[System.Serializable]
	public class RectF
	{
		public Vector2 min;
		public Vector2 max;


		public RectF() {}
		public RectF(float left,float up,float right,float down)
		{
			min = new Vector2(left,down);
			max = new Vector2(right,up);
		}
	}

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

}
