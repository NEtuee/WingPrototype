using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class DirectBase : MonoBehaviour {

	public Canvas directCanvas;
	public RectTransform[] transforms;
	public float directTime = 1f;

	protected bool progressing = false;

	protected List<Vector3> resetPos = new List<Vector3>();
	protected float mainTime = 0;

	public void Start()
	{
		//SetResetPos();
		SetCanvasCamTarget();
	}

	public void SetResetPos()
	{
		int count = transforms.Length;
		for(int i = 0; i < count; ++i)
		{
			resetPos.Add(transforms[i].localPosition);
		}
	}

	public void ResetPosition()
	{
		int count = transforms.Length;
		for(int i = 0; i < count; ++i)
		{
			transforms[i].localPosition = resetPos[i];
		}
	}

	public void SetCanvasCamTarget()
	{
		directCanvas.worldCamera = Camera.main;
	}

	public virtual void Initialize()
	{
		//=================================================check
//		GameObjectManager.instance.GamePause();
		BulletPlayer.instance.stop = true;

		mainTime = 0f;
		progressing = true;

		directCanvas.gameObject.SetActive(true);
		//ResetPosition();
	}
	public abstract void Progress();

	public virtual void DirectEnd(bool cont = true)
	{
		//=================================================check
		//GameObjectManager.instance.GamePause(cont);
		BulletPlayer.instance.stop = cont;

		directCanvas.gameObject.SetActive(false);
		progressing = false;
	}

	public bool IsProgressing() {return progressing;}
}
