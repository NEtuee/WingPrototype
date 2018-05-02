using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleObject : MonoBehaviour {
    public Collider2D coll;
    public Transform edge;

    public float startSize = 3f;
    public float endSize = 1f;
    public float scaleTime = 1f;

    public float judgeTime = 0.3f;

    public bool isActive = false;

    private float between;
    private float time = 0f;
    float calTime = 0f;

    public void Initialize()
    {
        coll = GetComponent<Collider2D>();

        //VarInit();
    }

    public void Active(Vector3 pos, float start, float end, float stime,float judge = 0.3f)
    {
        gameObject.SetActive(true);

        transform.position = pos;

        startSize = start;
        endSize = end;
        scaleTime = stime;
        judgeTime = judge;

        VarInit();
    }

    public void VarInit()
    {
        between = startSize - endSize;
        time = 0f;
        calTime = 0f;

        isActive = true;

        edge.localScale = new Vector3(startSize, startSize,1f);
    }

    public void Progress()
    {
        time += Time.deltaTime;

        calTime = time / scaleTime;
        float size = startSize - (calTime * between);

        edge.localScale = new Vector3(size, size, 1f);

        if(calTime >= 1f + judgeTime)
        {
            Disable();
        }
    }

    public void Disable()
    {
        isActive = false;
        gameObject.SetActive(false);
    }

    public bool Judge() { return calTime >= 1f - judgeTime && calTime <= 1f + judgeTime; }

}
