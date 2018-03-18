using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleManager : MonoBehaviour {

	public NameChecker nameCheck;
	public int screenWidth;
	public int screenHeight;

	public void Start()
	{
		Screen.SetResolution(screenWidth,screenHeight,true);
	}

	void Update () {
		if(Input.GetMouseButtonUp(0) && !nameCheck.nameChecking)
			SceneLoader.instance.LoadScene(1);
	}
}
