using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class PatternEditor : EditorBase {
    
    public class Frame
    {
        public static Frame select;
        public int frame;

        public Image frameImage;

        public List<DirectionMarker> markers = new List<DirectionMarker>();

        public Define.RectF bounds;

        public Define.RectF TpToRect(RectTransform rect)
        {
            return MathEx.RectTransformNormalToRect(rect,new Define.RectF());
        }

        public bool MouseCheck(Vector2 pos)
        {
            return MathEx.PointInRect(pos,bounds);
        }

        public void SetSprite(Sprite spr)
        {
            frameImage.sprite = spr;
        }

        public void SetColor(Color color)
        {
            frameImage.color = color;
        }

        public void MarkerProgress()
        {
            for(int i = 0; i < markers.Count; ++i)
            {
                markers[i].Progress(null);
            }
        }

        public void MarkerActive()
        {
            for(int i = 0; i < markers.Count; ++i)
            {
                markers[i].gameObject.SetActive(true);
            }
        }

        public void MarkerDisable()
        {
            for(int i = 0; i < markers.Count; ++i)
            {
                markers[i].gameObject.SetActive(false);
            }
        }

        public void Set(){}

        public Frame(){}
        public Frame(RectTransform rect, Image img)
        {
            bounds = TpToRect(rect);
            frameImage = img;
        }
    }

    public RectTransform frameContainor;
    public GameObject frameBase;
    public GameObject markerBase;
    public GameObject spikeMarkerBase;

    public Dropdown patternList;
    public InputField newPatternName;

    public Sprite[] frameSprite;

    private PatternDatabase patternDatabase;
    private PatternPresetDatabase presetDatabase;
    private List<Frame> frameList = new List<Frame>();

    private bool scriptCheck = false;

    public void Start()
    {
        patternDatabase = AssetDatabase.LoadAssetAtPath("Assets/Database/PatternDatabase.asset", typeof(PatternDatabase)) as PatternDatabase; 
        presetDatabase = AssetDatabase.LoadAssetAtPath("Assets/Database/PatternPresetDatabase.asset", typeof(PatternPresetDatabase)) as PatternPresetDatabase;

        PatternListSync();
        FrameSync();
    }

    public void Update()
    {
        if(Frame.select != null)
        {
            if(Input.GetKeyDown(KeyCode.LeftArrow))
            {
                if(Frame.select.frame == 0)
                {
                    FrameSelect(frameList.Count - 1);
                }
                else
                    FrameSelect(Frame.select.frame - 1);
            }
            else if(Input.GetKeyDown(KeyCode.RightArrow))
            {
                if(Frame.select.frame == frameList.Count - 1)
                {
                    FrameSelect(0);
                }
                else
                    FrameSelect(Frame.select.frame + 1);
            }
        }


        FrameProgress();
        
        if(Input.GetMouseButtonDown(0))
        {
            FrameSelectCheck();
        }
    }

    public void AddPatternPreset()
    {
        if(Frame.select == null)
        {
            PopupWindow.instance.Active("Frame is null",Color.white);
            return;
        }

        PatternFrameInfo.Preset pre = new PatternFrameInfo.Preset();

        patternDatabase.AddPatternPreset(patternList.value,Frame.select.frame, pre);
        CreateMarker(pre,Frame.select);

        Frame.select.MarkerActive();
        Frame.select.SetSprite(frameSprite[1]);
    }

    public void PatternSync()
    {
        //if(!scriptCheck)
        Frame.select = null;
        FrameSync();
    }

    public void AddFrame()
    {
        CreateFrame();
    }

    public void AddNewPattern()
    {
        if(!patternDatabase.NameOverlapCheck(newPatternName.text))
        {
            PopupWindow.instance.Active("Pattern already Exists",Color.white);
            return;
        }
        else if(newPatternName.text == "")
        {
            PopupWindow.instance.Active("Name Is Empty",Color.white);
            return;
        }

        patternDatabase.AddPattern(newPatternName.text);
        AddPatternListItem(newPatternName.text);
        patternList.value = patternList.options.Count - 1;
        patternList.captionText.text = newPatternName.text;

        newPatternName.text = "";
    }

    public void AddPatternListItem(string n)
    {
        patternList.options.Add(new Dropdown.OptionData(n));
    }

    public void PatternListSync()
    {
        string[] s = patternDatabase.GetNames();
        patternList.ClearOptions();

        patternList.value = 0;

        if(s == null)
        {
            PopupWindow.instance.Active("Pattern does not Exists",Color.white);
            return;
        }

        for(int i = 0; i < s.Length; ++i)
        {
            AddPatternListItem(s[i]);
        }

        patternList.captionText.text = s[patternList.value];
    }

    public void FrameProgress()
    {
        if(Frame.select != null)
        {
            Frame.select.MarkerProgress();
        }
    }

    public void FrameSelectCheck()
    {
        Vector2 point = Input.mousePosition;

        for(int i = 0; i < frameList.Count; ++i)
        {
            if(frameList[i].MouseCheck(point))
            {
                FrameSelect(i);
                return;
            }
        }
    }

    public void SelectFrameRelease()
    {
        if(Frame.select != null)
        {
            Frame.select.MarkerDisable();
            Frame.select.SetColor(Color.white);
            Frame.select = null;
        }
    }

    public void FrameSelect(int index)
    {
        if(Frame.select != null)
        {
            Frame.select.MarkerDisable();

            if(Frame.select == frameList[index])
            {
                Frame.select = null;
                frameList[index].SetColor(Color.white);

        //        EventListSync();

                return;
            }
            else
            {
                Frame.select.SetColor(Color.white);
            }
        }

        Frame.select = frameList[index];
        frameList[index].SetColor(Color.green);

    //    EventListSync();

        Frame.select.MarkerActive();
    }

    public void FrameSync()
    {
        DeleteAllFrames();

        if(patternDatabase.ExistsCheck(patternList.value))
        {
            CreateFrames();
        }
    }

    public void CreateFrames()
    {
        if(!patternDatabase.ExistsCheck(patternList.value))
        {
            PopupWindow.instance.Active("Pattern does not Exists",Color.white);
            return;
        }

        PatternDatabase.PatternInfo info = patternDatabase.data[patternList.value];

        if(info.frames.Count == 0)
            return;

        int frame = info.frames[info.frames.Count - 1].frame;
        int count = 0;

        for(int i = 0; i <= frame; ++i)
        {
            Frame f = CreateFrame();
        
            if(info.frames[count].frame == i)
            {
                
                for(int j = 0; j < info.frames[count].presets.Count; ++j)
                {
                    CreateMarker(info.frames[count].presets[j],f);
                }

                f.SetSprite(frameSprite[1]);

                ++count;
            }
        }
    }

    public void CreateMarker(PatternFrameInfo.Preset info, Frame f)
    {
        DirectionMarker marker = Instantiate(markerBase).GetComponent<DirectionMarker>();
        f.markers.Add(marker);

        marker.transform.SetParent(this.transform);
        marker.info = info;

        for(int i = 0; i < presetDatabase.data[info.preset].shots.Count; ++i)
        {
            Transform tp = Instantiate(spikeMarkerBase).transform;
            tp.SetParent(marker.transform);
            tp.rotation = Quaternion.Euler(0f,0f,presetDatabase.data[info.preset].shots[i].angle);
        }

        marker.transform.rotation = Quaternion.Euler(0f,0f,info.extraAngle);
        marker.gameObject.SetActive(false);
    }

    public Frame CreateFrame()
    {
        GameObject obj = Instantiate(frameBase) as GameObject;

        obj.transform.SetParent(frameContainor);
        obj.transform.localPosition = new Vector3(frameList.Count * 19f - frameContainor.sizeDelta.x / 2f + 30f,0f ,0f);

        Frame f = new Frame(obj.GetComponent<RectTransform>(),obj.GetComponent<Image>());
        f.frame = frameList.Count;

        frameList.Add(f);

        return f;
    }

    public void DeleteAllFrames()
    {
        for(int i = 0; i < frameList.Count; ++i)
        {
            for(int j = 0; j < frameList[i].markers.Count; ++j)
            {
                Destroy(frameList[i].markers[j].gameObject);
            }

            Destroy(frameList[i].frameImage.gameObject);
        }

        frameList.Clear();
        DirectionMarker.select = null;
    }
}