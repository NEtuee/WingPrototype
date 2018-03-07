using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect_Test : ItemEffectBase {

	public override void Effect()
	{
		MobileDebugger.instance.AddLine("test effect");
	}

	public override void ItemInit()
	{
		
	}
}
