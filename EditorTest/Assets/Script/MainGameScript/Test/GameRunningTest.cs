using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameRunningTest : ObjectBase {

    public static GameRunningTest instance;

	public float fps = 0f;
    public List<FrameInfo> events = new List<FrameInfo>();

    public Button button;

    public bool testMode = false;

    private float startTime = 1f;
    private bool stageStart = false;
    private bool stageEnd = false;

    private bool waitExtinc = false;
    private bool staticEvent = false;

    public bool dialogActive = false;

    private float gameTime = 0f;
    private float progressTime = 0f;

    private int eventPos = 0;
    private int frameCount = 0;
    private int eventEnd = 0;

    public TextAsset stageData;
    public TextAsset stageScript;

    public override void Initialize()
    {
        instance = this;
        DataManager datam = GetComponent<DataManager>();

        LoadData(stageData.text.Split('\n'));

        InitEvent();
    }

    public void SpawnTestEnemy()
    {
        MobileDebugger.instance.AddLine("테스트용 적 생성");
        EnemyManager.instance.ObjectActive(new Vector3(10f,0f,0f),1,0);
    }

    public void InitEvent()
    {
        MobileDebugger.instance.AddLine("이벤트 초기화");

        button.interactable = false;

        startTime = 1f;
        stageStart = false;
        stageEnd = false;
        gameTime = 0;
        progressTime = 0f;
        eventPos = 0;
        frameCount = 0;
        //eventEnd = 0;

        progressCheck = true;
    }

    public override void Progress()
    {
        float delta = Time.deltaTime;

        if(stageEnd)
        {
            progressCheck = false;
            button.interactable = true;

            if(!testMode)
            {
                StageClearInfo.instance.SetInfo(true,gameTime);
                SceneLoader.instance.LoadScene(6);
            }
            return;
        }

        if(waitExtinc)
        {
            if(EnemyManager.instance.count == 0)
                waitExtinc = false;
            else
                return;
        }

        if(dialogActive)
            return;

        gameTime += delta;

        progressTime += delta;

        if(!stageStart)
        {
            if(progressTime >= startTime)
            {
                stageStart = true;
                progressTime = 0f;
            }
        }
        else
        {
            if(progressTime >= fps)
            {
                if(frameCount == eventEnd)
                {
                    stageEnd = true;
                }
                else
                {
                    if(events[eventPos].frame == frameCount)
                    {
                        int count = events[eventPos].events.Count;
                        for(int i = 0; i < count; ++i)
                        {
                            events[eventPos].events[i].Progress();
                        }
                        ++eventPos;
                    }
                }
                ++frameCount;
                progressTime = 0f;
            }
        }

    }

    public override void Release()
    {

    }

    public void SetStaticEvent(bool value) {staticEvent = value;}
    public void SetWaitExtinc() {waitExtinc = true;}
    public bool IsStaticEvent() {return staticEvent;}

    [System.Serializable]
    public class FrameInfo
    {
        public int frame;
        public List<GameEventBase> events;

        public FrameInfo()
        {
            events = new List<GameEventBase>();
        }
    }

    public void LoadData(string[] data)
    {
        if(data == null)
        {
            MobileDebugger.instance.AddLine("data is null");
            return;
        }
        MobileDebugger.instance.AddLine(data[0]);
        MobileDebugger.instance.AddLine(data[1]);
        MobileDebugger.instance.AddLine(data[2]);
        MobileDebugger.instance.AddLine(data[3]);
        int dataPos = 0;
        fps = 100f / (float.Parse(data[dataPos++])) / 100f;

        while(true)
        {
            if(data[dataPos] == "////")
                break;
            int frame = int.Parse(data[dataPos++]);
            FrameInfo f = new FrameInfo();
            f.frame = frame;

            while(true)
            {
                if(data[dataPos][0] == '*')
                {
                    break;
                }
                string[] sp = data[dataPos++].Split('>');
                int code = int.Parse(sp[0]);

                f.events.Add(GetEvent(code,sp[1]));
            }

            ++dataPos;
            events.Add(f);
        }
    
        eventEnd = events[events.Count - 1].frame + 1;
    }

    public GameEventBase GetEvent(int code,string data)
    {
        if(code == 0)
        {
            return new GameTestEvent(data);
        }
        else if(code == 1)
        {
            //return new 
        }
        else if(code == 2)
        {
            return new GameEnemySpawnEvent(data);
        }
        else if(code == 3)
        {
            return new GameDialog(data);
        }
        else if(code == 4)
        {
            return new GameStaticEvent();
        }
        else if(code == 5)
        {
            return new GameWaitExtinc();
        }

        return null;
    }
}
