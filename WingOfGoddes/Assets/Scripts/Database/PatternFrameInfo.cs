using System.Collections.Generic;

[System.Serializable]
public class PatternFrameInfo {
	
	[System.Serializable]
	public class Preset
	{
		public int preset = 0;
		public float extraSpeed = 1f;
		public float extraAngle = 0f;
	}

	public int frame;

	public List<Preset> presets = new List<Preset>();

	public PatternFrameInfo (int f) {frame = f;}
}
