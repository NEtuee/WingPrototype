using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierMarker : LineInfoMarker {

	public int bezierPoint = 0;

	public void Set(PathDatabase.LineInfo i, int b)
	{
		info = i;
		bezierPoint = b;
		transform.position = info.bezierPoint[bezierPoint];

		gameObject.SetActive(info.type == MathEx.EaseType.BezierCurve);
		ColorSet();
	}

	public override void Progress(Sync[] s)
	{
		if(Input.GetMouseButtonDown(0))
		{
			mousePos = GetMousePosition();
			if(CollisionCheck(mousePos) != -1)
			{
				if(select == null)
				{
					isMove = true;
					clickPos = mousePos;
					originPos = transform.position;
					select = this;
					ColorCheck(true);
				}
				else if(select == this)
				{
					clickPos = mousePos;
					originPos = transform.position;
				}
				else
				{
					select.ColorCheck(false);
					select.isMove = false;

					isMove = true;
					clickPos = mousePos;
					originPos = transform.position;
					select = this;
					ColorCheck(true);
				}
			}
		}
		
		if(Input.GetMouseButton(0) && select == this)
		{
			mousePos = GetMousePosition();

			if(!isMove)
			{
				if(CollisionCheck(mousePos) != -1)
				{
					if(clickPos - mousePos != Vector2.zero)
					{
						isMove = true;
					}
				}
			}
			if(isMove)
			{
				transform.position = originPos + (mousePos - clickPos);
				info.bezierPoint[bezierPoint] = transform.position;
				s[0]();
			}
		}

		if(Input.GetMouseButtonUp(0) && select == this)
		{
			if(isMove)
				isMove = false;
			else
			{
				if(CollisionCheck(mousePos) != -1)
				{
					ColorCheck(false);
					select = null;
				}
			}
		}

	}

	public void ColorSet()
	{
		Color c;

		if(bezierPoint == 0)
		{
			c = Color.green;
		}
		else
		{
			c = Color.red;
		}

		c.a = 0.3f;

		spriteRenderer.color = c;
	}

	public override void ColorCheck(bool select)
	{
		if(select)
		{
			if(bezierPoint == 0)
			{
				spriteRenderer.color = Color.green;
			}
			else
			{
				spriteRenderer.color = Color.red;
			}
		}
		else
		{
			Color c = spriteRenderer.color;
			c.a = 0.3f;
			spriteRenderer.color = c;
		}
	}
}
