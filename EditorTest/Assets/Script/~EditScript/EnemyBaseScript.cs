using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBaseScript : MonoBehaviour {

	public SpriteRenderer spriteRenderer;
	public Vector3[] point;
	public Vector2[] shotPoints;

	public float speed = 1f;
	public float fireRate = 0.1f;
	public float patternRate = 1f;
	public bool constant = false;

	private int dummy = 0;

	public EnemyDatabase.BulletFrameInfo[] bullet;
	private PathDatabase.PathInfo path;

	public Transform tp;

	private float time = 0f;
	private int lineCode = 0;

	private float fireTime = 0f;
	private float patternTime = 0f;

	private float stayTime = 0f;
	private int currFrame = 0;

	private Vector3 spawnPos;
	private PathDatabase.LineInfo posOrigin;
	private PathDatabase.LineInfo targetPos;

	public void Init(float ptr,float sp,bool con, Vector3 spawn, Sprite sprite,EnemyDatabase.BulletFrameInfo[] bt,PathDatabase.PathInfo p,Vector2[] shotpt)
	{
		spriteRenderer.sprite = sprite;
		patternRate = ptr;
		//point = pt;
		path = p;

		spawnPos = spawn;

		speed = sp;
		constant = con;

		stayTime = path.startPoint.stayTime;

		fireRate = 1f / 12f;

		bullet = bt;
		shotPoints = shotpt;

		posOrigin = path.startPoint;
		targetPos = path.lines[lineCode++];
	}

	void Update () 
	{
		if(stayTime != 0f)
		{
			stayTime -= Time.deltaTime;
			if(stayTime <= 0f)
				stayTime = 0f;
		}
		else
		{
			Movement();
		}
		Shot();
	}

	public void Shot()
	{
		if(patternTime >= patternRate)
		{
			fireTime += Time.deltaTime;
			if(fireTime >= fireRate)
			{
				fireTime = 0f;
				int count = bullet[currFrame].bulletInfo.Count;
				for(int i = 0; i < count; ++i)
				{
					CreateBullet(bullet[currFrame].bulletInfo[i]);
				}
				++currFrame;

				if(currFrame == 12)
				{
					fireTime = fireRate;
					patternTime = 0f;
					currFrame = 0;
				}
			}
		}
		else
			patternTime += Time.deltaTime;
	}

	public void CreateBullet(EnemyDatabase.BulletInfo bulletInfo)
	{
		Vector2 pos = tp.position;
		BulletScript bs = Instantiate(Test.instance.bulletBase,shotPoints[bulletInfo.shotPoint] + pos,Quaternion.identity).GetComponent<BulletScript>();
		bs.Init(bulletInfo.angle,bulletInfo.speed);
	}

	public void Movement()
	{
		if(constant)
			time += speed * Time.deltaTime / Vector3.Distance(posOrigin.point,targetPos.point);
		else
			time += targetPos.speed * Time.deltaTime;
		tp.position = MathEx.GetEaseFormula(posOrigin,targetPos,time,targetPos.type,spawnPos);
		//tp.position = Vector3.Lerp(posOrigin,targetPos,time);
		if(time >= 1f)
		{
			if(lineCode >= path.lines.Count)
			{
				tp.gameObject.SetActive(false);
				return;
			}
			//tp.position = Vector3.Lerp(posOrigin,targetPos,1);
			tp.position = MathEx.GetEaseFormula(posOrigin,targetPos,1,path.lines[lineCode].type,spawnPos);
			time = 0f;
			posOrigin = targetPos;
			targetPos = path.lines[lineCode++];

			stayTime = posOrigin.stayTime;
		}
	}
}
