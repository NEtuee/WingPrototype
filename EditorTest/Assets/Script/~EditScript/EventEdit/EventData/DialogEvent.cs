using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogEvent : EventBase {

	public static int start = 0;
	public List<TextEditScript.Options> options = new List<TextEditScript.Options>();

	public override void Progress()
	{
//dialog
		Time.timeScale = 0f;
		DialogTest.instance.Active(options);
	}
	public override void PickEvent()
	{

	}
	public override void ReleasePick()
	{

	}

	public override void ButtonClicked()
	{
		Test.instance.TextEdit.Link(this);
		Test.instance.TextEdit.gameObject.SetActive(Test.instance.TextEdit.gameObject.activeSelf ? false : true);
	}

	public override void ButtonRelease()
	{
		Test.instance.TextEdit.gameObject.SetActive(false);
	}

	public override string DataToString()
	{
		string s = code + ">";

		int count = options.Count;
		for(int i = 0; i < count; ++i)
		{
			s += options[i].auto;
			s += "/";
			s += options[i].skip;
			s += "/";
			s += options[i].speed;
			s += "/";
			s += options[i].talker;
			s += "/";
			s += options[i].name;
			s += "/";
			s += options[i].mainText;
			s += "*";
		}

		return s;
	}

	public override string DataToStringForGame()
	{
		string s = code + ">";
		s += start;
		
		start += options.Count;

		MainDataToString();

		return s;
	}

	public void MainDataToString()
	{
		int count = options.Count;
		for(int i = 0; i < count; ++i)
		{
			string s = "";
			s += options[i].auto;
			s += "/";
			s += options[i].skip;
			s += "/";
			s += options[i].speed;
			s += "/";
			s += options[i].talker;
			s += "/";
			s += options[i].name;
			s += "/";
			s += options[i].mainText;

			Test.instance.TextEdit.saveOptions.Add(s);
		}
	}

	public override void StringToData(string s)
	{
		string[] data = s.Split('*');

		int count = data.Length - 1;

		options.Clear();

		for(int i = 0; i < count; ++i)
		{
			TextEditScript.Options option = new TextEditScript.Options();
			string[] deepData = data[i].Split('/');
			
			option.auto = bool.Parse(deepData[0]);
			option.skip = bool.Parse(deepData[1]);
			option.speed = float.Parse(deepData[2]);
			option.talker = int.Parse(deepData[3]);
			option.name = deepData[4];
			option.mainText = deepData[5];

			options.Add(option);
		}
	}

	public DialogEvent()
	{
		code = 3;

		//TextEditScript.Options option = new TextEditScript.Options();
		options.Add(new TextEditScript.Options());
	}
}
