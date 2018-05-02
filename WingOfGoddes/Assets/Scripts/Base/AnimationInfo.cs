using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationInfo {

	public class AnimationFrameInfo
	{
		public int start;
		public int end;

		public float speed;
	}

	public int group;
	public int set;
	public int framePerSec;

	public bool loop;

	public AnimationFrameInfo[] layers;
	public Keyframe[] curve;

}
