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

	public Text clearTime;

	public void Set(int w,int s)
	{
		world = w;
		stage = s;
	}

	public void Start()
	{
		SceneManager.sceneLoaded += OnSceneLoaded;
		test.text = "world : " + world + ", stage : " + stage;

		SaveDataInfo.StageSaveInfo info = SaveDataContainer.instance.saveData.FindStageSave(world,stage);
		if(info != null)
		{
			clearTime.text = info.ValueToString();
		}
	}

	void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(SceneManager.GetActiveScene().buildIndex == 7)
		{
			GameRunningTest manager = GameObject.Find("GameObjectManager").GetComponent<GameRunningTest>();
			manager.stageData = worldDatabase.data[world].stageData[stage].stageEventData;
			manager.stageScript = worldDatabase.data[world].stageData[stage].stageDialog;
			manager.stage = stage;
			manager.world = world;
		}

		SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
