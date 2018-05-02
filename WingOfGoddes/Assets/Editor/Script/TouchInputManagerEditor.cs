using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

[CustomEditor(typeof(TouchInputManager))]
public class TouchInputManagerEditor : Editor {

	private string createName = "";

	private TouchInputManager manager;
	private Define.RectF refRect = new Define.RectF();

	public void OnEnable()
	{
		manager = (TouchInputManager)target;
	}

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

		GUILayout.Space(5f);
		GUILayout.BeginVertical("box");

		GUILayout.Label("TouchPlaceList");

		for(int i = 0; i < manager.touchPlaceList.Count; ++i)
		{
			GUILayout.BeginHorizontal();

			GUIStyle s = new GUIStyle(EditorStyles.label);

			if(manager.touchAreaCanvas != null)
			{
				RectToPlace(manager.rectTransformList[i],ref refRect);
				if(manager.touchPlaceList[i].place.CompareTo(refRect))
				{
					s.normal.textColor = new Color(0f,.5f,.2f,1f);
				}
				else
					s.normal.textColor = new Color(0.5f,0f,0f,1f);
			}
			else
				s.normal.textColor = new Color(1f,1f,1f,1f);


			GUILayout.Label(manager.touchPlaceList[i].name,s);

			EditorGUI.BeginDisabledGroup(manager.touchAreaCanvas == null);

			if(GUILayout.Button("ObjectSync",GUILayout.Width(80f)))
			{
				ObjectSync(manager.touchPlaceList[i].place,manager.rectTransformList[i]);
			}

			if(GUILayout.Button("ValueSync",GUILayout.Width(80f)))
			{
				ValueSync(ref manager.touchPlaceList[i].place,refRect);
			}

			EditorGUI.EndDisabledGroup();

			if(GUILayout.Button("Delete",GUILayout.Width(60f)))
			{
				manager.DeletePlaceInfo(i);
			}

			GUILayout.EndHorizontal();
		}

		GUILayout.Space(5f);

		GUILayout.EndVertical();

		GUILayout.BeginHorizontal();
		GUILayout.Label("Name");
		createName = GUILayout.TextField(createName,GUILayout.Width(100f));

		if(GUILayout.Button("CreateTouchPlace",GUILayout.Width(120f)))
		{
			TouchInputManager.TouchPlaceInfo info = manager.CreatePlaceInfo(createName);
			if(manager.touchAreaCanvas != null)
			{
				CraetePlaceObject(info);
			}

			createName = "";
		}
		GUILayout.EndHorizontal();

		GUILayout.Space(5f);

		GUILayout.BeginHorizontal();
		GUILayout.Label("ScreenSize");
		manager.screenSize.x = EditorGUILayout.FloatField(manager.screenSize.x);
		manager.screenSize.y = EditorGUILayout.FloatField(manager.screenSize.y);

		if(GUILayout.Button("CreateTouchPlaceVisualizer"))
		{
			CraeteCanvas();
		}
		GUILayout.EndHorizontal();
	}

	public void CraeteCanvas()
	{
		if(manager.touchAreaCanvas != null)
			return;

		manager.rectTransformList.Clear();

		GameObject canvas = Instantiate(
			AssetDatabase.LoadAssetAtPath("Assets/Editor/Prefab/TouchPlaceCanvas.prefab", typeof(GameObject))) as GameObject;
		CanvasScaler c = canvas.GetComponent<CanvasScaler>();
		c.referenceResolution = manager.screenSize;

		manager.touchAreaCanvas = canvas.GetComponent<Canvas>();

		CraetePlaceObjects();
	}

	public void CraetePlaceObjects()
	{
		for(int i = 0; i < manager.touchPlaceList.Count; ++i)
		{
			CraetePlaceObject(manager.touchPlaceList[i]);
		}
	}

	public void CraetePlaceObject(TouchInputManager.TouchPlaceInfo placeInfo)
	{
		GameObject place = Instantiate(
			AssetDatabase.LoadAssetAtPath("Assets/Editor/Prefab/TouchPlace.prefab", typeof(GameObject))) as GameObject;

		place.GetComponentInChildren<Text>().text = placeInfo.name;

		RectTransform tp = place.GetComponent<RectTransform>();
		place.transform.SetParent(manager.touchAreaCanvas.transform,false);

		tp.sizeDelta = PlaceToSize(placeInfo.place);
		tp.anchoredPosition = PlaceToPosition(placeInfo.place);

		manager.rectTransformList.Add(tp);
	}

	public void RectToPlace(RectTransform rectTp,ref Define.RectF rect)
	{
		rect.min.x = (rectTp.anchoredPosition.x - rectTp.sizeDelta.x * .5f);
		rect.min.y = (rectTp.anchoredPosition.y - rectTp.sizeDelta.y * .5f);
		rect.max.x = (rectTp.anchoredPosition.x + rectTp.sizeDelta.x * .5f);
		rect.max.y = (rectTp.anchoredPosition.y + rectTp.sizeDelta.y * .5f);
	}

	public Vector2 PlaceToPosition(Define.RectF rect)
	{
		return new Vector2(rect.min.x + (rect.max.x - rect.min.x) * .5f, rect.min.y + (rect.max.y - rect.min.y) * .5f);
	}

	public Vector2 PlaceToSize(Define.RectF rect)
	{
		return new Vector2((rect.max.x - rect.min.x),(rect.max.y - rect.min.y));
	}
    
	public void ValueSync(ref Define.RectF rect, Define.RectF tg)
	{
		rect.min = tg.min;
		rect.max = tg.max;
	}

	public void ObjectSync(Define.RectF rect, RectTransform rectTp)
	{
		float xDist = rect.max.x - rect.min.x;
		float yDist = rect.max.y - rect.min.y;

		rectTp.anchoredPosition = new Vector2(rect.min.x + xDist / 2f,rect.min.y + yDist / 2f);
		rectTp.sizeDelta = new Vector2(xDist,yDist);
	}
}
