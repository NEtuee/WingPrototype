using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIParticlePlacer : ObjectBase {

	public RectTransform gb;
	public ParticleSystem particle;
	public Image image;

	public float size = 0f;

	public override void Initialize()
	{

	}

	public override void Progress()
	{
		if(image.fillAmount > 0.85f && particle.isPlaying)
		{
			particle.Stop();
		}
		else if(image.fillAmount < 0.85f && image.fillAmount != 0f)
		{
			particle.Play();
		}

		Vector3 pos = Vector3.zero;
		float value = 140f * image.fillAmount;
		pos.y += -70f + value + 5f;
		gb.localPosition = pos;

		Vector3 box = particle.shape.box;


		float fill = image.fillAmount / 0.5f - 1f;
		float circular = Mathf.Sqrt(1 - fill * fill);

		float val = size * circular;

		box.x = val;

		ParticleSystem.ShapeModule shapeModule = particle.shape;
		shapeModule.box = box;

	}

	public override void Release()
	{

	}
}
