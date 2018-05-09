using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class EditorBase : MonoBehaviour {

	public Canvas canvas;
	public GameObject editorMenu;
	public virtual void SaveData(){}
}
