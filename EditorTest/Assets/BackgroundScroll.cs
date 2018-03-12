using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundScroll : ObjectBase {

	public class Back
	{
		public List<Transform> targets = new List<Transform>();
		public List<SpriteRenderer> sprites = new List<SpriteRenderer>();
		public float dist;
		public float distHelper;
		public float speed = 0;

		public bool rand = false;
		public int count;

		public void Set(float s) {speed = s;}
		public void SetRand() {rand = true;}

		public void SetGloom(float value)
		{
			int count = sprites.Count;
			for(int i = 0; i < count; ++i)
			{
				sprites[i].color = new Color(value,value,value,1f);
			}
		}

		public void Progress()
		{
			float helper = distHelper;
			
			for(int i = 0; i < count; ++i)
			{
				targets[i].transform.position += Vector3.left * speed * Time.deltaTime;
				if(targets[i].transform.position.x <= -(dist + helper))
				{
					Vector3 pos;
					if(rand)
						helper = Random.Range(distHelper,distHelper + 30f);

					if(i == count - 1f)
					{
						pos = targets[0].transform.position;
						pos.x += dist + helper;
					}
					else
					{
						pos = targets[i + 1].transform.position;
						pos.x += (i + 1) * dist + helper - 0.5f;
					}
					//Vector3 pos = targets[i].transform.position;
					// pos.x += ((i + 1) * dist * 2) + distHelper;

					targets[i].transform.position = pos;
				}
			}
		}

		public Back(){}
		public Back(GameObject tp,int ct = 1,float helper = 0f)
		{
			count = ct;
			distHelper = helper;


			SpriteRenderer spr = Instantiate(tp).GetComponent<SpriteRenderer>();
			dist = spr.sprite.texture.width * spr.transform.localScale.x / spr.sprite.pixelsPerUnit;

			targets.Add(spr.transform);
			sprites.Add(spr);

			for(int i = 1; i < count; ++i)
			{
				Transform t = Instantiate(tp).transform;
				t.position = new Vector3(t.position.x + i * dist + distHelper - 0.05f,t.position.y);

				targets.Add(t);
				sprites.Add(t.GetComponent<SpriteRenderer>());
			}
		}
	}

	public GameObject far;
	public GameObject middle;
	public GameObject near;
	public GameObject near_1;

	public Slider farSlider;
	public Slider middleSlider;
	public Slider nearSlider;

	public float farSpeed;
	public float middleSpeed;
	public float nearSpeed;
	public float nearSpeed_0;

	Back farBack;
	Back middleBack;
	Back nearBack_0;
	Back nearBack_1;

	public override void Initialize()
	{
		farBack = new Back(far,2);
		farBack.Set(farSpeed);
		middleBack = new Back(middle,2,10f);
		middleBack.Set(middleSpeed);
		nearBack_0 = new Back(near,2,40f);
		nearBack_0.Set(nearSpeed);
		nearBack_0.SetRand();
		nearBack_1 = new Back(near_1,2,32f);
		nearBack_1.Set(nearSpeed_0);
		nearBack_1.SetRand();
	}

	public override void Progress()
	{
		farBack.Progress();
		middleBack.Progress();
		nearBack_0.Progress();
		nearBack_1.Progress();
	}

	public override void Release()
	{

	}

	public void SetFarColor()
	{
		farBack.SetGloom(farSlider.value);
	}

	public void SetMiddleColor()
	{
		middleBack.SetGloom(middleSlider.value);
	}

	public void SetNearColor()
	{
		nearBack_0.SetGloom(nearSlider.value);
		nearBack_1.SetGloom(nearSlider.value);
	}
}
