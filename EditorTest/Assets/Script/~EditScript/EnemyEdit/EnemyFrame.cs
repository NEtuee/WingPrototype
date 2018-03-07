using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyFrame : MonoBehaviour {

	public EnemyDatabase.BulletFrameInfo frame;

	private Image icon;
	private bool pick = false;

	public EnemyFrame Init(EnemyDatabase.BulletFrameInfo f)
	{
		icon = GetComponent<Image>();
		SetFrame(f);

		return this;
	}

	public void SetFrame(EnemyDatabase.BulletFrameInfo f)
	{
		frame = f;

		SetColor();
	}

	public void SetColor()
	{
		if(frame.bulletInfo.Count == 0)
		{
			icon.color = Color.gray;
		}
		else
		{
			icon.color = pick ? Color.green : Color.white;
		}

	}

	public void Progress()
	{
		icon.color = Color.blue;
		int count = frame.bulletInfo.Count;
		if(count == 0)
			return;

		for(int i = 0; i < count; ++i)
		{
			EnemyEdit.instance.CreateBullet(frame.bulletInfo[i]);
		}
	}

	public void Pick()
	{
		pick = true;
		icon.color = Color.green;
	}

	public void ReleasePick()
	{
		pick = false;
		SetColor();
	}
}
