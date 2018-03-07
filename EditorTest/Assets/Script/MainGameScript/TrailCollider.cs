using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailCollider : BulletBase {

	public Define.LineInfo[] lines;
	public LineRenderer lineRenderer;

	public int accuracy = 3;

	public override void Initialize()
	{
		GetTransform();

		SetPenetrate(true);
		attack = 5f;

		lines = new Define.LineInfo[accuracy];

		feverAttack = true;

		for(int i = 0; i < accuracy; ++i)
		{
			lines[i] = new Define.LineInfo(tp.position,tp.position);
		}

//		lineRenderer.positionCount = accuracy;
	}

	public void Active()
	{
		for(int i = 0; i < accuracy; ++i)
		{
			lines[i].Set(tp.position);
		}
	}

	public void Draw()
	{
		List<Vector3> vecList = new List<Vector3>();

		vecList.Add(lines[0].start);
		for(int i = 0; i < accuracy; ++i)
		{
			vecList.Add(lines[i].end);
		}

		lineRenderer.SetPositions(vecList.ToArray());
	}

	public override void Progress()
	{
		for(int i = accuracy - 1; i >= 0; --i)
		{
			if(i == 0)
			{
				lines[i].Set(tp.position,lines[i].start);
			}
			else
			{
				lines[i].Set(lines[i-1].start,lines[i].start);
			}
		}

		if(lines[0].start == lines[accuracy - 1].end)
			return;
		
		CopyList();
		EnemyManager.instance.LineIntersectCheck(this);
		ItemManager.instance.CollisionCheck(this);
		DeleteExitObjects();

		//Draw();
	}

	public override bool Collision(ObjectBase obj)
	{
		for(int i = 0; i < accuracy; ++i)
		{
			if(obj.CircleLineIntersect(lines[i].start,lines[i].end))
			{
				//ColliseionActive(obj);
				return true;
			}
		}

		return false;
	}

// 	public override void ColliseionActive(ObjectBase obj)
// 	{
// 		if(!penetrate)
// 		{
// 			obj.DecreaseHp(attack);
// 			DisableBullet();
// 		}
// 		else
// 		{
// //			check = false;

// 			for(int i = 0; i < collisionObjects.Count; ++i)
// 			{
// 				if(collisionObjects[i] == obj)
// 				{
// 					//충돌중
// 					exitObjects.Remove(obj);
// 					return;
// 				}
// 			}

// 			//최초 충돌
// 			obj.DecreaseHp(attack);
// 			collisionObjects.Add(obj);
// 		}
// 	}
}
