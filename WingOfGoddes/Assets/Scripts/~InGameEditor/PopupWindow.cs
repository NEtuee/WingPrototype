using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupWindow : MonoBehaviour {

	public static PopupWindow instance;

	public Text text;
	public Image image;
	public AnimationCurve curve;

	private float time = 0f;
	private Color popupColor;

	void Awake () 
	{
		instance = this;

		popupColor = Color.white;
		gameObject.SetActive(false);
	}
	
	void Update () 
	{
		time += Time.deltaTime;
		popupColor.a = curve.Evaluate(time);
		SetColor(popupColor);

		if(time >= 4f)
		{
			gameObject.SetActive(false);
		}
	}

	public void Active(string s, Color color)
	{
		text.text = s;
		popupColor = color;
		time = 0f;

		SetColor(popupColor);
		gameObject.SetActive(true);
	}

	public void SetColor(Color c)
	{
		Color cc = Color.black;
		cc.a = c.a;
		text.color = cc;
		image.color = c;
	}
}
