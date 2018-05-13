using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotPointMarker : MarkerBase {

	public static ShotPointMarker select;

	public EnemyDatabase.ShotPointInfo info;

	public void PositionSync()
	{
		transform.position = info.pos;
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

				info.pos = transform.position;
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
}
