using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeverBackGague : ObjectBase {

	public float zeroPos = -46.4f;
	public float speed = 3f;

	public override void Initialize()
	{
		GetTransform();
	}

	public override void Progress()
	{
		float gague = zeroPos + (-zeroPos) * PlayerManager.instance.target.GetCurrFeverGague();
		tp.position = new Vector3(0f,Mathf.Lerp(tp.position.y, gague, speed * Time.deltaTime));
	}

	public override void Release()
	{

	}
}
