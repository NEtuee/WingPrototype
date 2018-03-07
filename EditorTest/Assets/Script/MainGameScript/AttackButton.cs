using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackButton : ButtonPressing {

	public override void ButtonDown()
	{
		PlayerManager.instance.SetAttack(true);
	}

	public override void ButtonUp()
	{
		PlayerManager.instance.SetAttack(false);
	}
}
