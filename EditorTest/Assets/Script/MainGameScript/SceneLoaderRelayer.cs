using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneLoaderRelayer : MonoBehaviour {


	public void LoadScene(int lv)
	{
		SceneLoader.instance.LoadScene(lv);
	}

	public void LoadPrevScene()
	{
		SceneLoader.instance.LoadPrevScene();
	}
}
