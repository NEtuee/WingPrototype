using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextEditScript : MonoBehaviour {

	public Toggle skip;
	public Toggle auto;
	public InputField speed;
	public Dropdown talker;

	public InputField nameText;
	public InputField mainText;

	public Button prev;
	public Button next;

	public Text pageViewer;

	public int currPage = 0;

	public class Options
	{
		public bool skip = true;
		public bool auto = false;

		public float speed = 1f;
		public int talker = 0;//0 = 좌 1 = 우 2 = 둘 다 3 = 아무도

		public string name = "이름";
		public string mainText = "내용";
	}

	public List<string> saveOptions = new List<string>();

	//Options options = new Options();

	//public List<string> textList = new List<string>();

	public DialogEvent target;

	public void Set(int code)
	{
		skip.isOn = target.options[code].skip;
		auto.isOn = target.options[code].auto;

		speed.text =  target.options[code].speed.ToString();
		talker.value = target.options[code].talker;

		nameText.text = target.options[code].name;
		mainText.text = target.options[code].mainText;

		// options.skip = target.options[code].skip;
		// options.auto = target.options[code].auto;
		// options.speed = target.options[code].speed;
		// options.talker = target.options[code].talker;

		// options.name = target.options[code].name;
		// options.mainText = target.options[code].mainText;
	}

	public void MainTextSync()
	{
		target.options[currPage].name = nameText.text;
		target.options[currPage].mainText = mainText.text;
	}

	public void OptionSync()
	{
		SkipSync();
		AutoSync();
		SpeedSync();
		TalkerSync();
	}

	public void SkipSync()
	{
		target.options[currPage].skip = skip.isOn;
	}

	public void AutoSync()
	{
		target.options[currPage].auto = auto.isOn;
	}

	public void SpeedSync()
	{
		target.options[currPage].speed =  float.Parse(speed.text);
	}

	public void TalkerSync()
	{
		target.options[currPage].talker = talker.value;
	}

	public void Display()
	{
		// skip.isOn = options.skip;
		// auto.isOn = options.auto;

		// speed.text = options.speed.ToString();
		// talker.value = options.talker;

		// nameText.text = options.name;
		// mainText.text = options.mainText;
	}

	public void PageViewerUpdate()
	{
		pageViewer.text = "(" + (currPage + 1) + "/" + target.options.Count + ")";
	}

	public bool ViewPage(int page)
	{
		if(page < 0 || page >= target.options.Count)
		{
			//Debug.Log("page Error");
			return false;
		}

		currPage = page;

		// MainTextSync();
		// OptionSync();

		Set(page);
		//Display();

		if(page == 0)
			prev.interactable = false;
		else
			prev.interactable = true;
		// if(page == target.options.Count - 1)
		// 	next.interactable = false;

		PageViewerUpdate();

		return true;
	}

	public void CreateNextPage()
	{
		int page = target.options.Count;

		target.options.Add(new Options());
		ViewPage(page);
	}

	public void ViewNextPage()
	{
		if(!ViewPage(currPage + 1))
		{
			CreateNextPage();
		}
	}

	public void ViewPrevPage()
	{
		ViewPage(currPage - 1);
	}

	public void Link(DialogEvent t)
	{
		target = t;

		ViewPage(0);
	}

}
