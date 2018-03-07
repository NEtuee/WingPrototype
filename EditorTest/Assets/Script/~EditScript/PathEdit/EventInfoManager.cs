using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventInfoManager : MonoBehaviour {

	public GameObject[] menus;
	public CreateObjInfo coinfo;
	public CreateEnemyInfo ceinfo;

	private GameObject currMenu;
	public void MenuActive(int code)
	{
		if(currMenu != null)
			currMenu.SetActive(false);
		
		currMenu = menus[code];
		currMenu.SetActive(true);
	}

	public CreateObjInfo.Info GetCreateObjInfo()
	{
		return coinfo.GetInfo();
	}

	public string GetCreateEnemyInfo()
	{
		return ceinfo.GetInfo();
	}
}
