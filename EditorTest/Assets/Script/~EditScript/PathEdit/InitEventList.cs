using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InitEventList : MonoBehaviour {

	public GameObject origin;

	private Button currButton;

	private Button[] buttons;

	void Start () {
		int count = (int)Test.Events.End;
		buttons = new Button[count];

		for(int i = 0; i < count; ++i)
		{
			Button bt = Instantiate(origin,Vector3.zero,Quaternion.identity).GetComponent<Button>();
			bt.GetComponentInChildren<Text>().text = ((Test.Events)i).ToString();
			bt.transform.SetParent(transform);
			bt.transform.localPosition = new Vector2(0f,208f - 30f * (float)i);
			int c = i;
			bt.onClick.AddListener(() => ButtonClicked(c));

			buttons[i] = bt;
		}
	}
	
	public void ButtonClicked(int code)
	{
		if(currButton != null)
			currButton.interactable = true;

		currButton = buttons[code];
		currButton.interactable = false;

		Test.instance.addEvent.SetEventCode(code);
		
	}
}
