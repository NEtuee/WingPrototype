using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class EnemyEdit : MonoBehaviour {

	public static EnemyEdit instance;

	public GameObject frameOrigin;
	public GameObject bulletOrigin;
	public GameObject shotPointOrigin;
	public GameObject LineMarkerOrigin;
	public Transform frameContainer;
	public Transform inGameObjects;

	public Canvas canvas;
	public Dropdown enemyList;
	public SpriteRenderer enemy;
	public Text nameUi;
	public Text fpsUi;
	public Text firerateUi;
	public Text hpUi;

	public Text[] shotPointUi;

	public EnemyFrame currFrame;
	public int frameCode = -1;
	public FirePointMarker currPoint;
	public BulletLineMarker currMarker;
	
	public EnemyDatabase database;

	public EnemyDatabase.EnemyInfo currInfo;

	private List<EnemyFrame> frames = new List<EnemyFrame>();
	private List<FirePointMarker> markers = new List<FirePointMarker>();
	private List<BulletLineMarker> lineMarkers = new List<BulletLineMarker>();

	private int currShotPoint = -1;

	GraphicRaycaster gp;
	PointerEventData ped;

	public void Start()
	{
		instance = this;

		gp = canvas.GetComponent<GraphicRaycaster>();
        ped=  new PointerEventData(null);

		int count = database.data.Count;
		for(int i = 0; i < count; ++i)
		{
			enemyList.options.Add(new Dropdown.OptionData(database.data[i].name));
		}

		if(enemyList.options.Count != 0)
		{
			SetCurrInfo(0);
		}

		CreateFrames();
		FirePointUpdate();
	}

	public List<RaycastResult> UiRaycast()
	{
		ped.position = Input.mousePosition; 
        List<RaycastResult> results = new List<RaycastResult>();
        gp.Raycast(ped, results);

    	return results;
	}

	public void Update()
	{
		if(Input.GetMouseButtonUp(0))
		{
			List<RaycastResult> hit = UiRaycast();
    	    if (hit.Count != 0) 
    	    { 
    	        GameObject obj = hit[0].gameObject; 
        	    if (obj.CompareTag("FrameObject"))
            	{
                	frameCode = Pick(obj.GetComponent<EnemyFrame>());
            	}
        	}
			else
			{
				// currFrame = -1;
				// slide.ReleasePick();
			}
		}

		if(frameCode != -1)
		{
			if(Input.GetKeyDown(KeyCode.LeftArrow))
			{
				if(frameCode == 0)
					frameCode = 12;
				Pick(frames[--frameCode]);
			}
			else if(Input.GetKeyDown(KeyCode.RightArrow))
			{
				if(frameCode == 11)
					frameCode = -1;

				Pick(frames[++frameCode]);
			}
		}
	}

	public void EnemyInfoSelect(int code)
	{
		SetCurrInfo(code);
		DeleteAllMarker();
		FirePointUpdate();
	}

	public void DropdownSelect()
	{
		if(database.data[enemyList.value] == currInfo)
			return;
		EnemyInfoSelect(enemyList.value);
	}

	public void SetInGameSprite()
	{
		enemy.sprite = currInfo.sprite;
	}

	public void SetCurrInfo(EnemyDatabase.EnemyInfo i)
	{
		currInfo = i;
		SetInGameSprite();
		InfoUiUpdate();
		FrameSync();
		DeleteAllLineMarker();
		Pick(currFrame);
	}

	public void SetCurrInfo(int code)
	{
		currInfo = database.data[code];
		SetInGameSprite();
		InfoUiUpdate();
		FrameSync();
		DeleteAllLineMarker();
		frameCode = Pick(currFrame);
	}

	public void ShotPointUiUpdate(int code, Vector2 pos)
	{
		SetCurrShotPoint(code);

		shotPointUi[0].text = "Code : " + code;
		shotPointUi[1].text = pos.ToString();
	}

	public void SetCurrShotPoint(int c)
	{
		currShotPoint = c;
	}
	
	public void InfoUiUpdate()
	{
		nameUi.text = "Name : " + currInfo.name;
		hpUi.text = "hp : " + currInfo.hp.ToString();
		fpsUi.text = "fps : " + currInfo.fps.ToString();
		firerateUi.text = "patternRate : " + currInfo.patternRate.ToString();
	}

	public void CreateFrames()
	{
		for(int i = 0; i < 12; ++i)
		{
			GameObject tp = Instantiate(frameOrigin,Vector3.zero,Quaternion.identity) as GameObject;
			tp.name = i.ToString();
			tp.transform.SetParent(frameContainer);
			tp.transform.localPosition = new Vector3(-110f + i * 20f,0f,0f);
			frames.Add(tp.GetComponent<EnemyFrame>().Init(currInfo.bullet[i]));
		}
	}

	public void FrameSync()
	{
		int count = frames.Count;
		for(int i = 0; i < count; ++i)
		{
			frames[i].SetFrame(currInfo.bullet[i]);
		}
	}

	public void FrameColorUpdate()
	{
		int count = frames.Count;
		for(int i = 0; i < count; ++i)
		{
			frames[i].SetColor();
		}
	}

	public int Pick(EnemyFrame f)
	{
		if(currFrame != null)
			currFrame.ReleasePick();
		if(currFrame == f)
		{
			currFrame = null;
			DeleteAllLineMarker();
			return -1;
		}

		currFrame = f;
		currFrame.Pick();

		frameCode = int.Parse(currFrame.name);
		CreateLineMarker();

		return frameCode;
	}

	public void FirePointUpdate()
	{
		int count = currInfo.shotPoint.Count;
		for(int i = 0; i < count; ++i)
		{
			CreateFirePoint(i);
		}
	}

	public void AddMarker(FirePointMarker obj)
	{
		markers.Add(obj);
	}

	public void DeleteAllMarker()
	{
		int count = markers.Count;
		for(int i = 0; i < count; ++i)
		{
			Destroy(markers[i].gameObject);
		}
		markers.Clear();
	}

	public void CreateFirePoint()
	{
		int count = currInfo.shotPoint.Count;
		currInfo.shotPoint.Add(Vector2.zero);
		FirePointMarker marker = Instantiate(shotPointOrigin,Vector2.zero,Quaternion.identity).GetComponent<FirePointMarker>();
		marker.transform.SetParent(inGameObjects);
		marker.Init(count,currInfo);

		AddMarker(marker);
	}

	public void CreateFirePoint(int code)
	{
		FirePointMarker marker = Instantiate(shotPointOrigin,Vector2.zero,Quaternion.identity).GetComponent<FirePointMarker>();
		marker.transform.SetParent(inGameObjects);
		marker.Init(code,currInfo);
		marker.PositionUpdate();

		AddMarker(marker);
	}

	public void DeleteAllLineMarker()
	{
		int count = lineMarkers.Count;
		for(int i = 0; i < count; ++i)
		{
			lineMarkers[i].Delete();
		}
		lineMarkers.Clear();

		
	}

	public void DeleteBulletLine()
	{
		if(currMarker == null)
		{
			Debug.Log("no marker");
			return;
		}

		currInfo.bullet[currMarker.frame].bulletInfo.RemoveAt(currMarker.code);
		lineMarkers.Remove(currMarker);

		for(int i = 0; i < lineMarkers.Count; ++i)
		{
			if(lineMarkers[i].code > currMarker.code)
				lineMarkers[i].code--;
		}

		currMarker.Delete();
	}

	public void DeleteShotPoint()
	{
		DeleteAllLineMarker();

		if(currPoint == null)
			return;

		int count = frames.Count;
		for(int i = 0; i < count; ++i)
		{
			EnemyDatabase.BulletFrameInfo info = currInfo.bullet[i];
			if(info.bulletInfo.Count != 0)
			{
				//int count = info.bulletInfo.Count;
				for(int j = 0; j < info.bulletInfo.Count;)
				{
					if(info.bulletInfo[j].shotPoint == currShotPoint)
					{
						info.bulletInfo.RemoveAt(j);
					}
					else if(info.bulletInfo[j].shotPoint > currShotPoint)
					{
						info.bulletInfo[j].shotPoint--;
						++j;
					}
					else
						 ++j;
				}
			}
		}

		FirePointMarker save = currPoint;

		currInfo.shotPoint.RemoveAt(currShotPoint);
		markers.RemoveAt(currShotPoint);

		for(int i = 0; i < markers.Count; ++i)
		{
			if(markers[i].code != i)
				markers[i].code = i;
		}
		save.Delete();

		if(currFrame != null)		
			CreateLineMarker();

		FrameColorUpdate();
	}

	public void AddPattern()
	{
		if(currFrame == null)
		{
			Debug.Log("no frame");
			return;
		}
		else if(currPoint == null)
		{
			Debug.Log("no point");
			return;
		}

		EnemyDatabase.BulletInfo bi = new EnemyDatabase.BulletInfo(currShotPoint,10,0);

		currFrame.frame.bulletInfo.Add(bi);
		CreateLineMarker(bi.shotPoint,currFrame.frame.bulletInfo.Count - 1,bi.angle);
	}

	public void CreateLineMarker(bool delete = true)
	{
		if(delete)
			DeleteAllLineMarker();
		int count = currFrame.frame.bulletInfo.Count;
		for(int i = 0; i < count; ++i)
		{
			CreateLineMarker(currFrame.frame.bulletInfo[i].shotPoint,i,currFrame.frame.bulletInfo[i].angle);
		}
	}

	public void CreateLineMarker(int shotCode,int c,float angle)
	{
		BulletLineMarker mk = Instantiate(LineMarkerOrigin,Vector3.zero,Quaternion.identity).GetComponent<BulletLineMarker>();
		if(mk == null)
			Debug.Log("chjec");
		mk.Init(c,frameCode,angle);

		mk.transform.SetParent(markers[shotCode].transform);
		mk.transform.localPosition = Vector3.zero;
		lineMarkers.Add(mk);
	}

	public void CreateBullet(EnemyDatabase.BulletInfo bulletInfo)
	{
		BulletScript bs = Instantiate(bulletOrigin,currInfo.shotPoint[bulletInfo.shotPoint],Quaternion.identity).GetComponent<BulletScript>();
		bs.Init(bulletInfo.angle,bulletInfo.speed);
	}

	public void TestStart()
	{
		if(currFrame != null)
		{
			currFrame.ReleasePick();
			currFrame = null;
			DeleteAllLineMarker();
		}
		StartCoroutine(Progress(0,1f / currInfo.fps));
	}

	public IEnumerator Progress(int start,float time)
	{
		int i = start;
		int count = frames.Count;
		//inGameObjects.gameObject.SetActive(false);
		while(true)
		{
			frames[i].Progress();
			yield return new WaitForSeconds(time);
			frames[i++].SetColor();
			if(i == count)
				break;
		}
		//inGameObjects.gameObject.SetActive(true);
		//running = false;
	}
}
