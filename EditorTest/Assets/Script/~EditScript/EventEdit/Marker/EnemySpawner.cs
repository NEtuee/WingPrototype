using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MarkerBase {

	public LineRenderer line;

	public EnemyCreateEvent target;
	public PathDatabase.PathInfo path;

	public override void Init(int f)
	{
		base.Init(f);

		if(path.lines.Count == 0)
		{
			line.positionCount = 0;
			return;
		}
		Vector3[] array = path.InfoToVectorArray();
		line.positionCount = array.Length;
		line.SetPositions(path.InfoToVectorArray());
	}

	public override void Progress()
	{
		target.pos = tp.position;
	}
}
