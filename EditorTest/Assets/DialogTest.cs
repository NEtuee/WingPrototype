using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogTest : MonoBehaviour {

	public Text nameText;
	public Text mainText;
	public Text skip;
	public Text auto;
	public Text speed;
	public Text talker;

	public static DialogTest instance;
	List<TextEditScript.Options> options;


	int count = 0;
	float time = 0f;
	int pos = 0;

	bool end = false;
	bool allEnd = false;

	public void Start()
	{
		instance = this;
		this.gameObject.SetActive(false);
	}

	public void Active(List<TextEditScript.Options> op)
	{
		options = op;
		time = 0f;

		count = 0;
		pos = 0;

		end = false;
		allEnd = false;
		
		Sync(count);

		this.gameObject.SetActive(true);
	}

	public void Sync(int num)
	{
		nameText.text = options[num].name;
		skip.text = "skip : " + options[num].skip.ToString();
		auto.text = "auto : " + options[num].auto.ToString();
		speed.text = "speed : " + options[num].speed.ToString();
		talker.text = "talker : " + options[num].talker.ToString();

		mainText.text = "";
	}

	public void Update()
	{
		if(end)
		{
			if(options[count - 1].auto)
			{
				if(allEnd)
				{
					gameObject.SetActive(false);
					Time.timeScale = 1f;
					return;
				}

				Sync(count);

				end = false;
			}
		}

		if(!end)
		{
			time += 0.0166f * options[count].speed;
			if(time >= 1f)
			{
				time = 0f;
				mainText.text += options[count].mainText[pos++];
				if(pos >= options[count].mainText.Length)
				{
					mainText.text = options[count].mainText;

					++count;
					pos = 0;
					end = true;

					if(count >= options.Count)
					{
						allEnd = true;
					}
				}
			}

			if(Input.GetMouseButtonUp(0) && options[count].skip)
			{
				mainText.text = options[count].mainText;
				++count;
				pos = 0;
				end = true;

				if(count >= options.Count)
				{
					allEnd = true;
				}
			}
		}
		else
		{
			if(Input.GetMouseButtonUp(0))
			{
				if(allEnd)
				{
					gameObject.SetActive(false);
					Time.timeScale = 1f;
					return;
				}

				Sync(count);

				end = false;
			}
		}

		Time.timeScale = 0f;
	}
}
