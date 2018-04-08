using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotBow : ObjectBase {

	public Transform aimObject;
	public Transform aimArrow;
	public Vector3 center;
	public float radius;

	public LineRenderer circle;

	private Vector3 dir;
	private float angle;
	private float dist;

	public override void Initialize()
	{
		GetTransform();
		radius = 5f;
	}

	public override void Progress()
	{
		if(!PlayerManager.instance.target.GetFeverEnabled())
		{
			gameObject.SetActive(false);
			aimArrow.gameObject.SetActive(false);
		}
		// if(Input.GetMouseButtonDown(0))
		// {
		// 	Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		// 	pos.z = 0f;
		// 	tp.position = pos;
		// 	Active();
		// }
		// else if(Input.GetMouseButton(0))
		// {
		// 	Aiming();
		// }
		// else if(Input.GetMouseButtonUp(0))
		// {
		// 	Shot();
		// }
	}

	public override void Release()
	{

	}

	public void DrawCircle(int accur)
	{
		List<Vector3> vec = new List<Vector3>();
		for(int i = 0; i < 360; i += accur)
		{
			vec.Add(new Vector3(Mathf.Sin(((float)i) * Mathf.Deg2Rad) * radius,Mathf.Cos(((float)i) * Mathf.Deg2Rad) * radius));
		}

		vec.Add(vec[0]);

		circle.positionCount = vec.Count;
		circle.SetPositions(vec.ToArray());
	}

	public void Active()
	{
		gameObject.SetActive(true);
		aimArrow.gameObject.SetActive(true);
		aimArrow.localScale = new Vector3(0f,1f,1f);
		
		DrawCircle(10);
		aimObject.transform.position = tp.position;
		center = tp.position;

		dist = 0f;
	}

	public void Shot()
	{
		gameObject.SetActive(false);
		aimArrow.gameObject.SetActive(false);

		if(dist < 1f)
			return;


		GameObjectManager.instance.bulletManager.ObjectActive
		(PlayerManager.instance.target,PlayerManager.instance.target.tp.position,dist * 20,dist,angle,false,false,BulletBase.BulletTeam.Player).
			SetPenetrate(true).SetAnimation(GameObjectManager.instance.effectManager.spriteContainer.aniSet[4]).feverAttack = true;

		CameraControll.instance.Shake(dist / 10,dist / 2);
	}

	public void Aiming(Vector3 pos)
	{
		pos.z = 0f;

		dist = Vector3.Distance(pos,center);
		dir = pos - center;
		angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + 180;

		aimArrow.localRotation = Quaternion.Euler(new Vector3(0f,0f,angle));

		if(dist >= radius)
		{
			pos = (pos - center).normalized * radius + center;
			dist = radius;
		}

		aimObject.position = pos;
		aimArrow.localScale = new Vector3(dist / radius * 2,2f,1f);
	}
}
