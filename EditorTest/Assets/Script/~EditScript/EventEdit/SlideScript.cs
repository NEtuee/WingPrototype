using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlideScript : MonoBehaviour {

	public delegate void NextPage();

	public GameObject frameObj;
	public FrameScript select = null;
	private FrameScript[] frames;
	private bool running = false;

	public void Init(Test.Container con)
	{
		select = null;
		if(frames != null)
			Release();
		frames = new FrameScript[60];
		for(int i = 0; i < 60; ++i)
		{
			FrameScript obj = Instantiate(frameObj,Vector2.zero,Quaternion.identity).GetComponent<FrameScript>();
			obj.name = i.ToString();
			obj.transform.SetParent(transform);
			obj.transform.localScale = new Vector3(1f,1f,1f);
			float value = -590.5f + (i * 20f);
			obj.transform.localPosition = new Vector2(value,0f);

			obj.Init(con.frames[0][i]);
			frames[i] = obj;
		}
	}

	public bool IsRunning() {return running;}

	public void Release()
	{
		for(int i = 0; i < 60; ++i)
			Destroy(frames[i].gameObject);
	}

	public void Link(List<Test.Frame> frameList)
	{
		for(int i = 0; i < 60; ++i)
		{
			frames[i].ReleasePick();
			frames[i].SetFrame(frameList[i]);
		}
	}

	public void SetAllColor()
	{
		for(int i = 0; i < 60; ++i)
		{
			frames[i].SetColor();
		}
	}

	public void ReleasePick()
	{
		if(select !=null)
		{
			DisableEventList();
			select.ReleasePick();
			Test.instance.currFrame = -1;
			select = null;
		}
	}

	public int Pick(FrameScript frame)
	{
		if(select != frame)
		{
			ReleasePick();
			select = frame;
			select.Pick();
			EventListUpdate();
			return int.Parse(select.name);
		}
		else
		{
			DisableEventList();
			ReleasePick();
		}
		return -1;
	}

	public void Pick(int code)
	{
		if(code == -1)
			return;
		if(select != frames[code])
		{
			ReleasePick();
		}
		
		Test.instance.currFrame = code;
		select = frames[code];
		select.Pick();
		EventListUpdate();
	}

	public void DisableEventList()
	{
		int count = Test.instance.eventList.Length;
		for(int i = 0; i < count; ++i)
		{
			Test.instance.eventList[i].gameObject.SetActive(false);
		}
	}

	public void ButtonClicked(int code)
	{
		select.ButtonClicked(code);
	}

	public void EventListUpdate()
	{
		DisableEventList();
		int count = select.GetFrame().events.Count;
		for(int i = 0; i < count; ++i)
		{
			Test.instance.eventList[i].transform.GetComponentInChildren<Text>().text = ((Test.Events)select.GetFrame().events[i].code).ToString();
			
			Test.instance.eventList[i].onClick.RemoveAllListeners();
			int j = i;
			Test.instance.eventList[i].onClick.AddListener(delegate{ButtonClicked(j);});

			Test.instance.eventList[i].gameObject.SetActive(true);
		}
		
	}

	public bool IsSelect() {return select != null;}

	public void AddEventForPick(EventBase eventBase)
	{
		select.AddEvent(eventBase);
		EventListUpdate();
	}

	public void ProgressThisPage(int start,float time)
	{
		if(running)
			return;

		ReleasePick();

		running = true;
		StartCoroutine(Progress(start,time));
	}

	public void ProgressAll(int page,float time,NextPage nextPage)
	{
		if(running)
			return;

		ReleasePick();

		running = true;
		StartCoroutine(Progress(page,time,nextPage));
	}

	public void ProgressStop()
	{
		running = false;
		StopAllCoroutines();
		SetAllColor();
	}

	public IEnumerator Progress(int start,float time)
	{
		int i = start;
		Debug.Log(time);
		while(true)
		{
			if(!Test.instance.waitExtinc)
			{
				frames[i].Progress();
				yield return new WaitForSeconds(time);
				if(!Test.instance.waitExtinc)
					frames[i++].ProgressEnd();
				else
					++i;
				if(i == 60)
					break;
			}
			else
			{
				Test.instance.waitExtinc = Test.instance.enemyCount == 0 ? false : true;
				yield return null;
			}
		}

		Test.instance.DeleteAllGameObject();
		Test.instance.MarkerEnable();
		running = false;
	}

	public IEnumerator Progress(int page,float time,NextPage nextPage)
	{
		int i = 0;
		int p = 0;
		while(true)
		{
			if(!Test.instance.waitExtinc)
			{
				frames[i].Progress();
				yield return new WaitForSeconds(time);
				if(!Test.instance.waitExtinc)
					frames[i++].ProgressEnd();
				else
					++i;
				if(i == 60)
				{
					if(p == page - 1)
						break;
					i = 0;
					++p;
					nextPage();
				}
			}
			else
			{
				Test.instance.waitExtinc = Test.instance.enemyCount == 0 ? false : true;
				yield return null;
			}

		}

		Test.instance.DeleteAllGameObject();
		Test.instance.MarkerEnable();
		running = false;
	}
}
