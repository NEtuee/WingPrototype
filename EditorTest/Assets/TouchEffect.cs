using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchEffect : MonoBehaviour {

	public GameObject particle;
	public int count = 10;

	public List<ParticleSystemChecker> particleList = new List<ParticleSystemChecker>();
	private Vector2 touchPos;

	public void Start()
	{
		for(int i = 0; i < count; ++i)
		{
			ParticleSystemChecker p = Instantiate(particle).GetComponent<ParticleSystemChecker>();
			p.transform.SetParent(this.transform);
			particleList.Add(p);
		}
	}

	void Update ()
	{
		if(Input.GetMouseButtonDown(0) && SceneLoader.instance.GetCurrScene() != Define.SceneInfo.MainStage)
		{
			touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

			Active(touchPos);
		}
	}
	public void Active(Vector2 pos)
	{
		for(int i = 0; i < count; ++i)
		{
			if(particleList[i].getParticleCount() == 0)
			{
				particleList[i].play();
				particleList[i].transform.position = pos;
				return;
			}
		}
	}
}
