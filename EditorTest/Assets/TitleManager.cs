using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleManager : MonoBehaviour {

	void Update () {
		if(Input.GetMouseButtonUp(0))
			SceneLoader.instance.LoadScene(1);
	}
}
