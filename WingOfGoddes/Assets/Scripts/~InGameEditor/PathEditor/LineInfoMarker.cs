using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineInfoMarker : MarkerBase {

	public static LineInfoMarker select = null;

	public GameObject bezierMakrer;
	public PathDatabase.LineInfo info;
	public BezierMarker[] markers;

	public void Set(PathDatabase.LineInfo i)
	{
		info = i;
		transform.position = info.point;

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

					s[0]();
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

					s[0]();
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

				info.point = transform.position;
				s[1]();
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

					s[0]();
				}
			}
		}

	}
}
