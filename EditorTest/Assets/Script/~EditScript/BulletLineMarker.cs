using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletLineMarker : MonoBehaviour {

	public Transform tp;
	public Collider2D coll;
	public SpriteRenderer spr;
	public float angle;

	public int code;
	public int frame;

	private Vector2 dir;
	private bool select;
	private static bool one;
	private int count = 0;

	public void Init(int c,int f,float a)
	{
		code = c;
		frame = f;
		angle = a;

		tp.rotation = Quaternion.Euler(0,0,angle);
		ColorUpdate();
	}

	public void Update()
	{
		Vector2 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		if(Input.GetMouseButtonDown(0) && !one)
		{
			count = EnemyEdit.instance.UiRaycast().Count;
			if(coll.OverlapPoint(mouse) && count == 0)
			{
				EnemyEdit.instance.currMarker = this;
				select = true;
				one = true;
				ColorUpdate();
			}

		}
		else if(Input.GetMouseButtonDown(0) && select)
		{
			count = EnemyEdit.instance.UiRaycast().Count;
			if(!coll.OverlapPoint(mouse) && count == 0)
			{
				EnemyEdit.instance.currMarker = null;
				select = false;
				one = false;
				ColorUpdate();
			}

		}
		if(select && Input.GetMouseButton(0) && count == 0)
		{
			GetMouseDirection();
			tp.rotation = Quaternion.Euler(0,0,angle);
			Progress();
		}
	}

	public void GetMouseDirection()
	{
		dir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - tp.position;
		dir = dir.normalized;
		angle = Mathf.Atan2(dir.y,dir.x) * Mathf.Rad2Deg;
	}

	public void Progress()
	{
		EnemyEdit.instance.currInfo.bullet[frame].bulletInfo[code].angle = angle;
	}

	public void ColorUpdate()
	{
		if(select)
			spr.color = Color.green;
		else
			spr.color = Color.gray;
	}

	public void Delete()
	{
		one = false;
		//EnemyEdit.instance.currMarker = null;
		Destroy(this.gameObject);
	}
}
