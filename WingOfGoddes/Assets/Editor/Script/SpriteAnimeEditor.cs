using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.AnimatedValues;
using UnityEngine.Events;

public class Frame
{
    public static Frame select = null;

    public Rect area;
    public Define.SpriteInfo frameInfo = null;


    Color color = Color.white;
    string contain = "";

    public void Draw()
    {
        Color old = GUI.color;

        GUI.color = color;
        GUI.Box(area,"");

        GUI.color = old;

        GUI.Label(area,contain);

        GUILayout.Space(area.width);
    }

    public bool FrameCheck(EditorWindow edit,ref Define.SpriteInfo dragInfo)
    {
        Event ev = Event.current;

        if(ev.type == EventType.MouseDown)
        {
            if(area.Contains(ev.mousePosition))
            {
                if(select == this)
                {
                    select = null;
                }
                else
                {
                    select = this;
                }

                edit.Repaint();
            }
        }
        if(dragInfo != null)
        {
            if(area.Contains(ev.mousePosition))
            {
                if(ev.type == EventType.mouseUp)
                {
                    frameInfo = dragInfo;
                    dragInfo = null;

                    ContainCheck();
                    edit.Repaint();
                }
            }
        }

        ColorCheck();

        return select == this;
    }

    public void ColorCheck()
    {
        if(select == this)
        {
            color = Color.green;
        }
        else
        {
            color = Color.white;
        }
    }

    public void ContainCheck()
    {
        if(frameInfo == null)
        {
            contain = "";
        }
        else
            contain = "●";

    }

    public Frame() {}
    public Frame(Rect rt) {area = rt;}
    public Frame(Rect rt, Define.SpriteInfo frame) {area = rt; frameInfo = frame; ContainCheck();}
}

public class SpriteObject
{
    public static SpriteObject select = null;

    public Rect area;
    public Define.SpriteInfo info = null;
    public string name;

    bool thisFrame = false;

    Color color = Color.white;

    public void Draw(float space)
    {
        Color old = GUI.color;

        GUI.color = color;
        GUI.Box(area,"");

        GUI.color = old;

        GUI.Label(area,name);

        GUILayout.Space(area.height + space);
    }

    public void ObjectCheck(EditorWindow edit,ref Define.SpriteInfo dragInfo)
    {
        Event ev = Event.current;

        if(ev.type == EventType.MouseDown)
        {
            if(area.Contains(ev.mousePosition))
            {
                select = this;

                edit.Repaint();
            }
        }
        else if(ev.type == EventType.mouseUp)
        {
            if(area.Contains(ev.mousePosition))
            {
                if(select == this)
                {
                    if(thisFrame)
                    {
                        thisFrame = false;

                        select = null;

                        edit.Repaint();
                    }
                    else
                    {
                        thisFrame = true;
                    }
                }
            }
        }

        if(select != this)
        {
            thisFrame = false;
        }


        if(dragInfo == null)
        {
            if(area.Contains(ev.mousePosition))
            {
                if(ev.type == EventType.MouseDrag)
                {
                    dragInfo = info;
                }
            }
        }

        ColorCheck();
    }

    public void ColorCheck()
    {
        if(select == this)
        {
            color = Color.green;
        }
        else
        {
            color = Color.white;
        }
    }

    public SpriteObject(){}
    public SpriteObject(Rect rect) {area = rect;}

    public SpriteObject(Rect rect, Define.SpriteInfo i, string n)
    {
        area = rect;
        info = i;
        name = n;
    }
}

public class SpriteAnimeEditor : EditorWindow
{
    public int frameSec = 0;

    public SpriteDatabase spriteDatabase;

    public Frame frame;

    [MenuItem("Window/SpriteAnimeEditor")]
    static void Open ()
    {
        GetWindow<SpriteAnimeEditor>();
    }

    string[] spriteGroup;
    string[] spriteSet;
    int spriteGroupSelect = 0;
    int prevSpriteGroupSelect = 0;
    int spriteSetSelect = 0;
    int prevSpriteSetSelect = 0;

    Vector2 frameScroll = new Vector2();
    Vector2 spriteScroll = new Vector2();

    List<Frame> frameList = new List<Frame>();
    List<SpriteObject> spriteList = new List<SpriteObject>();

    AnimationCurve curve = AnimationCurve.Linear(0f,1f,1f,1f);

    Define.SpriteInfo dragInfo = null;

    string saveName = "";

    float frameX = 4f;
    float frameY = 30f;
    float frameWidth = 12f;
    float frameHeight = 35f;

    float spriteObjectX = 15f;
    float spriteObjectWidth = 157f;
    float spriteObjectHeight = 17f;

    float framePerSec = 12f;

    int currFrame = -1;

    int rangeMin = 0;
    int rangeMax = 1;

    bool mouseUp = false;
    bool loop = false;

    void OnEnable()
    {
        spriteDatabase = AssetDatabase.LoadAssetAtPath("Assets/Database/SpriteDatabase.asset", typeof(SpriteDatabase)) as SpriteDatabase;

        GetSpriteGroup();
        GetSpriteSelect();

        GetSprites();
    }

    void OnGUI() 
    {
        if(Event.current.type == EventType.MouseUp)
        {
            mouseUp = true;
        }

        Rect frameArea = new Rect(165f,10f,position.width - 370f,position.height - 25f);
        Rect boxArea = frameArea;

        boxArea.height += 5f;
        GUI.Box(boxArea,"");

        GUILayout.BeginArea(frameArea);
        frameScroll = GUILayout.BeginScrollView(frameScroll);

        GUILayout.BeginHorizontal();

        for(int i = 0; i < frameList.Count; ++i)
        {
            if(frameList[i].FrameCheck(this,ref dragInfo))
            {
                currFrame = i;
            }

            frameList[i].Draw();

            if(i % 5 == 0)
            {
                Rect r = frameList[i].area;

                DrawLine(new Vector3(5f + i * frameWidth,28f,0f),new Vector3(5f + i * frameWidth,15f,0f));

                r.y -= 24f;
                r.width = 20f;
                GUI.Label(r,i.ToString());
            }
            else
            {
                DrawLine(new Vector3(5f + i * frameWidth,28f,0f),new Vector3(5f + i * frameWidth,23f,0f));
            }
        }

        if(Frame.select == null)
        {
            currFrame = -1;
        }

        if(mouseUp)
        {
            if(dragInfo != null)
            {
                dragInfo = null;
            }
        }

        DrawLine(new Vector3(5f,28f,0f), new Vector3(5f + frameList.Count * frameWidth,28f,0f));

        GUILayout.EndHorizontal();

        GUILayout.Space(frameHeight + 30f);

        if(frameList.Count != 0)
        {
            curve = EditorGUILayout.CurveField(curve,Color.green,new Rect(0,0,1f,5f),
                new GUILayoutOption[]
                {
                    GUILayout.Width(frameList.Count * 12f),
                    GUILayout.Height(position.height - 155f),
                });
        }

        GUILayout.EndScrollView();

        if(GUILayout.Button("AddFrame"))
        {
            AddFrame();
        }
        if(GUILayout.Button("DeleteFrame"))
        {
            DeleteFrame(currFrame);
        }
        GUILayout.EndArea();

        Rect spriteArea = new Rect(position.width - 200f,10f,195f,position.height - 20f);

        GUILayout.BeginArea(spriteArea);

        spriteGroupSelect = EditorGUILayout.Popup(spriteGroupSelect, spriteGroup);
        if(prevSpriteGroupSelect != spriteGroupSelect)
        {
            prevSpriteGroupSelect = spriteGroupSelect;
            GetSpriteSelect();
            GetSprites();
        }

        if(spriteSet != null)
        {
            spriteSetSelect = EditorGUILayout.Popup(spriteSetSelect,spriteSet);
            if(prevSpriteSetSelect != spriteSetSelect)
            {
                prevSpriteSetSelect = spriteSetSelect;
                GetSprites();
            }
        }

        GUILayout.BeginVertical("box");

        spriteScroll = GUILayout.BeginScrollView(spriteScroll,false,true);

        if(spriteList.Count != 0)
        {
            for(int i = 0; i < spriteList.Count; ++i)
            {
                Rect rect = spriteList[i].area;
                rect.x = 0f;

                spriteList[i].ObjectCheck(this,ref dragInfo);
                GUI.Label(rect,i.ToString() + ".");
                spriteList[i].Draw(2f);
            }
        }

        GUILayout.EndScrollView();


        GUILayout.EndVertical();

        GUILayout.BeginHorizontal();

        GUILayout.Label("Range");
        rangeMin = EditorGUILayout.IntField(rangeMin);
        GUILayout.Label("~");
        rangeMax = EditorGUILayout.IntField(rangeMax);

        GUILayout.EndHorizontal();

        if(GUILayout.Button("Add to Range"))
        {
            AddToRange();
        }
        if(GUILayout.Button("Add All"))
        {
            AddAll();
        }

        GUILayout.EndArea();

        // curve = EditorGUILayout.CurveField(curve,Color.green,new Rect(0,0,1f,5f),GUILayout.Height(100f));

        // if(GUILayout.Button("Test"))
        // {
        //     Debug.Log(curve.Evaluate(0.001f));
        //     Debug.Log(curve.Evaluate(1f));
        // }

        // GUI.color = Color.red;
        // GUI.Box(new Rect(0,0,100,100),"check");

        InformationDraw();


        SpeedSync();
        mouseUp = false;
    }

    public void InformationDraw()
    {
        Rect infoArea = new Rect(5f,10f,155f,position.height - 20f);

        GUILayout.BeginArea(infoArea);
        GUILayout.BeginHorizontal();

        GUILayout.Button("▶",GUILayout.Width(20f));
        GUILayout.Button("▣",GUILayout.Width(20f));
        GUILayout.Label("currFrame : " + currFrame.ToString());

        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();

        GUILayout.Label("fps");
        framePerSec = EditorGUILayout.FloatField(framePerSec);
        
        GUILayout.EndHorizontal();


        GUILayout.BeginHorizontal();

        GUILayout.Label("loop");
        loop = EditorGUILayout.Toggle(loop);

        GUILayout.EndHorizontal();

        if(Frame.select != null)
        {
            Define.SpriteInfo info = Frame.select.frameInfo;

            if(info != null)
            {
                GUI.Label(new Rect(3f,55f,150f,25f),"sprite : " + spriteList[info.index].name);
            }
        }

        GUILayout.Space(25f);

        SaveButton();

        LoadButton();

        GUILayout.EndArea();
    }

    public void SaveButton()
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label("fileName : ");
        saveName = EditorGUILayout.TextField(saveName);

        GUILayout.EndHorizontal();

        if(GUILayout.Button("Save"))
        {
            if(saveName == "")
            {
                EditorUtility.DisplayDialog("Error", "fileName is null", "ok");
                return;
            }
            List<Define.SpriteInfo> info = new List<Define.SpriteInfo>();
            for(int i = 0; i < frameList.Count; ++i)
            {
                if(frameList[i].frameInfo != null)
                {
                    frameList[i].frameInfo.frame = i;
                    info.Add(frameList[i].frameInfo);
                }
            }

            if(info.Count == 0)
            {
                EditorUtility.DisplayDialog("Error", "frameList is null", "ok");
                return;
            }

            SpriteAnimeInfo.Create(info,curve,framePerSec,saveName,loop);

            saveName = "";
        }
    }

    public void LoadButton()
    {
        UnityEngine.Object[] objects = DropAreaGUI();

        if(objects != null)
        {
            SpriteAnimeInfo info = objects[0] as SpriteAnimeInfo;
            if(info.info.Count != 0)
            {
                ClearFrameList();

                spriteGroupSelect = info.info[0].group;
                spriteSetSelect = info.info[0].set;

                framePerSec = info.fps;
                loop = info.loop;

                for(int i = 0; i < info.info.Count; ++i)
                {
                    AddFrame(info.info[i]);
                }

                curve = info.curveInfo;
                saveName = info.name;
            }
            else
            {
                EditorUtility.DisplayDialog("Error", "why save this file. this file is empty", "ok");
            }
        }
    }

    UnityEngine.Object[] DropAreaGUI()
    {
        Event evt = Event.current;
        Rect drop_area = GUILayoutUtility.GetRect(0.0f, 20.0f, GUILayout.ExpandWidth(true));
        GUI.Box(drop_area, "Anime Drop");

        switch (evt.type)
        {
            case EventType.DragUpdated:
            case EventType.DragPerform:
                if (!drop_area.Contains(evt.mousePosition))
                    return null;

                if(DragAndDrop.objectReferences[0].GetType() != typeof(SpriteAnimeInfo))
                    return null;

                DragAndDrop.visualMode = DragAndDropVisualMode.Copy;

                if (evt.type == EventType.DragPerform)
                {
                    DragAndDrop.AcceptDrag();
                    return DragAndDrop.objectReferences;
                }
                break;
        }
        return null;
    }

    public void GetSpriteGroup()
    {
        spriteGroup = spriteDatabase.GetKeys();
    }

    public void GetSpriteSelect()
    {
        spriteSet = spriteDatabase.GetNames(spriteGroupSelect);
    }

    public void GetSprites()
    {
        spriteList.Clear();
        
        Sprite[] sprites = spriteDatabase.GetSprites(spriteGroupSelect,spriteSetSelect);

        if(sprites == null)
        {
            return;
        }

        for(int i = 0; i < sprites.Length; ++i)
        {
            Define.SpriteInfo info = null;
            if(sprites[i] != null)
            {
                info = new Define.SpriteInfo();
                info.group = spriteGroupSelect;
                info.set = spriteSetSelect;
                info.index = i;
            }

            spriteList.Add(new SpriteObject(new Rect(spriteObjectX,i * (spriteObjectHeight + 2f)
                                            ,spriteObjectWidth,spriteObjectHeight)
                                            ,info,sprites[i] == null ? "null" : sprites[i].name));
        }
    }

    public void GetSelectedSpriteGroup()
    {

    }

    public void DrawLine(Vector3 one, Vector3 two)
    {
        Color old = GUI.color;

        Handles.color = Color.black;
        Handles.DrawLine(one, two);

        GUI.color = old;
    }

    public void AddFrame()
    {
        frameList.Add(new Frame(new Rect(12f * frameList.Count + frameX,frameY,frameWidth,frameHeight)));
    }

    public void AddFrame(Define.SpriteInfo info)
    {
        frameList.Add(new Frame(new Rect(12f * frameList.Count + frameX,frameY,frameWidth,frameHeight),info));        
    }

    public void AddToRange()
    {
        if(rangeMin > rangeMax)
        {
            EditorUtility.DisplayDialog("Error", "Min Value is Higher Than Max Value", "ok");
            return;
        }

        if(rangeMin >= spriteList.Count || rangeMax >= spriteList.Count || rangeMin < 0 || rangeMax < 0)
        {
            EditorUtility.DisplayDialog("Error", "Out of Range", "ok");
            return;
        }

        ClearFrameList();

        for(int i = rangeMin; i <= rangeMax; ++i)
        {
            AddFrame(spriteList[i].info);
        }

    }

    public void AddAll()
    {
        if(spriteList.Count < 0)
        {
            EditorUtility.DisplayDialog("Error", "List is null", "ok");
            return;
        }

        ClearFrameList();

        for(int i = 0; i < spriteList.Count; ++i)
        {
            AddFrame(spriteList[i].info);
        }
    }

    public void ClearFrameList()
    {
        frameList.Clear();
        Frame.select = null;
    }

    public void DeleteFrame(int index)
    {
        if(Frame.select == null)
        {
            EditorUtility.DisplayDialog("Error", "Selected Frame does not Exist", "ok");
            return;
        }

        if(frameList.Count > index)
        {
            if(index >= 0)
            {
                frameList.RemoveAt(index);
                Frame.select = null;

                for(int i = index; i < frameList.Count; ++i)
                {
                    frameList[i].area.x = 12f * i + frameX;

                }
            }
            else
            {
                EditorUtility.DisplayDialog("Error", "Frame does not Exists", "ok");
            }
            
        }
        else
        {
            EditorUtility.DisplayDialog("Error", "Frame does not Exists", "ok");
        }
    }

    public void SpeedSync()
    {
        float time = 1f / frameList.Count;
        for(int i = 0; i < frameList.Count; ++i)
        {
            if(frameList[i].frameInfo != null)
            {
                frameList[i].frameInfo.speed = framePerSec * curve.Evaluate(time * (float)i);
            }
        }
    }
}