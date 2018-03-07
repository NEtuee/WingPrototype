using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePath : MonoBehaviour {

	private PathDatabase.PathInfo path;

	private float speed;
	public Transform tp;

	private PathDatabase.LineInfo posOrigin;
	private PathDatabase.LineInfo targetPos;

	private float time = 0f;
	private float stayTime = 0f;
	private int lineCode = 0;
	private bool constant = false;

	public void Update()
	{
		if(stayTime != 0f)
		{
			stayTime -= Time.deltaTime;
			if(stayTime <= 0f)
				stayTime = 0f;
		}
		else
		{
			if(constant)
			time += speed * Time.deltaTime / Vector3.Distance(posOrigin.point,targetPos.point);
			else
				time += targetPos.speed * Time.deltaTime;
			tp.position = MathEx.GetEaseFormula(posOrigin,targetPos,time,targetPos.type);

			//tp.position = Vector3.Lerp(posOrigin,targetPos,time);
			if(time >= 1f)
			{
				if(lineCode >= path.lines.Count)
				{
					tp.gameObject.SetActive(false);
					return;
				}
				//tp.position = Vector3.Lerp(posOrigin,targetPos,1);

				tp.position = MathEx.GetEaseFormula(posOrigin,targetPos,1,path.lines[lineCode].type);
				time = 0f;
				posOrigin = targetPos;
				targetPos = path.lines[lineCode++];

				stayTime = posOrigin.stayTime;
			}
		}

	}

	public void Stop()
	{
		gameObject.SetActive(false);
	}

	public void SetUp(PathDatabase.PathInfo p)
	{
		if(p.lines.Count == 0)
			return;
		path = p;
		lineCode = 0;
		time = 0f;
		speed = path.speed;
		constant = path.constantSpeed;

		tp.position = path.startPoint.point;
		posOrigin = path.startPoint;
		targetPos = path.lines[lineCode++];

		tp.gameObject.SetActive(true);
	}
}
