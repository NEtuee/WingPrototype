using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.IO;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Test : MonoBehaviour {

	public delegate void SaveCallBack(string s);
	public enum Events
	{
		Test = 0,
		ObjectCreate,
		EnemyCreate,
		Dialog,
		Static,
		WaitExtinc,
		End,
	}

	[System.Serializable]
	public class Frame
	{
		public string expl;
		public List<EventBase> events = new List<EventBase>();

		public Frame(){expl = "";}
		public Frame(string e){expl = e;}

		public bool IsEmpty() {return events.Count == 0;}
	}

	[System.Serializable]
	public class Container
	{
		public int framePerSec;
		public int page = 0;
		public float time;
		public List<List<Frame>> frames;

		public Container(){}
		public Container(int fps,bool newPage)
		{
			SetFps(fps);
			CreateNew(newPage);
		}

		public void SetFps(int fps)
		{
			framePerSec = fps;
			time = (100f / (float)fps) / 100f;
		}

		public void CreateNew(bool newPage)
		{
			frames = new List<List<Frame>>();
			if(newPage)
				AddPage();
		}

		public void AddPage()
		{
			frames.Add(new List<Frame>());
			for(int i = 0; i < 60; ++i)
			{
				frames[page].Add(new Frame());
			}
			++page;
		}
	}

	public ObjectDatabase objData;
	public Transform inGameObjects;
	public Transform markerObjects;

	public Container frameContainer;
	public SlideScript slide;
	public Canvas canvas;
	public Button[] eventList;
	public Toggle markerToggle;
	public LineRenderer screenEdge;

	public AddEventManager addEvent;

	public Button pagePrev;
	public Button pageNext;
	public Text pageText;
	public InputField frameText;

	public GameObject[] markers;
	public TextEditScript TextEdit;

	public GameObject enemyBase;
	public GameObject bulletBase;

	public EnemyDatabase enemy;
	public PathDatabase path;

	private int currPage = 0;
	public int currFrame = -1;
	public int enemyCount = 0;

	public bool showAllMarker = false;

	public bool staticEvent = false;
	public bool waitExtinc = false;

	public float width;
	public float height;

	private DataManager dataManager;
	public static Test instance;

	GraphicRaycaster gp;
	PointerEventData ped;

	public void Start()
	{
		instance = this;
		dataManager = GetComponent<DataManager>();//new DataManager();

		//if(!DataLoad())
			CreateNew(12,true);
		height = 2 * Camera.main.orthographicSize;
		width = height * Camera.main.aspect;

		Vector3[] points = {new Vector3(-width / 2f,height / 2f,1f),new Vector3(width / 2f,height / 2f,1f)
							,new Vector3(width / 2f, -height / 2f,1f),new Vector3( -width / 2f, - height / 2f,1f)};

		screenEdge.SetPositions(points);

		gp = canvas.GetComponent<GraphicRaycaster>();
        ped=  new PointerEventData(null);

		PageUiUpdate();
	}

	public void Update()
	{
		if(slide.IsRunning())
		{
			frameText.interactable = false;
			pageNext.interactable = false;
			pagePrev.interactable = false;
		}
		else if(!frameText.interactable)
		{
			frameText.interactable = true;
			PageUiUpdate();
		}
		if(Input.GetMouseButtonUp(0))
		{
			ped.position = Input.mousePosition; 
        	List<RaycastResult> results = new List<RaycastResult>();
        	gp.Raycast(ped, results);
    	    if (results.Count !=0) 
    	    { 
    	        GameObject obj = results[0].gameObject; 
        	    if (obj.CompareTag("FrameObject"))
            	{ 
                	currFrame = slide.Pick(obj.GetComponent<FrameScript>());
            	}
        	}
			else
			{
				// currFrame = -1;
				// slide.ReleasePick();
			}
		}

		if(!slide.IsRunning())
		{
			if(slide.IsSelect())
			{
				if(Input.GetKeyDown(KeyCode.LeftArrow) && currFrame != 0)
				{
					slide.Pick(--currFrame);
				}
				else if(Input.GetKeyDown(KeyCode.RightArrow) && currFrame != 59)
				{
					slide.Pick(++currFrame);
				}
			}
		}
	}

	public void PageUiUpdate()
	{
		if(currPage == 0)
			pagePrev.interactable = false;
		else
			pagePrev.interactable = true;
		if(currPage == frameContainer.page - 1)
			pageNext.interactable = false;
		else
			pageNext.interactable = true;

		pageText.text = "<" + (currPage + 1) + "/" + frameContainer.page + ">";
	}

	public void FpsUiUpdate()
	{
		frameText.text = frameContainer.framePerSec.ToString();
	}

	public void AddNewPage()
	{
		frameContainer.AddPage();
		currPage = frameContainer.page - 1;
		LinkFrame();
		PageUiUpdate();
	}

	public void PrevPage()
	{
		currPage--;
		LinkFrame();
		PageUiUpdate();
	}

	public void NextPage()
	{
		currPage++;
		LinkFrame();
		PageUiUpdate();
	}

	public void LinkFrame()
	{
		slide.Link(frameContainer.frames[currPage]);
	}

	public void AddEventForPick(int code)
	{
		if(!slide.IsSelect())
		{
			Debug.Log("select is null");
			return;
		}


		slide.AddEventForPick(addEvent.SetEvent(currFrame));
	}

	public GameObject CreateMarker(int code)
	{
		GameObject g = Instantiate(markers[code],Vector2.zero,Quaternion.identity) as GameObject;
		g.transform.SetParent(markerObjects);
		return g;
	}

	public void SetFramePerSec()
	{
		string s = frameText.text;
		if(s == "")
		{
			frameContainer.SetFps(12);
		}
		else
		{
			frameContainer.SetFps(int.Parse(s));
		}
	}

	public void DeleteAllGameObject(Transform tp = null)
	{
		if(tp == null)
			tp = inGameObjects;

		Transform[] tps = tp.GetComponentsInChildren<Transform>();
		if(tps == null)
			return;

		int count = tps.Length;
		for(int i = 1; i < count; ++i)
		{
			Destroy(tps[i].gameObject);
		}
	}

	public void MarkerEnable() {markerObjects.gameObject.SetActive(true);}
	public void MarkerDisable() {markerObjects.gameObject.SetActive(false);}

	public void MarkerToggle()
	{
		showAllMarker = markerToggle.isOn;
		if(showAllMarker)
			SetAllMarker(true);
		else
		{
			SetAllMarker(false);
			slide.Pick(currFrame);
		}
	}

	public void SetAllMarker(bool b)
	{
		//Transform[] tp = markerObjects.GetComponentsInChildren<Transform>();
		MarkerBase[] mb = Resources.FindObjectsOfTypeAll(typeof(MarkerBase)) as MarkerBase[];
		int len = mb.Length;
		for(int i = 0; i < len; ++i)
		{
			if(mb[i].name.Contains("(Clone)"))
				mb[i].gameObject.SetActive(b);
		}
	}

	public void ProgressStop()
	{
		Time.timeScale = 1f;

		staticEvent = false;
		waitExtinc = false;

		DialogTest.instance.gameObject.SetActive(false);
		slide.ProgressStop();
		DeleteAllGameObject();
		MarkerEnable();
	}

	public void ProgressThisPage(int frame)
	{
		staticEvent = false;
		waitExtinc = false;

		MarkerDisable();
		slide.ProgressThisPage(frame,frameContainer.time);
	}

	public void ProgressAll()
	{
		staticEvent = false;
		waitExtinc = false;

		currPage = 0;
		LinkFrame();
		PageUiUpdate();
		MarkerDisable();
		slide.ProgressAll(frameContainer.page,frameContainer.time,NextPage);
	}

	public void CreateNew(int frame,bool newPage)
	{
		frameContainer = new Container(frame,newPage);
		slide.Init(frameContainer);
	}

	public void DataSave(string path = "0")
	{
		List<string> data = new List<string>();
		string s = frameContainer.framePerSec + "/" + frameContainer.page;
		data.Add(s);
		s = currPage + "/" + currFrame;
		data.Add(s);

		for(int i = 0; i < frameContainer.page; ++i)
		{
			data.Add(i.ToString());
			for(int j = 0; j < frameContainer.frames[i].Count; ++j)
			{
				if(!frameContainer.frames[i][j].IsEmpty())
				{
					data.Add(j.ToString());
					for(int k = 0; k < frameContainer.frames[i][j].events.Count; ++k)
					{
						data.Add(frameContainer.frames[i][j].events[k].DataToString());
					}
					data.Add("//");
				}
			}
			data.Add("///");

		}

		dataManager.WriteStringToFile(data.ToArray(),path);

	}

	public void DataSave()
	{
		SaveDialog(new SaveCallBack(DataSave));
	}

    public void DataSaveForStageInfo(string path = "0")
    {

    }

	public void DataSaveForGame(string path = "0")
	{
		List<string> data = new List<string>();
		string s = frameContainer.framePerSec.ToString();
		data.Add(s);

		for(int i = 0; i < frameContainer.page; ++i)
		{
			for(int j = 0; j < frameContainer.frames[i].Count; ++j)
			{
				if(!frameContainer.frames[i][j].IsEmpty())
				{
					data.Add((j + (i * 60)).ToString());
					for(int k = 0; k < frameContainer.frames[i][j].events.Count; ++k)
					{
						data.Add(frameContainer.frames[i][j].events[k].DataToStringForGame());
					}
					data.Add("*");
				}
			}
		}

		dataManager.WriteStringToFile(data.ToArray(),path);
	}

	public void DialogDataSaveForGame(string path = "0")
	{
		dataManager.WriteStringToFile(TextEdit.saveOptions.ToArray(),path);
	}

	public void DataSaveForGame()
	{
		SaveDialog(new SaveCallBack(DataSaveForGame));
		SaveDialog(new SaveCallBack(DialogDataSaveForGame));
	}

	public EventBase GetEvent(int code,int frameCode)
	{
		EventBase eventBase;
		switch((Events)code)
		{
		case Events.Test:
			eventBase = new TestEvent();
			break;
		case Events.ObjectCreate:
			eventBase = new ObjectCreateEvent(frameCode);
			break;
		case Events.EnemyCreate:
			eventBase = new EnemyCreateEvent(frameCode);
			break;
		case Events.Dialog:
			eventBase = new DialogEvent();
			break;
		case Events.Static:
			eventBase = new StaticEvent();
			break;
		case Events.WaitExtinc:
			eventBase = new WaitExtinc();
			break;
		default:
			eventBase = null;
			break;
		}

		return eventBase;
	}

	public EventBase LoadEvent(string s, int frmaeCode)
	{
		string[] split = s.Split('>');
		int code = int.Parse(split[0]);
		EventBase eventBase = GetEvent(code,frmaeCode);

		if(eventBase != null)
			eventBase.StringToData(split[1]);

		return eventBase;
	}
	

	public bool DataLoad(string path = "0")
	{
		string[] data = dataManager.ReadStringFromFile(path);
		if(data == null)
			return false;

		DeleteAllGameObject();
		DeleteAllGameObject(markerObjects);

		string[] lineSplit = data[0].Split('/');

		CreateNew(int.Parse(lineSplit[0]),true);
		FpsUiUpdate();
		int page = int.Parse(lineSplit[1]);
		for(int i = 0; i < page - 1; ++i)
			frameContainer.AddPage();

		lineSplit = data[1].Split('/');
		currPage = int.Parse(lineSplit[0]);
		currFrame = int.Parse(lineSplit[1]);

		int dataPos = 2;

		while(true)
		{
			if(data[dataPos] == "////")
				break;
			int p = int.Parse(data[dataPos++]);
			while(true)
			{
				if(data[dataPos][0] == '/'
					&& data[dataPos][1] == '/'
					&& data[dataPos][2] == '/')
				{
					++dataPos;
					break;
				}

				int f = int.Parse(data[dataPos++]);
				int fc = 0;
				while(true)
				{
					if(data[dataPos][0] == '/' && data[dataPos][1] == '/')
					{
						++dataPos;
						break;
					}
					frameContainer.frames[p][f].events.Add(LoadEvent(data[dataPos++],f));
					frameContainer.frames[p][f].events[fc++].ReleasePick();
				}
			}
		}

		return true;
	}

	[DllImport("user32.dll")]
	private static extern void OpenFileDialog();
	[DllImport("user32.dll")]
	private static extern void SaveFileDialog();

	public void SaveDialog(SaveCallBack s)
	{
		System.Windows.Forms.SaveFileDialog sfd = new System.Windows.Forms.SaveFileDialog();
		sfd.Filter = "DataFile|*.dat|TextFile|*.txt";
		sfd.ShowDialog();
		if(sfd.FileName != "")
		{
			s(sfd.FileName);
		}
	}

	public void LoadDialog()
	{
		System.Windows.Forms.OpenFileDialog lfd = new System.Windows.Forms.OpenFileDialog();
 
      	lfd.ShowDialog();
		if(lfd.FileName != "")
		{
			DataLoad(lfd.FileName);
			slide.SetAllColor();

			LinkFrame();
			slide.Pick(currFrame);
			PageUiUpdate();
		}
		//Debug.Log(sfd.FileName);
	}

}