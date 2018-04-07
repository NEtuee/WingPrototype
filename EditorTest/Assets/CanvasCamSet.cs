using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CanvasCamSet : MonoBehaviour {

	public Canvas canvas;
	
	void Start () {
		canvas = GetComponent<Canvas>();
		SceneManager.sceneLoaded += OnSceneLoaded;
	}
	
	void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        canvas.worldCamera = Camera.main;
    }
}
