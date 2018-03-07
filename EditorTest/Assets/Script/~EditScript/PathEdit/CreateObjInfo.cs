using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class CreateObjInfo : MonoBehaviour {

	public InputField code;
	public InputField posx;
	public InputField posy;
	public InputField angle;

	[System.Serializable]
	public class Info
	{
		public int code;
		public Vector2 pos;
		public float angle;
	}

	public void SetInfo(ObjectCreateEvent e)
	{
		code.text = e.code.ToString();
		posx.text = e.pos.x.ToString();
		posy.text = e.pos.y.ToString();
		angle.text = e.angle.ToString();
	}

	public Info GetInfo()
	{
		Info i = new Info();
		string[] check = {code.text,posx.text,posy.text,angle.text};
		int count = check.Length;
		for(int j = 0; j < count; ++j)
		{
			if(check[j] == "")
			{
				Debug.Log("NoValue");
				return null;
			}
		}

		i.code = int.Parse(code.text);
		i.pos = new Vector2(float.Parse(posx.text),float.Parse(posy.text));
		i.angle = float.Parse(angle.text);

		return i;
	}
}
