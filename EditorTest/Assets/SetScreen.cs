using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetScreen : MonoBehaviour {

	public void Awake()
	{
		Screen.SetResolution( Screen.width, (Screen.width * 16) / 9 , true);
		Destroy(this);
	}
}
