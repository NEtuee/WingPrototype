using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EditorScript : MonoBehaviour {

	public GameObject inGameObjects;
	public Button button;
	public MonoBehaviour script;

	public void OnClick()
	{
		inGameObjects.SetActive(true);
		gameObject.SetActive(true);
		button.interactable = false;
		script.enabled = true;
	}

	public void Release()
	{
		inGameObjects.SetActive(false);
		gameObject.SetActive(false);
		button.interactable = true;
		script.enabled = false;
	}
}
