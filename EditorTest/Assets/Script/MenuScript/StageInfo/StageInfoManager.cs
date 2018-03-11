using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StageInfoManager : MonoBehaviour {

	public TextAsset stageInfo;
	public WorldDatabase worldDatabase;

	public int world = 0;
	public int stage = 0;

	public Text test;

	public void Set(int w,int s)
	{
		world = w;
		stage = s;
	}

	public void Start()
	{
		SceneManager.sceneLoaded += OnSceneLoaded;
		test.text = "world : " + world + ", stage : " + stage;
	}

	void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(SceneManager.GetActiveScene().buildIndex == 7)
		{
			GameRunningTest manager = GameObject.Find("GameObjectManager").GetComponent<GameRunningTest>();
			manager.stageData = worldDatabase.data[world].stageData[stage].stageEventData;
			manager.stageScript = worldDatabase.data[world].stageData[stage].stageDialog;
		}

		SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
