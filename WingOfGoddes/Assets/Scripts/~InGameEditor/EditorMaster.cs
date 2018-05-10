using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EditorMaster : MonoBehaviour {

	public enum EditorEnum
	{
		EventEditor = 0,
		PathEditor,
		EnemyEditor,
		PatternEditor,
		BulletEditor
	}

	public EditorEnum currEditor = EditorEnum.EventEditor;

	public EditorBase[] editors;
	public Button[] menus;

	public void Awake()
	{
		EventSwap((int)currEditor);
	}

	public void Save()
	{
		editors[(int)currEditor].SaveData();
	}

	public void EventSwap(int editor)
	{
		currEditor = (EditorEnum)editor;

		for(int i = 0; i < editors.Length; ++i)
		{
			editors[i].gameObject.SetActive(false);
			editors[i].editorMenu.SetActive(false);

			menus[i].interactable = true;
		}

		editors[(int)currEditor].gameObject.SetActive(true);
		editors[(int)currEditor].editorMenu.SetActive(true);
		menus[(int)currEditor].interactable = false;
	}
}
