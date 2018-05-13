using UnityEngine;

public class MathEx {

	public enum EaseType{
		Linear,
		BezierCurve,
		EaseInQuad,
		EaseOutQuad,
		EaseInOutQuad,
		EaseInCubic,
		EaseOutCubic,
		EaseInOutCubic,
		End,
	}
#region EaseFormula
	public static Vector2 GetEaseFormula(PathDatabase.LineInfo start, PathDatabase.LineInfo end, float time, EaseType type, Vector2 spawn = new Vector2())
	{
		if(type == EaseType.Linear)
		{
			return LinearVector2(start.point + spawn,end.point + spawn,time);
		}
		else if(type == EaseType.BezierCurve)
		{
			return GetPointOnBezierCurve(start.point + spawn,start.bezierPoint[1] + spawn,end.bezierPoint[0] + spawn,end.point + spawn,time);
		}
		else if(type == EaseType.EaseInQuad)
		{
			return EaseInQuadVector2(start.point + spawn,end.point + spawn,time);
		}
		else if(type == EaseType.EaseOutQuad)
		{
			return EaseOutQuadVector2(start.point + spawn,end.point + spawn,time);
		}
		else if(type == EaseType.EaseInOutQuad)
		{
			return EaseInOutQuadVector2(start.point + spawn,end.point + spawn,time);
		}
		else if(type == EaseType.EaseInCubic)
		{
			return EaseInCubicVector2(start.point + spawn,end.point + spawn,time);
		}
		else if(type == EaseType.EaseOutCubic)
		{
			return EaseOutCubicVector2(start.point + spawn,end.point + spawn,time);
		}
		else if(type == EaseType.EaseInOutCubic)
		{
			return EaseInOutCubicVector2(start.point + spawn,end.point + spawn,time);
		}

		return new Vector2();
	}

	public static Vector2 LinearVector2(Vector2 start, Vector2 end ,float time)
	{
		return Vector2.Lerp(start,end,time);
	}

	public static Vector2 GetPointOnBezierCurve(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3, float t)
	{
    	float u = 1f - t;
    	float t2 = t * t;
    	float u2 = u * u;
    	float u3 = u2 * u;
    	float t3 = t2 * t;
 
    	Vector2 result =
    	    (u3) * p0 +
    	    (3f * u2 * t) * p1 +
    	    (3f * u * t2) * p2 +
    	    (t3) * p3;
 
    	return result;
	}

	public static Vector2 EaseInQuadVector2(Vector2 start, Vector2 end, float time)
	{
		return new Vector2(easeInQuad(start.x,end.x,time),easeInQuad(start.y,end.y,time));
	}

	public static Vector2 EaseOutQuadVector2(Vector2 start, Vector2 end ,float time)
	{
		return new Vector2(easeOutQuad(start.x,end.x,time),easeOutQuad(start.y,end.y,time));
	}

	public static Vector2 EaseInOutQuadVector2(Vector2 start, Vector2 end ,float time)
	{
		return new Vector2(easeInOutQuad(start.x,end.x,time),easeInOutQuad(start.y,end.y,time));
	}

	public static Vector2 EaseInCubicVector2(Vector2 start, Vector2 end ,float time)
	{
		return new Vector2(easeInCubic(start.x,end.x,time),easeInCubic(start.y,end.y,time));
	}

	public static Vector2 EaseOutCubicVector2(Vector2 start, Vector2 end ,float time)
	{
		return new Vector2(easeOutCubic(start.x,end.x,time),easeOutCubic(start.y,end.y,time));
	}
	
	public static Vector2 EaseInOutCubicVector2(Vector2 start, Vector2 end ,float time)
	{
		return new Vector2(easeInOutCubic(start.x,end.x,time),easeInOutCubic(start.y,end.y,time));
	}

	public static float linear(float start, float end, float value)
	{
		return Mathf.Lerp(start, end, value);
	}

	public static float easeInQuad(float start, float end, float value)
	{
		end -= start;
		return end * value * value + start;
	}

	public static float easeOutQuad(float start, float end, float value)
	{
		end -= start;
		return -end * value * (value - 2) + start;
	}

	public static float easeInOutQuad(float start, float end, float value)
	{
		value /= .5f;
		end -= start;
		if (value < 1) return end * 0.5f * value * value + start;
		value--;
		return -end * 0.5f * (value * (value - 2) - 1) + start;
	}

	public static float easeInCubic(float start, float end, float value)
	{
		end -= start;
		return end * value * value * value + start;
	}

	public static float easeOutCubic(float start, float end, float value)
	{
		value--;
		end -= start;
		return end * (value * value * value + 1) + start;
	}

	public static float easeInOutCubic(float start, float end, float value)
	{
		value /= .5f;
		end -= start;
		if (value < 1) return end * 0.5f * value * value * value + start;
		value -= 2;
		return end * 0.5f * (value * value * value + 2) + start;
	}

//움직임 목록에 포함 안되있는 식들===============================================
	public static float easeInBack(float start, float end, float value)
	{
		end -= start;
		value /= 1;
		float s = 1.70158f;
		return end * (value) * value * ((s + 1) * value - s) + start;
	}
//============================================================================



	public static bool GetRandom(int value) // 1 ~ 100
	{
		int random = Random.Range(1,100);
		return (random -= value) <= 0;
	}

	public static int GetRandom(int[] weights)
	{
		int count = weights.Length;
		int total = 0;
		for(int i = 0; i < count; ++i)
			total += weights[i];
		int random = Random.Range(1,total);
		
		for(int i = 0; i < count; ++i)
		{
			random -= weights[i];
			if(random <= 0)
				return i;
		}
		
		return -1;
	}

	public static int BetweenLineAndCircle(Vector2 circleCenter, float circleRadius, Vector2 point1, Vector2 point2 
    /*out Vector2 intersection1, out Vector2 intersection2*/)
 	{
//    	float t;
 
    	float dx = point2.x - point1.x;
    	float dy = point2.y - point1.y;
 
    	float a = dx * dx + dy * dy;
    	float b = 2 * (dx * (point1.x - circleCenter.x) + dy * (point1.y - circleCenter.y));
    	float c = (point1.x - circleCenter.x) * (point1.x - circleCenter.x) + (point1.y - circleCenter.y) * (point1.y - circleCenter.y) - circleRadius * circleRadius;
 
    	float determinate = b * b - 4 * a * c;

		if(determinate < 0f)
		{
    	    return 0;
		}

		float dist;
		
		if(!PointInCircle(point1,circleCenter,circleRadius) && !PointInCircle(point2,circleCenter,circleRadius))
		{
			dist = Vector2.Distance(point1,point2);
			if(Vector2.Distance(circleCenter,point1) > dist || Vector2.Distance(circleCenter,point2) > dist)
				return 0;
		}

		return 1;

    	// if ((a <= 0.0000001) || (determinate < -0.0000001))
    	// {
    	//     // No real solutions.
    	//     intersection1 = Vector2.zero;
    	//     intersection2 = Vector2.zero;
    	//     return 0;
    	// }
    	// if (determinate < 0.0000001 && determinate > -0.0000001)
    	// {
    	//     // One solution.
    	//     t = -b / (2 * a);
    	//     intersection1 = new Vector2(point1.x + t * dx, point1.y + t * dy);
    	//     intersection2 = Vector2.zero;
    	//     return 1;
    	// }
     
    	// // Two solutions.
    	// t = (float)((-b + Mathf.Sqrt(determinate)) / (2 * a));
    	// intersection1 = new Vector2(point1.x + t * dx, point1.y + t * dy);
    	// t = (float)((-b - Mathf.Sqrt(determinate)) / (2 * a));
	    // intersection2 = new Vector2(point1.x + t * dx, point1.y + t * dy);
 
    	// return 2;
 	}
#endregion
#region CollisionFormula
	public static bool PointInCircle(Vector2 point, Vector2 circlePos, float radius)
	{
		return (point.x - circlePos.x) * (point.x - circlePos.x) +
				(point.y - circlePos.y) * (point.y - circlePos.y) < radius * radius;
	}

	public static bool IntersectRectCircle(Define.RectF circleBound, Define.RectF rectBound, Define.RectF rect)
	{
		int zone = GetRectZone(circleBound.min,rectBound);

		if(zone == 1)
		{
			return rectBound.min.y - circleBound.max.x <= circleBound.min.y;
		}
		else if(zone == 3)
		{
			return rectBound.min.x - circleBound.max.x <= circleBound.min.x;
		}
		else if(zone == 4)
		{
			return true;
		}
		else if(zone == 5)
		{
			return rectBound.max.x + circleBound.max.x >= circleBound.min.x;
		}
		else if(zone == 7)
		{
			return rectBound.max.y + circleBound.max.x >= circleBound.min.y;
		}
		else
		{
			Vector2 vec = new Vector2(zone == 0 || zone == 6 ? rectBound.min.x : rectBound.max.x,
					zone == 0 || zone == 2 ? rectBound.min.y : rectBound.max.y);

			return PointInCircle(vec,circleBound.min,circleBound.max.x);
		}
	}

	public static bool IntersectRectCircle(Vector2 circlePos, Vector2 rectPos, float radius, Define.RectF rect)
	{
		int zone = GetRectZone(circlePos,rectPos,rect);

		if(zone == 1)
		{
			return rect.min.y - radius + rectPos.y <= circlePos.y;
		}
		else if(zone == 3)
		{
			return rect.min.x - radius + rectPos.x <= circlePos.x;
		}
		else if(zone == 4)
		{
			return true;
		}
		else if(zone == 5)
		{
			return rect.max.x + radius + rectPos.x >= circlePos.x;
		}
		else if(zone == 7)
		{
			return rect.max.y + radius + rectPos.y >= circlePos.y;
		}
		else
		{
			Vector2 vec = new Vector2(zone == 0 || zone == 6 ? rect.min.x : rect.max.x,
					zone == 0 || zone == 2 ? rect.min.y : rect.max.y);

			return PointInCircle(vec,circlePos,radius);
		}
	}

	public static bool IntersectRect(Define.RectF rect1, Define.RectF rect2)
	{
		if (rect1.min.x < rect2.max.x &&
			rect1.max.y < rect2.min.y &&
			rect1.max.x > rect2.min.x &&
			rect1.min.y > rect2.max.y)
		{
			return true;
		}

		return false;
	}

	public static bool IntersectCircle(Define.RectF rect1, Define.RectF rect2) //min = position max.x = radius
	{
		float dist = rect1.max.x + rect2.max.x;

		if(Vector2.Distance(rect1.min, rect2.min) <= dist)
		{
			return true;
		}

		return false;
	}

	public static bool PointInRect(Vector2 point, Vector2 rectPos, Define.RectF rect)
	{
		return point.x >= rectPos.x + rect.min.x && point.x <= rectPos.x + rect.max.x &&
				point.y >= rectPos.y + rect.min.y && point.y <= rectPos.y + rect.max.y;
	}

	public static bool PointInRect(Vector2 point, Define.RectF rect)
	{
		return point.x >= rect.min.x && point.x <= rect.max.x &&
				point.y >= rect.min.y && point.y <= rect.max.y;
	}

	public static int GetRectZone( Vector2 circlePos, Define.RectF rect)
	{
    	int xZone = ( circlePos.x <  rect.min.x ) ? 0 : ( circlePos.x >  rect.max.x ) ? 2 : 1;
    	int yZone = ( circlePos.y <  rect.min.y ) ? 0 : ( circlePos.y >  rect.max.y ) ? 2 : 1;
    	int nZone = xZone + 3*yZone;

    	return nZone;
	}

	public static int GetRectZone( Vector2 circlePos, Vector2 rectPos ,Define.RectF rect)
	{
    	int xZone = ( circlePos.x <  rect.min.x + rectPos.x ) ? 0 : ( circlePos.x >  rect.max.x + rectPos.x ) ? 2 : 1;
    	int yZone = ( circlePos.y <  rect.min.y + rectPos.y ) ? 0 : ( circlePos.y >  rect.max.y + rectPos.y ) ? 2 : 1;
    	int nZone = xZone + 3*yZone;

    	return nZone;
	}
#endregion

	public static Define.RectF RectTransformToRect(RectTransform rectTp,Define.RectF rect)
	{
		rect.min.x = (rectTp.anchoredPosition.x - rectTp.sizeDelta.x * .5f);
		rect.min.y = (rectTp.anchoredPosition.y - rectTp.sizeDelta.y * .5f);
		rect.max.x = (rectTp.anchoredPosition.x + rectTp.sizeDelta.x * .5f);
		rect.max.y = (rectTp.anchoredPosition.y + rectTp.sizeDelta.y * .5f);

		return rect;
	}

	public static Define.RectF RectTransformNormalToRect(RectTransform rectTp,Define.RectF rect)
	{
		rect.min.x = (rectTp.position.x - rectTp.sizeDelta.x * .5f);
		rect.min.y = (rectTp.position.y - rectTp.sizeDelta.y * .5f);
		rect.max.x = (rectTp.position.x + rectTp.sizeDelta.x * .5f);
		rect.max.y = (rectTp.position.y + rectTp.sizeDelta.y * .5f);

		return rect;
	}

	public static void Vector2Mul(ref Vector2 one,Vector2 two) {one.x *= two.x; one.y *= two.y;}

	public static void Increaser(ref float value,float max,float inc) {value = (value + inc) >= max ? max : (value + inc);}
	public static void Decreaser(ref float value,float dec,float min = 0f) {value = (value - dec) <= min ? min : (value - dec);}
}
