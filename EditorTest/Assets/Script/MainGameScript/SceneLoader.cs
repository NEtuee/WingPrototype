using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class SceneLoader : MonoBehaviour {

	public GameObject fader;
	public float fadeSpeed = 3300;

	public static SceneLoader instance;

	private float progress = 0f;
	private int currSceneNum = 0;
	private int prevSceneNum = 0;
	private bool sceneLoad = false;

	public void Start()
	{
		if(instance == null)
			instance = this;
	}

	public bool SceneLoading() {return sceneLoad;}

	public void LoadScene(int level)
	{
		if(sceneLoad)
			return;

		//fader = f == null ? null : f;
		sceneLoad = true;
		prevSceneNum = currSceneNum;
		StartCoroutine(LoadSceneCorou(level));
	}

	public void LoadPrevScene()
	{
		//fader = f == null ? null : f;
		LoadScene(prevSceneNum);
	}

	IEnumerator LoadSceneCorou(int lv)
	{
		while(true)
		{
			Vector3 pos = fader.transform.localPosition;
			pos.x -= fadeSpeed * Time.deltaTime;
			fader.transform.localPosition = pos;

			if(pos.x <= 0)
			{
				pos.x = 0;
				fader.transform.localPosition = pos;
				break;
			}

			yield return null;
		}

		AsyncOperation async = SceneManager.LoadSceneAsync(lv);
		async.allowSceneActivation = false;

		while(!async.isDone)
		{
			progress = async.progress;
			if(progress >= 0.9f)
			{
				progress = 1f;
				async.allowSceneActivation = true;
			}

			yield return null;
		}

		currSceneNum = lv;

		while(true)
		{
			Vector3 pos = fader.transform.localPosition;
			pos.x -= fadeSpeed * Time.deltaTime;

			fader.transform.localPosition = pos;

			if(pos.x <= -2083)
			{
				pos.x = 2083;
				fader.transform.localPosition = pos;
				break;
			}

			yield return null;
		}

		sceneLoad = false;
	}


}
