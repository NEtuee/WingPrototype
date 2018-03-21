using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageInfoRelayer : MonoBehaviour {

	public int world = 0;
	public int stage = 0;

	public void Set(int w,int s)
	{
		world = w;
		stage = s;
	}

	void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StageInfoManager manager = GameObject.Find("Manager").GetComponent<StageInfoManager>();
		manager.Set(world,stage);

		SceneManager.sceneLoaded -= OnSceneLoaded;

		//Destroy(GameObject.Find("Gold"));
    }
}
