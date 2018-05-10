using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PatternEditMaster : EditorBase {

	public GameObject[] editMenu;
	public Button[] buttons;

	public void Awake()
	{
		MenuSwap(0);
	}

	public void MenuSwap(int index)
	{
		buttons[0].interactable = true;
		buttons[1].interactable = true;
		buttons[index].interactable = false;

		for(int i = 0; i < editMenu.Length; ++i)
		{
			editMenu[i].SetActive(false);
		}

		editMenu[index].SetActive(true);
		editMenu[index + 2].SetActive(true);
	}
}
