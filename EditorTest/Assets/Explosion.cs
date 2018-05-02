using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour {

	public int count = 8;
	public int anime = 15;

	public float time = 2f;
	public float speed = 8f;
	public float angleAccel = 45f;

	public bool minus = false;

	private float betweenAngle = 0f;

	public float angle = 0f;

	public Vector3 direction = Vector3.zero;
	public float moveDist = 1f;

	private Vector3 origin;
	private Vector3 pos;
	private float moveTime = 0f;
	public void Start()
	{
		betweenAngle = 360f / (float)count;
		if(minus)
			angle = 180f - angle;

		origin = transform.position;
		pos = direction * moveDist + origin;
		moveTime = time;
	}
	void Update () 
	{
		if(PlayerManager.instance.IsFever() || PlayerManager.instance.target.feverBase.feverEndDirect.IsProgressing())
			return;
			
		time -= Time.deltaTime;

		transform.position = MathEx.EaseOutQuadVector2(origin,pos,1f - time / moveTime);

		if(time <= 0f)
		{
			for(int i = 0; i < count; ++i)
			{
				BulletManager.instance.ObjectActive(null,transform.position,speed,1f,betweenAngle * (float)i - angle,false,false)
								.SetAnimation(anime,false).SetAngleAccel(angleAccel);
			}
			Destroy(this.gameObject);
		}
	}
}
