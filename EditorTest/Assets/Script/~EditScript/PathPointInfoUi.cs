using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PathPointInfoUi : MonoBehaviour {

	public Text type;
	public InputField speed;
	public InputField time;
	public InputField posX;
	public InputField posY;
	public Toggle patternStart;
	public Dropdown movementType;

	public PathDatabase.LineInfo line = null;

	public bool setUpCheck = true;

	void Start()
	{
		movementType.ClearOptions();
		List<Dropdown.OptionData> options = new List<Dropdown.OptionData>();

		int end = (int)MathEx.EaseType.End;
		for(int i = 0; i < end; ++i)
		{
			options.Add(new Dropdown.OptionData(((MathEx.EaseType)i).ToString()));
		}

		movementType.AddOptions(options);

		DisableAllUI();
	}

	public void SetUp(PathDatabase.LineInfo l)
	{
		EnableAllUI();

		if(line != null)
		{
			if(line.type != l.type)
				setUpCheck = true;
			else
				setUpCheck = false;
		}

		line = l;

		//type.text = "타입 : " + line.type.ToString();

		ValueUpdate();
		
		movementType.value = (int)line.type;
	}

	public void ValueUpdate()
	{
		speed.text = line.speed.ToString();
		time.text = line.stayTime.ToString();
		posX.text = line.point.x.ToString();
		posY.text = line.point.y.ToString();
		patternStart.isOn = line.patternStart;
	}

	public void DisableAllUI()
	{
		speed.interactable = 
			time.interactable =
			posX.interactable =
			posY.interactable =
			patternStart.interactable =
			movementType.interactable = false;
	}

	public void EnableAllUI()
	{
		speed.interactable = 
			time.interactable =
			posX.interactable =
			posY.interactable =
			patternStart.interactable =
			movementType.interactable = true;
	}

	public void ChangeSpeed()
	{
		line.speed = float.Parse(speed.text == "" ? "0" : speed.text);
		ValueUpdate();
	}

	public void ChangeStayTime()
	{
		line.stayTime = float.Parse(time.text == "" ? "0" : time.text);
		ValueUpdate();
	}

	public void ChangePatternToggle()
	{
		line.patternStart = patternStart.isOn;
		ValueUpdate();
	}

	public void ChangeMovementType()
	{
		if(!setUpCheck)
		{
			line.type = (MathEx.EaseType)movementType.value;

			PathEdit.instance.SetPosMarker();
			PathEdit.instance.SetPointMarkerToPath();
			PathEdit.instance.SetLineRenderer();
			PathEdit.instance.AllUiUpdate();
		}
		else
		{
			setUpCheck = false;
		}
		// if(line.type != (MathEx.EaseType)movementType.value)
		// {
		// 	line.type = (MathEx.EaseType)movementType.value;

		// 	PathEdit.instance.UpdatePointMarker();
		// 	PathEdit.instance.SetPointMarkerToPath();
		// 	PathEdit.instance.SetLineRenderer();
		// 	PathEdit.instance.AllUiUpdate();

		// 	Debug.Log("check");
		// }
	}
}
