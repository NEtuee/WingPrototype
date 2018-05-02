using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FakeHpScript : ObjectBase {

	public Image fakeHpBar;
	public SimpleRectCollider rectCollider;
	public float maximumHp = 100f;

	public Image hpbar;
	public float height=0f;
	
	
	Vector3 hporigin;

	public override void Initialize()
	{
		hporigin = hpbar.transform.localPosition;
		SetMaxHp(maximumHp);
		GetTransform();
	}

	public override void Progress()
	{
		Vector3 pos = hporigin;
		pos.y += height * hp / maxHp;
		hpbar.transform.localPosition = pos;
		BulletManager.instance.CollisionCheck(this,BulletBase.BulletTeam.Player);
	}

	public override void Release()
	{

	}

	public override bool Collision(ObjectBase obj)
	{
		if(MathEx.IntersectRectCircle(obj.tp.position,tp.position,obj.GetColliderInfo().radius,rectCollider.rect))
		{
			PlayerManager.instance.target.IncreaseFever(10f);
			return true;
		}
		return false;
	}
}
