using System.Collections.Generic;

public class PatternFrameInfo {
	
	public int frame;
	public float extraSpeed = 1f;
	public float extraAngle = 0f;

	public List<int> presets = new List<int>();

	public PatternFrameInfo (int f) {frame = f;}
}
