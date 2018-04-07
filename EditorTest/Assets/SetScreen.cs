using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetScreen : MonoBehaviour {

	public void Awake()
	{
		Screen.SetResolution( Screen.width, (Screen.width * 9) / 16 , true);
		Destroy(this);
	}
}
