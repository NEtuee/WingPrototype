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

	public bool guided;
	public bool isMove = false;

	private Vector2 dir;

	private static bool one;
	private int count = 0;

	public float originAngle = 0f;
	public float clickAngle = 0f;
	public float currAngle = 0f;

	private Vector2 mousePos;
	private Vector2 clickPos;


	public void Init(int c,int f,float a,bool g)
	{
		code = c;
		frame = f;
		angle = a;
		guided = g;

		tp.rotation = Quaternion.Euler(0,0,angle);
		ColorCheck(false);
	}

	public void Update()
	{
		if(Input.GetMouseButtonDown(0))
		{
			mousePos = GetMousePosition();
			if(CollisionCheck(mousePos) != -1)
			{
				if(EnemyEdit.instance.currMarker == null)
				{
					isMove = true;

					AngleSetUp();

					clickPos = mousePos;
					EnemyEdit.instance.currMarker = this;
					ColorCheck(true);
				}
				else if(EnemyEdit.instance.currMarker == this)
				{
					AngleSetUp();

					clickPos = mousePos;
				}
				else
				{
					EnemyEdit.instance.currMarker.ColorCheck(false);
					EnemyEdit.instance.currMarker.isMove = false;

					AngleSetUp();

					isMove = true;
					clickPos = mousePos;
					EnemyEdit.instance.currMarker = this;
					ColorCheck(true);
				}
			}
		}
		
		if(Input.GetMouseButton(0) && EnemyEdit.instance.currMarker == this)
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

				angle = originAngle - (clickAngle - currAngle);

				if(Input.GetKey(KeyCode.LeftShift))
				{
					int factor = 10;
					if(Input.GetKey(KeyCode.LeftControl))
						factor = 30;
					
					angle = (float)((int)angle / factor) * (float)factor;
				}

				transform.rotation = Quaternion.Euler(0f,0f,angle);
				GetBulletInfo().angle = angle;

				EnemyEdit.instance.SetPatternInfo();

				//info.extraAngle = angle > 180 ? angle - 360f : angle;
			}
		}

		if(Input.GetMouseButtonUp(0) && EnemyEdit.instance.currMarker == this)
		{
			if(isMove)
				isMove = false;
			else
			{
				if(CollisionCheck(mousePos) != -1)
				{
					EnemyEdit.instance.DisablePatternInfo();
					ColorCheck(false);
					EnemyEdit.instance.currMarker = null;
				}
			}
		}
	}

	public void ColorCheck(bool select)
	{
		if(select)
		{
			Color c = Color.green;
			c.a = 0.2f;
			spr.color = c;
		}
		else
		{
			Color c = Color.white;
			c.a = 0.2f;
			spr.color = c;
		}
	}


	public void GetCurrAngle()
	{
		Vector2 pos = transform.position;
		Vector2 dir = GetMousePosition() - pos;
		dir = dir.normalized;

		currAngle = Mathf.Atan2(dir.y,dir.x) * Mathf.Rad2Deg;

		currAngle = currAngle < 0f ? currAngle + 360f : currAngle;
	}


	public void AngleSetUp()
	{
		originAngle = transform.localEulerAngles.z;

		GetCurrAngle();
		clickAngle = currAngle;
	}


	public Vector2 GetMousePosition()
	{
		return Camera.main.ScreenToWorldPoint(Input.mousePosition);
	}

	public int CollisionCheck(Vector2 point)
	{
		if(coll.OverlapPoint(point))
			return 0;

		return -1;
	}

	// public void GetMouseDirection()
	// {
	// 	dir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - tp.position;
	// 	dir = dir.normalized;
	// 	angle = Mathf.Atan2(dir.y,dir.x) * Mathf.Rad2Deg;
	// }

	// public void SetRotation()
	// {
	// 	tp.rotation = Quaternion.Euler(0,0,angle);
	// }

	public void DataSync()
	{
		angle = GetBulletInfo().angle;
		
		transform.rotation = Quaternion.Euler(0f,0f,angle);
		//SetRotation();
		EnemyEdit.instance.SetPatternInfo();
	}

	public EnemyDatabase.BulletInfo GetBulletInfo()
	{
		return EnemyEdit.instance.currInfo.bullet[frame].bulletInfo[code];
	}

	// public void ColorUpdate()
	// {
	// 	if(select)
	// 		spr.color = Color.green;
	// 	else
	// 		spr.color = Color.gray;
	// }

	public void Delete()
	{
		one = false;
		//EnemyEdit.instance.currMarker = null;
		Destroy(this.gameObject);
	}
}
