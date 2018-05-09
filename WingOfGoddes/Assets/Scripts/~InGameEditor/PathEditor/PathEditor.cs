using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEngine.EventSystems;

public class PathEditor : EditorBase {

	public GameObject lineInfoMarkerBase;
	public LineRenderer lineRenderer;

	public bool picking = false;

	public Dropdown pathListUI;
	public InputField newPathNameUI;
	public InputField pathNameUI;

	public InputField stayTimeUI;
	public InputField speedUI;
	public InputField posXUI;
	public InputField posYUI;
	public Dropdown movementTypeUI;

	public Button pickButton;

	private PathDatabase pathDatabase;
	
	private List<LineInfoMarker> lineinfoMarkerList = new List<LineInfoMarker>();

	private string newPathName = "";

	private GraphicRaycaster gp;
	private PointerEventData ped;


	public void Awake()
	{
		pathDatabase = AssetDatabase.LoadAssetAtPath("Assets/Database/PathDatabase.asset", typeof(PathDatabase)) as PathDatabase;

		gp = canvas.GetComponent<GraphicRaycaster>();
        ped=  new PointerEventData(null);


		MovementTypeListSync();
		PathListSync();

		PathSync();

		//PointInfoUISet(false);
	}

	public void Update()
	{
		for(int i = 0; i < lineinfoMarkerList.Count; ++i)
		{
			lineinfoMarkerList[i].Progress(new MarkerBase.Sync[]{() => PointInfoUISync(),() => LineRendererSync()});
		}

		if(picking)
		{
			if(Input.GetMouseButtonDown(0))
			{
				Picking(Camera.main.ScreenToWorldPoint(Input.mousePosition));
			}
			else if(Input.GetMouseButtonDown(1))
			{
				picking = false;
				pickButton.interactable = true;
			}
		}
	}

	public void NewPathNameSync()
	{
		newPathName = newPathNameUI.text;
	}

	public void PathNameSync()
	{
		pathDatabase.data[pathListUI.value].name = pathNameUI.text;

		PathListSync();
	}

	public void PickButton()
	{
		picking = true;
		pickButton.interactable = false;
	}

	public void MovementTypeSync()
	{
		LineInfoMarker.select.info.type = (MathEx.EaseType)movementTypeUI.value;

		LineRendererSync();
	}

	public void PosSync()
	{
		LineInfoMarker.select.info.point.x = float.Parse(posXUI.text);
		LineInfoMarker.select.info.point.y = float.Parse(posYUI.text);
	}

	public void SpeedSync()
	{
		LineInfoMarker.select.info.speed = float.Parse(speedUI.text);
	}

	public void StayTimeSync()
	{
		LineInfoMarker.select.info.stayTime = float.Parse(stayTimeUI.text);
	}

	public void Picking(Vector3 pos)
	{
		ped.position = Input.mousePosition; 
        List<RaycastResult> results = new List<RaycastResult>();
        gp.Raycast(ped, results);
    	if (results.Count != 0) 
			return;

		CreateNewLineInfo(pos);
	}

	public void CreateNewLineInfo(Vector3 pos)
	{
		if(pathDatabase.data[pathListUI.value].lineCode == -1)
		{
			pathDatabase.data[pathListUI.value].startPoint.point = pos;
			AddLineInfoMarker(pathDatabase.data[pathListUI.value].startPoint);
			++pathDatabase.data[pathListUI.value].lineCode;
		}
		else
		{
			PathDatabase.LineInfo info = new PathDatabase.LineInfo(pos);
			pathDatabase.data[pathListUI.value].lines.Add(info);

			AddLineInfoMarker(info);
		}

		LineRendererSync();
	}

	public void CreateNewPath()
	{
		if(newPathName == "")
		{
			Debug.Log("Name is Empty");
			return;
		}

		if(pathDatabase.PathNameCheck(name))
		{
			pathDatabase.data.Add(new PathDatabase.PathInfo(newPathName));
			AddPathList(newPathName);

			pathListUI.value = pathDatabase.data.Count - 1;

			newPathName = "";
			newPathNameUI.text = "";
		}
		else
		{
			Debug.Log("Path Name Is Already Exists");
		}
	}

	public void LineRendererSync()
	{
		if(pathDatabase.data[pathListUI.value].lines.Count == 0)
		{
			lineRenderer.enabled = false;
			return;
		}
		else
		{
			lineRenderer.enabled = true;
		}
	
		Vector3[] poses = pathDatabase.data[pathListUI.value].InfoToVectorArray();

		lineRenderer.positionCount = poses.Length;

		lineRenderer.SetPositions(poses);
	}

	public void PathSync()
	{
		if(pathDatabase.data.Count == 0)
			return;

		DeleteAllLineInfoMarker();

		LineRendererSync();

		pathNameUI.text = pathDatabase.data[pathListUI.value].name;

		if(pathDatabase.data[pathListUI.value].lineCode == -1)
			return;
		
		AddLineInfoMarker(pathDatabase.data[pathListUI.value].startPoint);

		for(int i = 0; i < pathDatabase.data[pathListUI.value].lines.Count; ++i)
		{
			AddLineInfoMarker(pathDatabase.data[pathListUI.value].lines[i]);
		}
	}

	public void AddLineInfoMarker(PathDatabase.LineInfo info)
	{
		LineInfoMarker marker = Instantiate(lineInfoMarkerBase).GetComponent<LineInfoMarker>();
		marker.Set(info);

		lineinfoMarkerList.Add(marker);
	}

	public void DeleteAllLineInfoMarker()
	{
		LineInfoMarker.select = null;

		for(int i = 0; i < lineinfoMarkerList.Count; ++i)
		{
			Destroy(lineinfoMarkerList[i].gameObject);
		}

		lineinfoMarkerList.Clear();
	}

	public void PointInfoUISet(bool value)
	{
		stayTimeUI.interactable = value;
		speedUI.interactable = value;
		posXUI.interactable = value;
		posYUI.interactable = value;
		movementTypeUI.interactable = value;
	}

	public void PointInfoUISync()
	{
		if(LineInfoMarker.select == null)
		{
			PointInfoUISet(false);
		}
		else
		{
			PointInfoUISet(true);
			stayTimeUI.text = LineInfoMarker.select.info.stayTime.ToString();
			speedUI.text = LineInfoMarker.select.info.speed.ToString();
			posXUI.text = LineInfoMarker.select.info.point.x.ToString();
			posYUI.text = LineInfoMarker.select.info.point.y.ToString();
			movementTypeUI.value = (int)LineInfoMarker.select.info.type;
		}
	}

	public void PathInfoSync()
	{
		if(pathDatabase.data.Count == 0)
		{
			pathNameUI.interactable = false;
		}
		else
		{
			pathNameUI.text = pathDatabase.data[pathListUI.value].name;
			pathNameUI.interactable = true;
		}
	}

	public void MovementTypeListSync()
	{
		movementTypeUI.options.Clear();

		int count = (int)MathEx.EaseType.End;
		for(int i = 0; i < count; ++i)
		{
			movementTypeUI.options.Add(new Dropdown.OptionData(((MathEx.EaseType)i).ToString()));
		}

		movementTypeUI.value = 0;
	}

	public void PathListSync()
	{
		pathListUI.options.Clear();

		for(int i = 0; i < pathDatabase.data.Count; ++i)
		{
			AddPathList(pathDatabase.data[i].name);
		}
	}

	public void AddPathList(string op)
	{
		pathListUI.options.Add(new Dropdown.OptionData(op));
	}
}
