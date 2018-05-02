using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NameChecker : MonoBehaviour {

	public SaveDataContainer container;
	public GameObject nameInput;
	public bool nameChecking = false;

	public Text errorText;
	public InputField inputField;

	public void Start ()
	{
		if(SaveDataContainer.instance.saveData.userName == "")
		{
            container = SaveDataContainer.instance;
			nameChecking = true;
			nameInput.SetActive(true);
		}
	}

	public void NameCheck()
	{
		if(inputField.text.Contains(" "))
		{
			errorText.text = "공백 문자 포함 불가";
			return;
		}
		else if(inputField.text.Length <= 2 || inputField.text.Length > 6)
		{
			errorText.text = "3글자 이상 6글자 이하만 가능";
			return;
		}
		
		container.saveData.userName = inputField.text;
		nameChecking = false;

		container.saveData.DataSave();
		nameInput.SetActive(false);
	}

}
