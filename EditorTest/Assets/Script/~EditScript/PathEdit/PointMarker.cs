using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointMarker : MonoBehaviour {

	public enum MarkerType
	{
		LinePointer,
		BezierPointer,
	}

	public PathDatabase.LineInfo line;
	public SpriteRenderer spr;
	public CircleCollider2D circle;
	public LineRenderer lineRenderer;

	public bool move = false;
	public bool selected = false;
	public static bool overlapCheck = false;
	public int bezierCode;
	public MarkerType markerType;

	private Vector3 clickPos;
	private Vector3 originPos;

	public void SetColor()
	{
		if(markerType == MarkerType.BezierPointer)
		{
			if(bezierCode == 0)
			{
				if(selected)
				{
					spr.color = Color.blue;
				}
				else
				{
					Color c = Color.blue;
					c.a = 0.3f;
					spr.color = c;
				}
			}
			else
			{
				if(selected)
				{
					spr.color = Color.red;
				}
				else
				{
					Color c = Color.red;
					c.a = 0.3f;
					spr.color = c;
				}
			}
		}
		else if(markerType == MarkerType.LinePointer)
		{
			if(selected)
			{
				spr.color = Color.white;
			}
			else
			{
				Color c = Color.white;
				c.a = 0.3f;
				spr.color = c;
			}
		}
	}

	public void SetLine()
	{
		Vector3[] pos = {line.point,transform.position};
		lineRenderer.SetPositions(pos);
	}

	public PointMarker CheckType()
	{
		if(markerType == MarkerType.BezierPointer)
		{
			if(line.type != MathEx.EaseType.BezierCurve && bezierCode == 0)
			{
				return this;
			}
			else
				return null;
		}

		return null;
	}

	public void Set(PathDatabase.LineInfo l,MarkerType type, int code = -1)
	{
		line = l;
		markerType = type;

		if(type == MarkerType.LinePointer)
		{
			transform.position = line.point;
		}
		else if(type == MarkerType.BezierPointer)
		{
			transform.position = line.bezierPoint[code];
			bezierCode = code;
			SetLine();
		}

		SetColor();
	}

	void Update () 
	{
		if(Input.GetMouseButtonDown(0) && !overlapCheck)
		{
			if(circle.OverlapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition)))
			{
				clickPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
				clickPos.z = 0f;
				originPos = transform.position;
				move = true;
				selected = true;
				overlapCheck = true;
				if(markerType == MarkerType.LinePointer)
				{
					PathEdit.instance.UpdatePointUI(line);
				}
				else
				{
					PathEdit.instance.pointInfoUi.DisableAllUI();
				}
				SetColor();
			}
		}
		else if(Input.GetMouseButtonDown(0) && selected)
		{
			if(!circle.OverlapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition)))
			{
				overlapCheck = false;
				selected = false;
				SetColor();
			}
			else
			{
				clickPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
				clickPos.z = 0f;
				originPos = transform.position;
				move = true;
			}
		}

		if(Input.GetMouseButton(0) && move)
		{
			Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			pos.z = 0f;
			pos = clickPos - pos;
			pos = originPos - pos;
			transform.position = pos;

			if(markerType == MarkerType.LinePointer)
			{
				line.point = pos;
				PathEdit.instance.UpdatePointMarker();
				PathEdit.instance.pointInfoUi.ValueUpdate();
			}
			else if(markerType == MarkerType.BezierPointer)
			{
				line.bezierPoint[bezierCode] = pos;
				SetLine();
			}

			PathEdit.instance.SetPointMarkerToPath();
			PathEdit.instance.SetLineRenderer();
			PathEdit.instance.AllUiUpdate();
		}

		if(Input.GetMouseButtonUp(0) && move)
		{
			move = false;
		}
	}
}
