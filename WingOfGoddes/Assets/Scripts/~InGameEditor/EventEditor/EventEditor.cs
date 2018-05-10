using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UnityEditor;

public class EventEditor : EditorBase {

    public class Frame
    {
        public static Frame select = null;
        public int frame;

        public List<EditorEventBase> events = new List<EditorEventBase>();

        public void Progress()
        {
            for(int i = 0; i < events.Count; ++i)
            {
                events[i].Progress();
            }
        }

        public void AddEvent(EditorEventBase ev){events.Add(ev);}

        public bool ListCheck()
        {
            return events.Count != 0;
        }

        public Frame(int f){frame = f;}
    }

    public class Page
    {
        public List<Frame> frames = new List<Frame>();

        public void AddFrame(Frame frame)
        {
            frames.Add(frame);
        }
        
        public Page(){}
    }

    public class FrameObject
    {
        public Define.RectF bounds;
        Image image;

        public Define.RectF TpToRect(RectTransform rect)
        {
            return MathEx.RectTransformToRect(rect,new Define.RectF());
        }

        public bool MouseCheck(Vector2 pos)
        {
            return MathEx.PointInRect(pos,bounds);
        }

        public void SetSprite(Sprite spr)
        {
            image.sprite = spr;
        }

        public void SetColor(Color color)
        {
            image.color = color;
        }

        public FrameObject(){}
        public FrameObject(RectTransform rect, Image img)
        {
            bounds = TpToRect(rect);
            image = img;
        }
    }

    public class ButtonObject
    {
        public static ButtonObject select = null;

        public Button button;
        public Text text;

        public int index;

        public EditorEventBase eventBase;

        public void Set(EditorEventBase ev, string name)
        {
            eventBase = ev;
            text.text = name;

            button.interactable = true;
        }

        public void Click()
        {
            if(select != null)
            {
                select.Release();
            }

            button.interactable = false;
            eventBase.ButtonClicked();

            select = this;
        }

        public void Release()
        {
            button.interactable = true;
            eventBase.ButtonRelease();
        }

        public ButtonObject(){}
        public ButtonObject(Button b,Text t,int i)
        {
            button = b;
            text = t;
            index = i;

            button.onClick.AddListener(() => Click());
        }
    }

    public enum EventType
    {
        TestEvent,
    }

    public Transform frameContainor;
    public Transform eventContainor;

	public GameObject frameOrigin;
    public GameObject eventButtonOrigin;

    public Sprite[] frameSprite;

    public Button newPageButton;
    public Button prevButton;
    public Button nextButton;
    public Button deleteEventButton;

    public Button[] ProgressButtons;

    public Text pageText;

    public InputField frameInput;

    List<Page> containor = new List<Page>();
    List<FrameObject> frameObjects = new List<FrameObject>();
    List<ButtonObject> buttonObjects = new List<ButtonObject>();

    float arrowTimer = 0f;
    float arrowTime = 0.5f;

    int frameCount = 60;

    int framePerSec = 12;

    int selectePage = 0;

    public void Awake()
    {
        FrameCreate();
        EventButtonsCreate();
        NewPage();

        AllUISync();
    }

    public void Update()
    {
        if(Input.GetMouseButtonDown(0))
            FrameSelectCheck();
        EventDeleteButtonSync();

        HotKeyCheck();
    }

    // public void OnGUI()
    // {
    //     Handles.
    //     Handles.DrawLine(Vector3.zero,new Vector3(1000f,1000f,0f));
    // }

    public void FrameSelectCheck()
    {
        Vector2 point = Input.mousePosition;
        
        point.x -= frameContainor.transform.localPosition.x * 2f;

        for(int i = 0; i < frameCount; ++i)
        {
            if(frameObjects[i].MouseCheck(point))
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
            OnReleasePick();
            frameObjects[Frame.select.frame].SetColor(Color.white);
            Frame.select = null;
        }
    }

    public void FrameSelect(int index)
    {
        if(Frame.select != null)
        {
            OnReleasePick();

            if(Frame.select == containor[selectePage].frames[index])
            {
                Frame.select = null;
                frameObjects[index].SetColor(Color.white);

                EventListSync();

                return;
            }
            else
            {
                frameObjects[Frame.select.frame].SetColor(Color.white);
            }
        }

        Frame.select = containor[selectePage].frames[index];
        frameObjects[index].SetColor(Color.green);

        EventListSync();

        ButtonObject.select = null;

        OnPickEvent();
    }

    public void OnPickEvent()
    {
        for(int i = 0; i < Frame.select.events.Count; ++i)
        {
            Frame.select.events[i].PickEvent();
        }
    }

    public void OnReleasePick()
    {
        for(int i = 0; i < Frame.select.events.Count; ++i)
        {
            Frame.select.events[i].ReleasePick();
        }
    }

    public void HotKeyCheck()
    {
        if(Frame.select != null)
        {
            if(Input.GetKey(KeyCode.LeftArrow))
            {
                arrowTimer -= Time.deltaTime;
                if(arrowTimer <= 0f)
                {
                    arrowTimer = (arrowTime -= arrowTime > 0.15f ? 0.15f : 0f);
                    int index = Frame.select.frame == 0 ? frameCount - 1 : Frame.select.frame - 1;
                
                    FrameSelect(index);
                }
            }
            else if(Input.GetKey(KeyCode.RightArrow))
            {
                arrowTimer -= Time.deltaTime;
                if(arrowTimer <= 0f)
                {
                    arrowTimer = (arrowTime -= arrowTime > 0.15f ? 0.15f : 0f);
                    int index = Frame.select.frame == frameCount - 1 ? 0 : Frame.select.frame + 1;
                
                    FrameSelect(index);
                }
            }
            else
            {
                arrowTimer = 0f;
                arrowTime = 0.5f;
            }
        }
    }

    public void PageSelect(int value)
    {
        selectePage += value;
        
        AllUISync();
    }

    public void NewPageButton()
    {
        NewPage();
        
        AllUISync();
    }

    public void EventButtonsCreate()
    {
        for(int i = 0; i < 13; ++i)
        {
            EventButtonCreate();
        }
    }

    public void EventButtonCreate()
    {
        GameObject obj = Instantiate(eventButtonOrigin);
        obj.transform.SetParent(eventContainor);
        obj.transform.localPosition = new Vector3(0f,200f - 30f * buttonObjects.Count,0f);

        buttonObjects.Add(new ButtonObject(obj.GetComponent<Button>(),obj.GetComponentInChildren<Text>(),buttonObjects.Count)); 
    }

    public void FrameCreate()
    {
        if(frameObjects.Count != 0)
        {
            return;
        }

        for(int i = 0; i < frameCount; ++i)
        {
            GameObject obj = Instantiate(frameOrigin) as GameObject;

            obj.transform.SetParent(frameContainor);
            obj.transform.localPosition = new Vector3(i * 19f - frameCount / 2 * 19f + 8f,0f ,0f);

            frameObjects.Add(new FrameObject(obj.GetComponent<RectTransform>(),obj.GetComponent<Image>()));
        }
    }

    public void AddEventTest()
    {
        if(Frame.select == null)
            return;
        Frame.select.AddEvent(new TestEvent());

        FrameSync();
        EventListSync();
    }

    public void NewPage()
    {
        Page page = new Page();

        for(int i = 0; i < frameCount; ++i)
        {
            page.AddFrame(new Frame(i));
        }

        containor.Add(page);

        selectePage = containor.Count - 1;
    }

    public void DeleteSelectEvent()
    {
        if(Frame.select != null && ButtonObject.select != null)
        {
            Frame.select.events.RemoveAt(ButtonObject.select.index);

            ButtonObject.select = null;
            EventListSync();
            
            if(Frame.select.events.Count == 0)
                FrameSync();
            //selectEvent = null;
        }
    }

    public void DisableAllButtons()
    {
        for(int i = 0; i < buttonObjects.Count; ++i)
        {
            buttonObjects[i].button.gameObject.SetActive(false);
        }
    }

    public void EventListSync()
    {
        DisableAllButtons();

        ButtonObject.select = null;

        if(Frame.select == null)
            return;

        for(int i = 0; i < Frame.select.events.Count; ++i)
        {
            buttonObjects[i].Set(Frame.select.events[i],"Check");
            buttonObjects[i].button.gameObject.SetActive(true);
        }

        EventDeleteButtonSync();
    }

    public void EventDeleteButtonSync()
    {
        if(ButtonObject.select != null)
        {
            deleteEventButton.interactable = true;
        }
        else
        {
            deleteEventButton.interactable = false;
        }
    }

    public void FrameSync()
    {
        for(int i = 0; i < frameCount; ++i)
        {
            if(containor[selectePage].frames[i].ListCheck())
            {
                frameObjects[i].SetSprite(frameSprite[1]);
            }
            else
            {
                frameObjects[i].SetSprite(frameSprite[0]);
            }
        }

        //SelectFrameRelease();
    }

    public void SetFramesColor(Color color)
    {
        for(int i = 0; i < frameObjects.Count; ++i)
        {
            if(Frame.select == containor[selectePage].frames[i])
            {
                frameObjects[i].SetColor(Color.green);
            }
            else
            {
                frameObjects[i].SetColor(color);
            }
        }
    }

    public void AllUISync()
    {
        PageButtonSync();
        FrameSync();
        PageTextSync();
        DisableAllButtons();
        SelectFrameRelease();

        EventDeleteButtonSync();
    }

    public void PageButtonSync(bool toFalse = false)
    {
        if(!toFalse)
        {
            newPageButton.interactable = true;

            if(selectePage == 0)
            {
                prevButton.interactable = false;
            }
            else
            {
                prevButton.interactable = true;
            }

            if(selectePage == containor.Count -1 || containor.Count == 0)
            {
                nextButton.interactable = false;
            }
            else
            {
                nextButton.interactable = true;
            }
        }
        else
        {
            newPageButton.interactable = false;
            prevButton.interactable = false;
            nextButton.interactable = false;
        }
    }

    public void PageTextSync()
    {
        pageText.text = (selectePage + 1) + "/" + containor.Count;
    }

    public void FpsSync()
    {
        if(frameInput.text == "")
        {
            frameInput.text = "12";
        }
        framePerSec = int.Parse(frameInput.text);
    }

    public void ProgressButtonActive(bool check)
    {
        ProgressButtons[0].interactable = check;
        ProgressButtons[1].interactable = check;
        ProgressButtons[2].interactable = !check;
    }

    public IEnumerator ProgressAll()
    {
        float speed = 1 / (float)framePerSec;
        int frame = 0;
        int page = selectePage = 0;
        PageTextSync();
        PageButtonSync(true);
        ProgressButtonActive(false);

        while(true)
        {
            containor[page].frames[frame].Progress();
            frameObjects[frame].SetColor(Color.blue);
            yield return new WaitForSeconds(speed);
            frameObjects[frame++].SetColor(Color.white);

            if(frame == frameCount)
            {
                ++page;
                frame = 0;
                if(page == containor.Count)
                    break;
            }

            selectePage = page;
            PageTextSync();
            FrameSync();
        }

        PageButtonSync();
        ProgressButtonActive(true);

        SetFramesColor(Color.white);
    }

    public IEnumerator ProgressThisPage()
    {
        float speed = 1 / (float)framePerSec;
        int count = 0;

        PageButtonSync(true);
        ProgressButtonActive(false);

        while(true)
        {
            containor[selectePage].frames[count].Progress();
            frameObjects[count].SetColor(Color.blue);
            yield return new WaitForSeconds(speed);
            frameObjects[count++].SetColor(Color.white);

            if(count == frameCount)
                break;
        }

        PageButtonSync();
        ProgressButtonActive(true);

        SetFramesColor(Color.white);
    }

    public void ProgressStop()
    {
        StopAllCoroutines();

        PageButtonSync();
        ProgressButtonActive(true);

        SetFramesColor(Color.white);
    }

    public void ProgressThisPageButton()
    {
        StartCoroutine(ProgressThisPage());
    }

    public void ProgressAllButton()
    {
        StartCoroutine(ProgressAll());
    }
}
