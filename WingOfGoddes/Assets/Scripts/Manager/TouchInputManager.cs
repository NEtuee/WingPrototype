using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchInputManager : LinkProgressManagerBase<TouchInputManager> {
#if UNITY_EDITOR
	[HideInInspector]
	public List<RectTransform> rectTransformList = new List<RectTransform>();
	[HideInInspector]
	public Canvas touchAreaCanvas = null;
#endif

	public delegate void TouchCallBack();

	public Camera mainCam;

	[System.Serializable]
	public class TouchPlaceInfo
	{
		public string name;
		public Define.RectF place;
		public TouchInfo touch = new TouchInfo();

		public TouchPlaceInfo(string n, Define.RectF p)
		{
			name = n;
			place = p;

			touch = null;
		}
	}

	[System.Serializable]
	public class TouchInfo
	{
		public Touch touch = new Touch();
		public TouchCallBack began = delegate {};
		public TouchCallBack moved = delegate {};
		public TouchCallBack end = delegate {};

		public void TouchEnd()
		{
			touch.fingerId = -1;
			end();
		}

		public void SetBeganCallBack(TouchCallBack back) {began = back;}
		public void SetMovedCallBack(TouchCallBack back) {moved = back;}
		public void SetEndCallBack(TouchCallBack back) {end = back;}

		public TouchInfo ()
		{

		}
	}

	[HideInInspector]
	public List<TouchPlaceInfo> touchPlaceList = new List<TouchPlaceInfo>();
	[HideInInspector]
	public Vector2 screenSize = new Vector2();

	private Touch touchs;
	private float screenRatio;

	public override void Initialize()
	{
		screenRatio = Screen.width / screenSize.x; //가로 기준

		instance = this;
		mainCam = Camera.main;

		Proportion(screenRatio);
	}

	public override void Progress()
	{
		TouchCheck();
	}

	public override void Release()
	{

	}

	public void TouchCheck()
	{
#if UNITY_EDITOR
		if(Input.GetMouseButtonDown(0))
		{
			TouchPlaceInfo info = GetTouchPlace(Input.mousePosition);

			if(info != null)
			{
				info.touch.began();
			}
		}
		// else if(Input.GetMouseButton(0))
		// {
		// 	TouchPlaceInfo info = GetTouchPlace(Input.mousePosition);
		// 	if(info != null)
		// 	{
		// 		info.touch.moved();
		// 	}
		// }
		// else if(Input.GetMouseButtonUp(0))
		// {
		// 	TouchPlaceInfo info = GetTouchPlace(Input.mousePosition);
		// 	if(info != null)
		// 	{
		// 		info.touch.TouchEnd();
		// 	}
		// }
#else
		if(Input.touchCount > 0)
		{
			for(int i = 0; i < Input.touchCount; ++i)
			{
				touchs = Input.GetTouch(i);

				if(touchs.phase == TouchPhase.Began)
				{
					TouchPlaceInfo info = GetTouchPlace(touchs);
					if(info != null)
					{
						info.touch.touch = touchs;
						info.touch.began();
					}
				}
				else if(touchs.phase == TouchPhase.Moved)
				{
					TouchInfo info = GetTouchInfo(touchs);
					if(info != null)
					{
						info.moved();
					}
				}
				else if(touchs.phase == TouchPhase.Ended)
				{
					TouchInfo info = GetTouchInfo(touchs);
					if(info != null)
					{
						info.TouchEnd();
					}
				}
			}
		}
#endif
	}

	public TouchInfo GetTouchInfo(Touch touch)
	{
		for(int i = 0; i < touchPlaceList.Count; ++i)
		{
			if(touchPlaceList[i].touch.touch.fingerId == touch.fingerId)
				return touchPlaceList[i].touch;
		}

		return null;
	}

	public TouchInfo GetTouchInfo(string n)
	{
		TouchPlaceInfo info = GetTouchPlace(n);
		if(info != null)
			return info.touch;

		return null;
	}

	public TouchPlaceInfo GetTouchPlace(Touch touch)
	{
		return GetTouchPlace(touch.position);
	}

	public TouchPlaceInfo GetTouchPlace(Vector2 touch)
	{
		for(int i = 0; i < touchPlaceList.Count; ++i)
		{
			if(MathEx.PointInRect(touch, touchPlaceList[i].place))
			{
				return touchPlaceList[i];
			}
		}

		return null;
	}

	public TouchPlaceInfo GetTouchPlace(string n)
	{
		for(int i = 0; i < touchPlaceList.Count; ++i)
		{
			if(touchPlaceList[i].name.Equals(n))
			{
				return touchPlaceList[i];
			}
		}

		return null;
	}

	public TouchPlaceInfo CreatePlaceInfo(string name)
	{
		TouchPlaceInfo info = new TouchPlaceInfo(name,new Define.RectF(100f,200f,200f,100f));
		touchPlaceList.Add(info);

		return info;
	}

	public void DeletePlaceInfo(int num)
	{
		touchPlaceList.RemoveAt(num);

		RectTransform rect = rectTransformList[num];
		rectTransformList.RemoveAt(num);

		DestroyImmediate(rect.gameObject);
	}

	public void Proportion(float ratio)
	{
		for(int i = 0; i < touchPlaceList.Count; ++i)
		{
			touchPlaceList[i].place *= ratio;
		}
	}

	public Vector2 ScreenToWorldPoint(Vector2 touchPos)
	{
		return mainCam.ScreenToWorldPoint(touchPos);
	}
}