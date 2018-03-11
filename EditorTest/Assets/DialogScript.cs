using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class DialogScript : ObjectBase {

	public static DialogScript instance;

	[System.Serializable]
	public class Options
	{
		public bool skip = true;
		public bool auto = false;

		public float speed = 1f;
		public int talker = 0;//0 = 좌 1 = 우 2 = 둘 다 3 = 아무도

		public string name = "이름";
		public string mainText = "내용";
	}

	public GameRunningTest run;

	public Text nameText;
	public Text mainText;

	public List<Options> options = new List<Options>();

	public int start = 0;
	public int end = 0;

	public int currLine = 0;

	int count = 0;
	float time = 0f;
	int pos = 0;

	bool lineEnd = false;
	bool allEnd = false;

	public override void Initialize()
	{
		instance = this;
		if(run.stageScript == null)
			return;

		string[] data = run.stageScript.text.Split('\n');

		int count = data.Length;

		for(int i = 0; i < count - 1; ++i)
		{
			Options option = new Options();
			string[] deepData = data[i].Split('/');
			
			option.auto = bool.Parse(deepData[0]);
			option.skip = bool.Parse(deepData[1]);
			option.speed = float.Parse(deepData[2]);
			option.talker = int.Parse(deepData[3]);
			option.name = deepData[4];
			option.mainText = deepData[5];

			options.Add(option);
		}

		gameObject.SetActive(false);
	}

	public override void Progress()
	{
		currLine = start + count;

		if(lineEnd)
		{
			if(options[currLine].auto)
			{
				if(allEnd)
				{
					GameRunningTest.instance.dialogActive = false;
					gameObject.SetActive(false);
					return;
				}

				Sync(currLine);

				lineEnd = false;
			}
		}

		if(!lineEnd)
		{
			time += 0.0166f * options[currLine].speed;
			if(time >= 1f)
			{
				time = 0f;
				mainText.text += options[currLine].mainText[pos++];
				if(pos >= options[currLine].mainText.Length)
				{
					mainText.text = options[currLine].mainText;

					++count;
					pos = 0;
					lineEnd = true;

					if((count + start) > end)
					{
						count--;
						allEnd = true;
					}
				}
			}

			if(Input.GetMouseButtonUp(0) && options[currLine].skip)
			{
				mainText.text = options[currLine].mainText;
				++count;
				pos = 0;
				lineEnd = true;

				if((count + start) > end)
				{
					count--;
					allEnd = true;
				}
			}
		}
		else
		{
			if(Input.GetMouseButtonUp(0))
			{
				if(allEnd)
				{
					GameRunningTest.instance.dialogActive = false;
					gameObject.SetActive(false);
					return;
				}

				Sync(currLine);

				lineEnd = false;
			}
		}
	}

	public void Sync(int num)
	{
		nameText.text = options[num].name;
		// skip.text = "skip : " + options[num].skip.ToString();
		// auto.text = "auto : " + options[num].auto.ToString();
		// speed.text = "speed : " + options[num].speed.ToString();
		// talker.text = "talker : " + options[num].talker.ToString();

		mainText.text = "";
	}
	

	public override void Release()
	{

	}

	public void Active(int s,int e)
	{
		start = s;
		end = e;

		currLine = 0;

		time = 0f;

		count = 0;
		pos = 0;

		lineEnd = false;
		allEnd = false;

		GameRunningTest.instance.dialogActive = true;

		Sync(start);

		this.gameObject.SetActive(true);
	}
}
