using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSelectManager : MonoBehaviour {

	public GameObject[] worldObjects;
	public List<StageObject> stageObjects = new List<StageObject>();

	public int count = 0;
	public int world = 0;

	public void Start()
	{
		GetStageObjects();

		ClearDataLoad();

		count = stageObjects.Count;
	}

	public void Update()
	{
		StageObject stage = ClickCheck();

		if(stage != null)
		{
			GameObject obj = new GameObject();
			StageInfoRelayer re = obj.AddComponent<StageInfoRelayer>();

			re.Set(world,stage.stage);

			SceneLoader.instance.LoadScene(Define.SceneInfo.StageInfo);
		}
	}

	public void ClearDataLoad()
	{
		//doing stuff
	}

	public void GetStageObjects()
	{
		Instantiate(worldObjects[world],Vector3.zero,Quaternion.identity);

		StageObject[] objs = GameObject.FindObjectsOfType<StageObject>() as StageObject[];
		int num = objs.Length;

		for(int i = 0; i < num; ++i)
		{
			SaveDataInfo.StageSaveInfo info = SaveDataContainer.instance.saveData.FindStageSave(world,objs[i].stage);
			if(info != null)
			{
				objs[i].SetClear(true);
//				objs[i].stageSave = info;
			}
			objs[i].Init();
			stageObjects.Add(objs[i]);
		}
	}

	public StageObject ClickCheck()
	{
		if(Input.GetMouseButtonUp(0))
		{
			Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			mousePos.z = 0f;

			for(int i = 0; i < count; ++i)
			{
				if(stageObjects[i].coll.OverlapPoint(mousePos))
				{
					return stageObjects[i];
				}
			}
		}
		else
			return null;

		return null;
	}
}
