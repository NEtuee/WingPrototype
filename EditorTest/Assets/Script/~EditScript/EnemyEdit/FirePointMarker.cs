using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirePointMarker : MonoBehaviour {

	public Transform tp;
	public SpriteRenderer spr;
	public Collider2D[] colliders;
	public bool select = false;
	private static bool one = false;
	public bool move = false;

	public EnemyDatabase.EnemyInfo info;
	public int code;

	private Vector2 origin;
	private Vector2 objOrigin;

	public void Init(int c,EnemyDatabase.EnemyInfo i)
	{
		code = c;
		info = i;
		tp = transform;
		spr = GetComponent<SpriteRenderer>();
		ColorUpdate();
	}

	public void PositionUpdate()
	{
		tp.position = info.shotPoint[code];
	}

	public void Update()
	{
		bool mouseButtonDown = Input.GetMouseButtonDown(0);
		Vector3 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		if(mouseButtonDown && !select && !one)
		{
			for(int i = 0; i < 1; ++i)
			{
				if(colliders[i].OverlapPoint(mouse))
				{
					select = true;
					one = true;
					EnemyEdit.instance.currPoint = this;
					ColorUpdate();
					EnemyEdit.instance.ShotPointUiUpdate(code,tp.position);
				}
			}
			//부위별로 만들어야 됨
		}
		if(mouseButtonDown && select)
		{
			for(int i = 0; i < 1; ++i)
			{
				if(colliders[i].OverlapPoint(mouse))
				{
					move = true;
					origin = mouse;
					objOrigin = tp.position;
				}
			}

			if(!move && EnemyEdit.instance.UiRaycast().Count == 0)
			{
				select = false;
				if(!one)
				{
					EnemyEdit.instance.currPoint = null;
				}
				ColorUpdate();
			}
		}
		else if(Input.GetMouseButton(0) && select && move)
		{
			tp.position = new Vector2(objOrigin.x + (mouse.x - origin.x),objOrigin.y + (mouse.y - origin.y));
			Progress();
		}
		else if(Input.GetMouseButtonUp(0))
		{
			if(!move)
			{
				one = false;
			}
			move = false;
			ColorUpdate();
		}
	}

	public void ColorUpdate()
	{
		if(select)
			spr.color = Color.white;
		else
			spr.color = Color.gray;
	}

	public void Progress()
	{
		info.shotPoint[code] = tp.position;
		EnemyEdit.instance.ShotPointUiUpdate(code,tp.position);
	}

	public void Delete()
	{
		one = false;
		Destroy(this.gameObject);
	}
}
