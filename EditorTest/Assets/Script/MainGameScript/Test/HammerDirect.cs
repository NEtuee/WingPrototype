using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerDirect : DirectBase {

	public GameObject directObjects;
	public Animator anime;


	private bool aniEnd = false;
	public override void Initialize()
	{
		base.Initialize();

		directObjects.SetActive(true);
		anime.Play(0);
		Debug.Log("check");

		aniEnd = false;
		//anime.
	}

	public override void Progress()
	{
		if (aniEnd)
 		{
			Debug.Log("check2");
			DirectEnd();
			directObjects.SetActive(false);
			PlayerManager.instance.target.feverBase.Initialize();
 		}

	}

	public void AnimationEnd()
	{
		aniEnd = true;
	}
}
