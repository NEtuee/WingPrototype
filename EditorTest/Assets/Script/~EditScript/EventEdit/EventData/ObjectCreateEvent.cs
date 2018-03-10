using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectCreateEvent : EventBase {

	public int objCode = -1;
	public Vector2 pos;
	public float angle;

	private Positioner marker;

	public override void Progress()
	{
		Transform tp = GameObject.Instantiate(Test.instance.objData.data[objCode],pos,Quaternion.Euler(0,0,angle)).transform;
		tp.SetParent(Test.instance.inGameObjects);
	}
	public override void PickEvent()
	{
		marker.gameObject.SetActive(true);
	}
	public override void ReleasePick()
	{
		if(!Test.instance.showAllMarker)
			marker.gameObject.SetActive(false);
	}

	public override string DataToString()
	{
		string s = 1 + ">" + objCode + "/" + pos.x.ToString() + "/" + pos.y.ToString() + "/" + angle.ToString();
		//Debug.Log(s);
		return s;
	}

	public override string DataToStringForGame()
	{
		return DataToString();
	}
	
	public override void StringToData(string s)
	{
		string[] split = s.Split('/');
		objCode = int.Parse(split[0]);
		pos = new Vector2(float.Parse(split[1]),float.Parse(split[2]));
		angle = float.Parse(split[3]);
		MarkerUpdate();
	}

	public void MarkerUpdate()
	{
		marker.tp.position = pos;
		marker.tp.eulerAngles = new Vector3(0,0,angle);
	}
	public ObjectCreateEvent(int fc)
	{
		frameCode = fc;
		code = 1;
		objCode = 0;
		pos = Vector2.zero;
		angle = 0;

		marker = Test.instance.CreateMarker(0).GetComponent<Positioner>();
		marker.Init(frameCode);
		marker.target = this;
		MarkerUpdate();
	}
	public ObjectCreateEvent(int c,Vector2 p, float a ,int fc)
	{
		frameCode = fc;
		code = 1;
		objCode = c;
		pos = p;
		angle = a;

		marker = Test.instance.CreateMarker(0).GetComponent<Positioner>();
		marker.Init(frameCode);
		marker.target = this;
		MarkerUpdate();
	}
}
