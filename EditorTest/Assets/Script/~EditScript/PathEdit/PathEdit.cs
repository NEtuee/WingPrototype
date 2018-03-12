using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PathEdit : MonoBehaviour {

	//public GameObject testObject;

	public static PathEdit instance;

	public PathPointInfoUi pointInfoUi;

	public GameObject posMarker;
	public Transform parent;

	public Canvas canvas;
	public Dropdown pathMenu;
	public InputField pathName;
	public Button pickingButton;
	public Text pointCount;
	public InputField speed;
	public Toggle constant;

	public Text nameDisplay;
	public Text speedDisplay;
	public Text constantDisplay;

	public LineRenderer pathVisualizer;

	public GameObject[] pointMarker;

	public bool pickingMode = false;

	public int lineCode = -1;

	public Vector3 valueSave;

	public PathDatabase pathInfo;
	public PathDatabase.PathInfo currPath;

	public MovePath testObj;

	private GraphicRaycaster gp;
	private PointerEventData ped;

	public void Start()
	{
		//testObj = Instantiate(testObject,Vector3.zero,Quaternion.identity).GetComponent<MovePath>();
		instance = this;
		testObj.gameObject.SetActive(false);

		gp = canvas.GetComponent<GraphicRaycaster>();
        ped=  new PointerEventData(null);

		PathMenuItemSync();

		//CreatePath();
	}

	public void Update()
	{
		if(pickingMode)
		{
			bool mouseDown = Input.GetMouseButtonDown(0);
			if(mouseDown)
			{
				ped.position = Input.mousePosition; 
        		List<RaycastResult> results = new List<RaycastResult>();
        		gp.Raycast(ped, results);
    	  		if (results.Count !=0) 
					return;
				
				Vector3 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
				mouse.z = 0f;

				if(currPath.lineCode == -1)
				{
					valueSave = mouse;
					SetActivePointMarker(0,true,mouse);
					AddFirstPoint(valueSave,mouse);
					CreatePosMarker(currPath.startPoint,PointMarker.MarkerType.LinePointer);
					++currPath.lineCode;
				}
				else if(currPath.lineCode == 0)
				{
					AddLine(mouse);
					SetLineRenderer();
					SetActivePointMarker(1,true,mouse);
					CreatePosMarker(currPath.lines[currPath.lines.Count - 1],PointMarker.MarkerType.LinePointer);
				}

			}
			else if(Input.GetMouseButtonDown(1))
			{
				pickingMode = false;
				pickingButton.interactable = true;
			}
		}
	}

	public void ChangeInfo()
	{
		if(pathName.text != "")
		{
			currPath.name = pathName.text;
			pathMenu.options[pathMenu.value].text = currPath.name;
		}
		if(speed.text != "")
			currPath.speed = float.Parse(speed.text);
		
		currPath.constantSpeed = constant.isOn;
		
		AllUiUpdate();
	}

	public void CreatePath()
	{
		PathDatabase.PathInfo info = new PathDatabase.PathInfo();

		lineCode = -1;
		if(pathName.text == "")
		{
			info.name = "New Path " + pathInfo.data.Count;
		}
		else
			info.name = pathName.text;
		if(speed.text == "")
			info.speed = 1f;
		else
			info.speed = float.Parse(speed.text);

		info.constantSpeed = constant.isOn;

		AddPathMenuItem(info.name);
		pathInfo.data.Add(info);
		PathMenuSelect(pathInfo.data.Count - 1);

		//SetCurrPath(info);
		SetEnablePointMarker();

		// AddFirstPoint(Vector3.zero,new Vector3(10,10,0));
	}

	public void PathMenuSelect(int value)
	{
		pathMenu.value = value;
		SetCurrPath();
	}

	public void AddPathMenuItem(string s)
	{
		pathMenu.options.Add(new Dropdown.OptionData(s));
	}

	public void PathMenuItemSync()
	{
		int count = pathInfo.data.Count;
		for(int i = 0; i < count; ++i)
		{
			AddPathMenuItem(pathInfo.data[i].name);
		}

		if(count > 0)
			PathMenuSelect(0);
	}

	public void StartTest()
	{
		testObj.SetUp(currPath);
	}

	public void StopTest()
	{
		testObj.Stop();
	}

	public void PointUiUpdate()
	{
		pointCount.text = "Point : " + currPath.lines.Count;
	}

	public void AllUiUpdate()
	{
		PointUiUpdate();
		nameDisplay.text = "Name : " + currPath.name;
		speedDisplay.text = "Speed : " + currPath.speed.ToString();
		constantDisplay.text = currPath.constantSpeed ? "Const : O" : "Const : X";
	}

	public void SetEnablePointMarker()
	{
		pointMarker[0].SetActive(false);
		pointMarker[1].SetActive(false);
	}

	public void SetActivePointMarker(int code,bool b,Vector3 pos)
	{
		pointMarker[code].SetActive(b);
		pointMarker[code].transform.position = pos;
	}

	public void SetPointMarkerToPath()
	{
		if(currPath.lines.Count != 0)
		{
			SetActivePointMarker(0,true,currPath.startPoint.point);
			//SetActivePointMarker(0,true,currPath.lines[0].start);
			SetActivePointMarker(1,true,currPath.lines[currPath.lines.Count - 1].point);
			//SetActivePointMarker(1,true,currPath.lines[currPath.lines.Count - 1].end);
		}
		else
			SetEnablePointMarker();
	}

	public void SetPickingMode(bool b)
	{
		pickingButton.interactable = false;
		pickingMode = b;
	}

	public void DeleteLatelyPoint()
	{
		if(currPath.lines.Count == 0)
			return;
		currPath.lines.RemoveAt(currPath.lines.Count - 1);
		SetLineRenderer();
		SetPointMarkerToPath();

		if(currPath.lines.Count == 0)
		{
			currPath.lineCode = -1;
			DeleteAllPosMarker();
		}
		else
		{
			SetPosMarker();
		}
	}

	public void SetLineRenderer()
	{
		if(currPath.lines.Count == 0)
		{
			pathVisualizer.positionCount = 0;
			return;
		}
		Vector3[] array = currPath.InfoToVectorArray();
		pathVisualizer.positionCount = array.Length;
		pathVisualizer.SetPositions(currPath.InfoToVectorArray());
	}

	List<PointMarker> ptList = new List<PointMarker>();
	public void CreatePosMarker(PathDatabase.LineInfo info, PointMarker.MarkerType type, int code = -1)
	{
		PointMarker pt = Instantiate(posMarker,Vector3.zero,Quaternion.identity).GetComponent<PointMarker>();
		pt.Set(info,type,code);

		pt.transform.SetParent(parent);

		ptList.Add(pt);
	}

	public void UpdatePointMarker()
	{
		int count = ptList.Count;
		for(int i = 0; i < count; ++i)
		{
			if(ptList[i].markerType == PointMarker.MarkerType.BezierPointer)
			{
				ptList[i].SetLine();
				PointMarker check = ptList[i].CheckType();
				if(check != null)
				{
					ptList.RemoveAt(i);
					Destroy(check.gameObject);
				}
			}
		}
	}

	public void UpdatePointUI(PathDatabase.LineInfo l)
	{
		pointInfoUi.SetUp(l);
	}

	public void SetPosMarker()
	{
		DeleteAllPosMarker();

		if(currPath.lines.Count == 0)
			return;

		CreatePosMarker(currPath.startPoint,PointMarker.MarkerType.LinePointer);

		int count = currPath.lines.Count;
		for(int i = 0; i < count; ++i)
		{
			CreatePosMarker(currPath.lines[i],PointMarker.MarkerType.LinePointer);

			if(currPath.lines[i].type == MathEx.EaseType.BezierCurve)
			{
				if(i == 0)
				{
					CreatePosMarker(currPath.lines[i],PointMarker.MarkerType.BezierPointer,0);
					CreatePosMarker(currPath.startPoint,PointMarker.MarkerType.BezierPointer,1);
				}
				else
				{
					CreatePosMarker(currPath.lines[i],PointMarker.MarkerType.BezierPointer,0);
					CreatePosMarker(currPath.lines[i - 1],PointMarker.MarkerType.BezierPointer,1);
				}
			}
		}
	}

	public void DeleteLatelyMarker()
	{
		int count = ptList.Count;
		PointMarker pt = ptList[count - 1];
		ptList.RemoveAt(count - 1);

		Destroy(pt.gameObject);

		UpdatePointMarker();
	}

	public void DeleteAllPosMarker()
	{
		int count = ptList.Count;
		for(int i = 0; i < count; ++i)
		{
			Destroy(ptList[i].gameObject);
		}

		ptList.Clear();
	}

	public void SetCurrPath(PathDatabase.PathInfo info)
	{
		currPath = info;
		SetPointMarkerToPath();
		SetLineRenderer();
		SetPosMarker();
		AllUiUpdate();
	}
	public void SetCurrPath(int code)
	{
		currPath = pathInfo.data[code];
		SetPointMarkerToPath();
		SetLineRenderer();
		SetPosMarker();
		AllUiUpdate();
	}
	public void SetCurrPath()
	{
		currPath = pathInfo.data[pathMenu.value];
		SetPointMarkerToPath();
		SetLineRenderer();
		SetPosMarker();
		AllUiUpdate();
	}
	public void AddFirstPoint(Vector2 start,Vector2 end)
	{
		currPath.startPoint = new PathDatabase.LineInfo(start);
		//currPath.lines.Add(new PathDatabase.LineInfo(start,end));
		PointUiUpdate();
	}
	public void AddLine(Vector2 end)
	{
		currPath.lines.Add(new PathDatabase.LineInfo(end));
		//currPath.lines.Add(new PathDatabase.LineInfo(currPath.lines[currPath.lines.Count - 1].end,end));
		PointUiUpdate();
	}
}
