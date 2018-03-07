using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnOffMenu : MonoBehaviour {

	public GameObject target;

	public void ButtonClicked()
	{
		target.SetActive(target.activeSelf ? false : true);
	}
}
