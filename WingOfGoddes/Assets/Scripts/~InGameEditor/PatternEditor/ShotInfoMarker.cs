﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotInfoMarker : MarkerBase {

	public static ShotInfoMarker select;

	public ShotInfo info;

	public float originAngle = 0f;
	public float clickAngle = 0f;
	public float currAngle = 0f;

	public void Set(ShotInfo i)
	{
		info = i;

		AngleSync();
	}

	public void AngleSync()
	{
		transform.rotation = Quaternion.Euler(0f,0f,info.angle);
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

					AngleSetUp();

					clickPos = mousePos;
					select = this;
					ColorCheck(true);

					s[0]();
				}
				else if(select == this)
				{
					AngleSetUp();

					clickPos = mousePos;
				}
				else
				{
					select.ColorCheck(false);
					select.isMove = false;

					AngleSetUp();

					isMove = true;
					clickPos = mousePos;
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
				GetCurrAngle();

				float angle = originAngle - (clickAngle - currAngle);

				if(Input.GetKey(KeyCode.LeftShift))
				{
					int factor = 10;
					if(Input.GetKey(KeyCode.LeftControl))
						factor = 30;
					
					angle = (float)((int)angle / factor) * (float)factor;
				}

				transform.rotation = Quaternion.Euler(0f,0f,angle);
				info.angle = angle;
				s[2]();
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
					s[1]();
				}
			}
		}
	}

	public override void ColorCheck(bool select)
	{
		if(select)
		{
			Color c = Color.green;
			c.a = 0.2f;
			spriteRenderer.color = c;
		}
		else
		{
			Color c = Color.white;
			c.a = 0.2f;
			spriteRenderer.color = c;
		}
	}

	public void AngleSetUp()
	{
		originAngle = transform.localEulerAngles.z;

		GetCurrAngle();
		clickAngle = currAngle;
	}

	public void GetCurrAngle()
	{
		Vector2 pos = transform.position;
		Vector2 dir = GetMousePosition() - pos;
		dir = dir.normalized;

		currAngle = Mathf.Atan2(dir.y,dir.x) * Mathf.Rad2Deg;

		currAngle = currAngle < 0f ? currAngle + 360f : currAngle;
	}
}