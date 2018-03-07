using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectCreater : MonoBehaviour {

	public GameObject origin;
	public string objTag;

	public void Start()
	{
		GameObject obj = GameObject.FindGameObjectWithTag(objTag);
		if(obj != null)
			return;

		Instantiate(origin,Vector3.zero,Quaternion.identity);
	}
}
