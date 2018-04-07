using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleManager : MonoBehaviour {

	public GameObject progresser;
	public NameChecker nameCheck;
	public int screenWidth;
	public int screenHeight;

	public void Awake()
	{
		if(DontDestroy.instance == null)
			Instantiate(progresser);
	}

	public void Start()
	{
		Screen.SetResolution(screenWidth,screenHeight,true);
	}

	void Update () {
		if(Input.GetMouseButtonUp(0) && !nameCheck.nameChecking)
			SceneLoader.instance.LoadScene(Define.SceneInfo.MainMenu);
	}
}
