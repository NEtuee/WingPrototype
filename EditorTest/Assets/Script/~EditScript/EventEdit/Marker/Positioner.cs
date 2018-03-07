using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Positioner : MarkerBase {

	public ObjectCreateEvent target;

	public override void Progress()
	{
		target.pos = tp.position;
	}
}
