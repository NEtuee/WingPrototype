using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorMenu : MonoBehaviour {

	public EditorScript[] editors;

	private EditorScript curr = null;

	public void Start()
	{
		for(int i = 0; i < editors.Length; ++i)
			if(editors[i].gameObject.activeSelf)
				ButtonClicked(i);
	}

	public void ButtonClicked(int code)
	{
		if(curr != null)
			curr.Release();

		editors[code].OnClick();
		curr = editors[code];
	}
}
