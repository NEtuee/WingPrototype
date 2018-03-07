using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MobileDebugger : MonoBehaviour {

	public static MobileDebugger instance;
	public Text text;
	public Text[] keep;

	public int lineNum = 0;

	public int currLine = 0;

	void Start () 
	{
		text.text = "";
		instance = this;
	}

	public void SetKeep(string line, int num)
	{
		keep[num].text = line;
	}

	public void AddLine(string line)
	{
		++lineNum;
		++currLine;
		text.text += currLine + " : " + line + "\n";

		if(lineNum >= 10)
		{
			PopLine();
			--lineNum;
		}
	}

	public void PopLine()
	{
		int l = 0;
		while(true)
		{
			if(text.text[l++] == '\n')
			{
				break;
			}

			// if(l >= 100)
			// {
			// 	Debug.Log("loopErr");
			// 	break;
			// }
		}

		text.text = text.text.Substring(l);
	}

	public void OnOff()
	{
		gameObject.SetActive(gameObject.activeSelf ? false : true);
	}
}
