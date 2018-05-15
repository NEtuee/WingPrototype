using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FrameScript : MonoBehaviour {

	private Test.Frame frame;
	private Image icon;
	private bool pick = false;

	public void Init(Test.Frame f)
	{
		icon = GetComponent<Image>();
		SetFrame(f);
	}

	public Test.Frame GetFrame() {return frame;}

	public void SetFrame(Test.Frame f)
	{
		frame = f;

		SetColor();
	}

	public void SetColor()
	{
		if(frame.events.Count == 0)
		{
			icon.color = Color.gray;
		}
		else
		{
			icon.color = pick ? Color.green : Color.white;
		}

	}

	public void ButtonClicked(int code)
	{
		frame.events[code].ButtonClicked();
	}

	public void Pick()
	{
		pick = true;
		icon.color = Color.green;
		int count = frame.events.Count;
		for(int i = 0; i < count; ++i)
		{
			frame.events[i].PickEvent();
		}
		//return this;
	}

	public void ReleasePick()
	{
		pick = false;
		SetColor();
		int count = frame.events.Count;
		for(int i = 0; i < count; ++i)
		{
			frame.events[i].ReleasePick();
			frame.events[i].ButtonRelease();
		}
	}

	public bool DeleteEvent(int index)
	{
		if(frame.events.Count > index)
		{
			frame.events[index].ButtonRelease();
			frame.events[index].DeleteEvent();
			frame.events.RemoveAt(index);

			return true;
		}

		return false;
	}
	public void AddEvent(EventBase eventBase)
	{
		frame.events.Add(eventBase);
		SetColor();
	}

	public void Progress()
	{
		icon.color = Color.blue;
		int c = frame.events.Count;
		if(c == 0)
			return;
			
		for(int i = 0; i < c; ++i)
		{
			frame.events[i].Progress();
		}
	}


	public void ProgressEnd()
	{
		SetColor();
		//end
	}
}
