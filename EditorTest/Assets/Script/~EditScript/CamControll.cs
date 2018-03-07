using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamControll : MonoBehaviour {

	public Transform tp;
	public Camera cam;

	private bool isMove = false;
	private Vector3 origin;
	private Vector3 objOrigin;
	private Vector3 mouse;

	void Update () {
		mouse = Input.mousePosition;
		float wheel = Input.GetAxis("Mouse ScrollWheel");
		if(Input.GetMouseButtonDown(2))
		{
			isMove = true;
			origin = mouse;
			objOrigin = tp.position;
		}
		else if(Input.GetMouseButton(2))
		{
			Vector3 pos = new Vector2(objOrigin.x - (mouse.x - origin.x) / 30f,objOrigin.y - (mouse.y - origin.y) / 30f);
			pos.z = -10;
			tp.position = pos;
		}

		if(wheel > 0)
		{
			cam.orthographicSize -= 0.5f;
			if(cam.orthographicSize < 1f)
				cam.orthographicSize = 1f;
		}
		else if(wheel < 0)
			cam.orthographicSize += 0.5f;
		
	}
}
