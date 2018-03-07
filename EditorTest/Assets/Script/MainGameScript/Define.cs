using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define {

	public class RectF
	{
		float left;
		float up;
		float right;
		float down;

		public RectF() {}
		public RectF(float l,float u,float r,float d)
		{
			left = l;
			up = u;
			right = r;
			down = d;
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
