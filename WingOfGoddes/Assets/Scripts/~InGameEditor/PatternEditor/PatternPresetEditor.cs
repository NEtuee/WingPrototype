using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class PatternPresetEditor : EditorBase {

	public GameObject markerBase;

    public Dropdown bulletGroupUI;
    public Dropdown bulletListUI;

    public Image bulletPreview;

    public InputField speedUI;
    public InputField attackUI;
    public InputField angleUI;
    public InputField lifeTimeUI;

    public InputField angleAccelUI;
    public InputField speedAccelUI;

    public Toggle penetrateUI;
    public Toggle rotationLockUI;
    public Toggle guidedUI;

    public Button cloneButton;
    public Button deleteButton;

    public InputField newPresetName;

    public Dropdown presetList;

    public InputField presetName;

    private List<ShotInfoMarker> markerList = new List<ShotInfoMarker>();
    private PatternPresetDatabase presetDatabase;
    private BulletDatabase bulletDatabase;
    private SpriteDatabase spriteDatabase;

    private bool scriptChange = false;

	public void Awake()
    {
        presetDatabase = AssetDatabase.LoadAssetAtPath("Assets/Database/PatternPresetDatabase.asset", typeof(PatternPresetDatabase)) as PatternPresetDatabase;
        bulletDatabase = AssetDatabase.LoadAssetAtPath("Assets/Database/BulletDatabase.asset", typeof(BulletDatabase)) as BulletDatabase;
        spriteDatabase = AssetDatabase.LoadAssetAtPath("Assets/Database/SpriteDatabase.asset", typeof(SpriteDatabase)) as SpriteDatabase;

        PresetListSync();
        PresetSync();

        GroupListSync();

        UIActive(false);
    }

    public void Update()
    {
        MarkerProgress();
    }

    public void SpriteInfoValueSync()
    {
        if(!SelectMarkerExistsCheck() || scriptChange)
        {
            return;
        }

        GetCurrInfo().bulletGroup = bulletGroupUI.value;
        GetCurrInfo().bulletIndex = bulletListUI.value;
    }

    public void SpritePreviewSync()
    {
        SpriteDatabase.SpriteIndexInfo info = bulletDatabase.GetSpriteInfo(bulletGroupUI.value,bulletListUI.value);

        if(info != null)
        {
            bulletPreview.sprite = spriteDatabase.GetSprite(info);
        }
    }

    public void BulletListSync()
    {
        Dropdown.OptionData[] data = bulletDatabase.GetListNames(bulletGroupUI.value);
        bulletListUI.ClearOptions();
        if(data != null)
        {
            for(int i = 0; i < data.Length; ++i)
            {
                bulletListUI.options.Add(data[i]);
            }

            bulletListUI.captionText.text = bulletListUI.options[0].text;
        }

        //bulletListUI.value = 0;
        SpritePreviewSync();
    }

    public void GroupListSync()
    {
        Dropdown.OptionData[] data = bulletDatabase.GetGroupNames();
        bulletGroupUI.ClearOptions();

        if(data != null)
        {
            for(int i = 0; i < data.Length; ++i)
            {
                bulletGroupUI.options.Add(data[i]);
            }

            bulletGroupUI.captionText.text = bulletGroupUI.options[0].text;
        }
        BulletListSync();
    }

    public void AddNewPreset()
    {
        if(!presetDatabase.NameOverlapCheck(newPresetName.text))
        {
            Debug.Log("name aerer");
            return;
        }
        if(newPresetName.text == "")
        {
            Debug.Log("emppthy");
            return;
        }

        presetDatabase.AddPatternPreset(newPresetName.text);
        presetName.text = "";

        PresetListSync();

        presetList.value = presetList.options.Count - 1;

        PresetSync();
    }

    public void AddShotInfo()
    {
        ShotInfo info = new ShotInfo();
        info.bulletGroup = 0;
        info.bulletIndex = 0;

        presetDatabase.AddShotInfo(presetList.value,info);
        CreateMarker(info);
    }

    public void AddCloneInfo()
    {
        ShotInfo info = CloneShotInfo();
        presetDatabase.AddShotInfo(presetList.value,info);
        CreateMarker(info);
    }

    public ShotInfo CloneShotInfo()
    {
        ShotInfo info = new ShotInfo();
        info.bulletGroup = GetCurrInfo().bulletGroup;
        info.bulletIndex = GetCurrInfo().bulletIndex;
        info.speed = float.Parse(speedUI.text);
        info.attack = float.Parse(attackUI.text);
        info.angle = float.Parse(angleUI.text);
        info.lifeTime = float.Parse(lifeTimeUI.text);
        info.angleAccel = float.Parse(angleAccelUI.text);
        info.speedAccel = float.Parse(speedAccelUI.text);
        info.penetrate = penetrateUI.isOn;
        info.rotationLock = rotationLockUI.isOn;
        info.guided = guidedUI.isOn;

        return info;
    }

    public void PresetNameSync()
    {
        if(presetDatabase.ExistsCheck(presetList.value))
        {
            if(presetName.text == "")
            {
                Debug.Log("name is empty");
                return;
            }
            if(presetName.text == presetDatabase.data[presetList.value].name)
                return;
            if(!presetDatabase.NameOverlapCheck(presetName.text))
            {
                Debug.Log("name is already exists");
                presetName.text = presetDatabase.data[presetList.value].name;
                return;
            }

            presetDatabase.data[presetList.value].name = presetName.text;
            presetList.options[presetList.value].text = presetName.text;
            presetList.captionText.text = presetName.text;
        }
    }

    public void PresetSync()
    {
        if(!presetDatabase.ExistsCheck(presetList.value))
        {
            return;
        }

        presetName.text = presetDatabase.data[presetList.value].name;
        DeleteAllMarker();
        CreateMarkers(presetDatabase.data[presetList.value].shots);
    }

    public void DeleteShotInfo()
    {
        DeleteShotInfo(ShotInfoMarker.select.info);
        UIActive(false);
    }

    public void DeleteShotInfo(ShotInfo info)
    {
        for(int i = 0; i < markerList.Count; ++i)
        {
            if(markerList[i].info == info)
            {
                if(presetDatabase.DeleteShotInfo(presetList.value,i))
                {
                    GameObject obj = markerList[i].gameObject;
                    markerList.RemoveAt(i);
                    Destroy(obj);
                }
            }
        }
    }

    public void CreateMarkers(List<ShotInfo> info)
    {
        for(int i = 0; i < info.Count; ++i)
        {
            CreateMarker(info[i]);
        }
    }

    public void CreateMarker(ShotInfo info)
    {
        ShotInfoMarker marker = Instantiate(markerBase).GetComponent<ShotInfoMarker>();
        marker.transform.SetParent(this.transform);
        marker.Set(info);
        marker.ColorCheck(false);
        markerList.Add(marker);
    }

    public void DeleteAllMarker()
    {
        for(int i = 0; i < markerList.Count; ++i)
        {
            Destroy(markerList[i].gameObject);
        }

        markerList.Clear();
    }

    public void PresetListSync()
    {
        string[] names = presetDatabase.GetNames();

        presetList.ClearOptions();

        if(names == null)
            return;

        for(int i = 0; i < names.Length; ++i)
        {
            presetList.options.Add(new Dropdown.OptionData(names[i]));
        }

        presetList.captionText.text = presetList.options[0].text;
    }

#region SyncFunc
    public void SpeedSync()
    {
        if(!SelectMarkerExistsCheck())
        {
            Debug.Log("select is null");
            return;
        }

        if(speedUI.text != "")
            GetCurrInfo().speed = float.Parse(speedUI.text);
        else
            speedUI.text = GetCurrInfo().speed.ToString();
    }

    public void AttackSync()
    {
        if(!SelectMarkerExistsCheck())
        {
            Debug.Log("select is null");
            return;
        }

        if(speedUI.text != "")
            GetCurrInfo().attack = float.Parse(attackUI.text);
        else
            attackUI.text = GetCurrInfo().attack.ToString();
    }

    public void AngleSync()
    {
        if(!SelectMarkerExistsCheck())
        {
            Debug.Log("select is null");
            return;
        }

        if(speedUI.text != "")
        {
            GetCurrInfo().angle = float.Parse(angleUI.text);
            ShotInfoMarker.select.AngleSync();
        }
        else
            angleUI.text = GetCurrInfo().angle.ToString();
    }

    public void LifeTimeSync()
    {
        if(!SelectMarkerExistsCheck())
        {
            Debug.Log("select is null");
            return;
        }

        if(speedUI.text != "")
            GetCurrInfo().lifeTime = float.Parse(lifeTimeUI.text);
        else
            lifeTimeUI.text = GetCurrInfo().lifeTime.ToString();
    }

    public void AngleAccelSync()
    {
        if(!SelectMarkerExistsCheck())
        {
            Debug.Log("select is null");
            return;
        }

        if(speedUI.text != "")
            GetCurrInfo().angleAccel = float.Parse(angleAccelUI.text);
        else
            angleAccelUI.text = GetCurrInfo().angleAccel.ToString();
    }

    public void SpeedAccelSync()
    {
        if(!SelectMarkerExistsCheck())
        {
            Debug.Log("select is null");
            return;
        }

        if(speedUI.text != "")
            GetCurrInfo().speedAccel = float.Parse(speedAccelUI.text);
        else
            speedAccelUI.text = GetCurrInfo().speedAccel.ToString();
    }

    public void PenetrateSync()
    {
        if(!SelectMarkerExistsCheck())
        {
            Debug.Log("select is null");
            return;
        }

        if(speedUI.text != "")
            GetCurrInfo().penetrate = penetrateUI.isOn;
        else
            penetrateUI.isOn = GetCurrInfo().penetrate;
    }

    public void RotationLockSync()
    {
        if(!SelectMarkerExistsCheck())
        {
            Debug.Log("select is null");
            return;
        }

        if(speedUI.text != "")
            GetCurrInfo().rotationLock = rotationLockUI.isOn;
        else
            rotationLockUI.isOn = GetCurrInfo().rotationLock;
    }

    public void GuidedSync()
    {
        if(!SelectMarkerExistsCheck())
        {
            Debug.Log("select is null");
            return;
        }

        if(speedUI.text != "")
            GetCurrInfo().guided = guidedUI.isOn;
        else
            guidedUI.isOn = GetCurrInfo().guided;
    }
#endregion

    public void UIActive(bool value)
    {   
        bulletGroupUI.interactable = value;
        bulletListUI.interactable = value;

        speedUI.interactable = value;
        attackUI.interactable = value;
        angleUI.interactable = value;
        lifeTimeUI.interactable = value;
        angleAccelUI.interactable = value;
        speedAccelUI.interactable = value;
        penetrateUI.interactable = value;
        rotationLockUI.interactable = value;
        guidedUI.interactable = value;

        cloneButton.interactable = value;
        deleteButton.interactable = value;
    }

    public void UISync()
    {
        if(!SelectMarkerExistsCheck())
        {
            Debug.Log("select is null");
            return;
        }

        UIActive(true);

        ShotInfo info = GetCurrInfo();

        scriptChange = true;

        bulletGroupUI.value = info.bulletGroup;
        bulletListUI.value = info.bulletIndex;

        scriptChange = false;

        speedUI.text = info.speed.ToString();
        attackUI.text = info.attack.ToString();
        angleUI.text = info.angle.ToString();
        lifeTimeUI.text = info.lifeTime.ToString();
        angleAccelUI.text = info.angleAccel.ToString();
        speedAccelUI.text = info.speedAccel.ToString();
        penetrateUI.isOn = info.penetrate;
        rotationLockUI.isOn = info.rotationLock;
        guidedUI.isOn = info.guided;
    }

    public bool SelectMarkerExistsCheck()
    {
        if(ShotInfoMarker.select == null)
            return false;
        
        return true;
    }

    public ShotInfo GetCurrInfo()
    {
        return ShotInfoMarker.select.info;
    }

    public void MarkerProgress()
    {
        for(int i = 0; i < markerList.Count; ++i)
        {
            markerList[i].Progress(new MarkerBase.Sync[]{()=> UISync(), ()=> UIActive(false),()=>{angleUI.text = GetCurrInfo().angle.ToString();}});
        }
    }
}
