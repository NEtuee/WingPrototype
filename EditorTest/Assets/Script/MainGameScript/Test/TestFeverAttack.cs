using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestFeverAttack : FeverBase {

	public Transform swordEffect;
	public GameObject startPointer;
	public int count = 0;

	public float checkDist = 5f;

	private float dist = 0f;
	private bool check = false;

	private Vector2 startPos;
	private Vector2 currPos;

    public override void Initialize()
	{
		Debug.Log("check");
		swordEffect.gameObject.SetActive(true);

		check = false;
		dist = 0f;
		count = 0;
	}
	public override void Progress()
	{
        PlayerManager.instance.target.DecreaseFever(12 * Time.deltaTime);


        if (Input.GetMouseButtonDown(0))
		{
			startPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

			startPointer.SetActive(true);
			StartPointInit();

			check = true;
		}
		else if(Input.GetMouseButtonUp(0))
		{
			startPointer.SetActive(false);
			check = false;
		}

		if(check)
		{
			currPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			dist = Vector2.Distance(startPos,currPos);

			swordEffect.position = currPos;

			if(dist >= checkDist)
			{
				++count;
				startPos = currPos;
				StartPointInit();
			}
		}
	}
	public override void EndEvent()
	{
		feverEndDirect.Initialize();
		
		startPointer.SetActive(false);
		swordEffect.gameObject.SetActive(false);

		Debug.Log(count);
	}

	public void StartPointInit()
	{
		startPointer.transform.position = startPos;
	}
}
