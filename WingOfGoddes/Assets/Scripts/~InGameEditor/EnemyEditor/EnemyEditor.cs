using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;


public class EnemyEditor : EditorBase {

	public class EnemyInfo
	{
		public EnemyDatabase.EnemyInfo info;
		public List<ShotPointMarker> markers = new List<ShotPointMarker>();

		public void ActiveMarkers()
		{
			for(int i = 0; i < markers.Count; ++i)
			{
				markers[i].gameObject.SetActive(true);
			}
		}

		public void DisableMarkers()
		{
			for(int i = 0; i < markers.Count; ++i)
			{
				markers[i].gameObject.SetActive(false);
			}
		}

		public void MarkerProgress()
		{
			for(int i = 0; i < markers.Count; ++i)
			{
				markers[i].Progress(null);
			}
		}
	}

	public GameObject markerBase;

	public InputField newEnemyNameUI;
	public Dropdown enemyListUI;

	private EnemyDatabase enemyDatabase;
	private List<EnemyInfo> infoList = new List<EnemyInfo>();

	private int prevList = 0;

	public void Start()
	{
		enemyDatabase = AssetDatabase.LoadAssetAtPath("Assets/Database/EnemyDatabase.asset", typeof(EnemyDatabase)) as EnemyDatabase;

		GetEnemyList();
		InfoListSync();

	}

	public void Update()
	{
		MarkerProgress();
	}

	public void DeleteShotPoint()
	{
		
	}

	public void MarkerSet()
	{
		ShotPointMarker.select = null;

		infoList[prevList].DisableMarkers();
		infoList[enemyListUI.value].ActiveMarkers();

		prevList = enemyListUI.value;
	}

	public void MarkerProgress()
	{
		if(infoList.Count > enemyListUI.value)
		{
			infoList[enemyListUI.value].MarkerProgress();
		}
	}

	public void InfoListSync()
	{
		for(int i = 0; i < enemyDatabase.data.Count; ++i)
		{
			EnemyInfo info = new EnemyInfo();
			info.info = enemyDatabase.data[i];
			CreateMarkers(info);

			infoList.Add(info);
		}
	}

	public void CreateMarkers(EnemyInfo info)
	{
		for(int i = 0; i < info.info.shotPointInfo.Count; ++i)
		{
			CreateMarker(info,info.info.shotPointInfo[i]);
		}
	}

	public void CreateMarker(EnemyInfo info, EnemyDatabase.ShotPointInfo shotPoint)
	{
		ShotPointMarker marker = Instantiate(markerBase).GetComponent<ShotPointMarker>();

		marker.info = shotPoint;
		marker.PositionSync();
		marker.transform.SetParent(this.transform);

		info.markers.Add(marker);
	}

	public void GetEnemyList()
	{
		string[] s = enemyDatabase.GetNames();
		enemyListUI.ClearOptions();

		if(s == null)
		{
			PopupWindow.instance.Active("Database is empty",Color.white);
			return;
		}

		for(int i = 0; i < s.Length; ++i)
		{
			enemyListUI.options.Add(new Dropdown.OptionData(s[i]));
		}

		enemyListUI.captionText.text = enemyListUI.options[0].text;
		enemyListUI.value = 0;
	}
}
