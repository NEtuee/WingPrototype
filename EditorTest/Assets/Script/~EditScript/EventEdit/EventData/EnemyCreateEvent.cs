using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCreateEvent : EventBase {

	public int enemyCode;
	public int pathCode;
	public Vector3 pos;

	private EnemySpawner marker;

	public override void Progress()
	{
//적 생성
		EnemyBaseScript enemy = GameObject.Instantiate(Test.instance.enemyBase,
			(Vector3)marker.path.startPoint.point + pos,Quaternion.identity).GetComponent<EnemyBaseScript>();
		
		enemy.Init(Test.instance.enemy.data[enemyCode].patternRate,marker.path.speed, marker.path.constantSpeed, pos ,Test.instance.enemy.data[enemyCode].sprite,
					Test.instance.enemy.data[enemyCode].bullet,marker.path,Test.instance.enemy.data[enemyCode].shotPoint.ToArray());
		enemy.gameObject.transform.SetParent(Test.instance.inGameObjects);
	}
	public override void PickEvent()
	{
		marker.Init(frameCode);
		marker.gameObject.SetActive(true);
	}
	public override void ReleasePick()
	{
		if(!Test.instance.showAllMarker)
			marker.gameObject.SetActive(false);
	}
	public override string DataToString()
	{
		string data = "";

		data += 2 + ">";
		data += enemyCode + "/" + pathCode + "/" + pos.x + "/" + pos.y;

		return data;
	}
	public override void StringToData(string s)
	{
		string[] data = s.Split('/');

		enemyCode = int.Parse(data[0]);
		pathCode = int.Parse(data[1]);

		pos = new Vector2(float.Parse(data[2]),float.Parse(data[3]));

		marker.path = Test.instance.path.data[pathCode];
		marker.Init(frameCode);
	}
	
	public EnemyCreateEvent(int fc)
	{
		code = 2;
		frameCode = fc;
		pathCode = 0;
		enemyCode = 0;

		marker = Test.instance.CreateMarker(1).GetComponent<EnemySpawner>();
		marker.target = this;
		marker.path = Test.instance.path.data[pathCode];
		marker.Init(frameCode);
	}

	public EnemyCreateEvent(int fc,int ec,int pc)
	{
		code = 2;
		frameCode = fc;
		pathCode = pc;
		enemyCode = ec;

		marker = Test.instance.CreateMarker(1).GetComponent<EnemySpawner>();
		marker.target = this;
		marker.path = Test.instance.path.data[pathCode];
		marker.Init(frameCode);
	}
}
