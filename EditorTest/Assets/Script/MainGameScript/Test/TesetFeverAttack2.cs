using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TesetFeverAttack2 : FeverBase
{
    public GameObject effectTest;
    public GameObject noteObject;
    public List<CircleObject> circleList = new List<CircleObject>();

    public float startSize = 3f;
    public float endSize = 1f;
    public float scaleTime = 1f;

    public float judgeTime = 0.3f;

    public int spawnCount = 9;
    public int count = 0;

    public float spawnTime = 0.5f;

    public Vector2 spawnMin;
    public Vector2 spawnMax;

    private float time = 0f;
    private int left = 0;

    public override void FirstInit()
    {
        for(int i = 0; i < spawnCount; ++i)
        {
            CircleObject circle = Instantiate(noteObject).GetComponent<CircleObject>();

            circle.Initialize();
            circle.gameObject.SetActive(false);

            circleList.Add(circle);
        }
    }

    public override void Initialize()
    {
        left = spawnCount;
        count = 0;
    }

    public override void Progress()
    {
       // PlayerManager.instance.target.DecreaseFever(12 * Time.deltaTime);


        bool mouseCheck = Input.GetMouseButtonDown(0);
        time += Time.deltaTime;
        if (time >= spawnTime && left > 0)
        {
            Spawn();
            time = 0f;
        }

        bool check = false;
        for (int i = 0; i < spawnCount; ++i)
        {
            if(circleList[i].isActive)
            {
                check = true;
                circleList[i].Progress();

                if(mouseCheck)
                {
                    if(circleList[i].coll.OverlapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition)))
                    {
                        if(circleList[i].Judge())
                        {
                            ++count;

                            GameObject obj = Instantiate(effectTest,circleList[i].transform.position,Quaternion.identity);
                            Destroy(obj,2f);
                        }

                        circleList[i].Disable();
                    }
                }
            }
        }
        if(!check && left == 0)
        {
            PlayerManager.instance.target.DecreaseFever(10000f);
        }
    }

    public override void EndEvent()
    {
        feverEndDirect.Initialize();

        Debug.Log(count);
    }

    public void Spawn()
    {
        for (int i = 0; i < spawnCount; ++i)
        {
            if(!circleList[i].isActive)
            {
                --left;
                float x = UnityEngine.Random.Range(spawnMin.x, spawnMax.x);
                float y = UnityEngine.Random.Range(spawnMin.y, spawnMax.y);

                x = x < 2f ? (x > -2f ? x + 2f : x) : x;
                y = y < 2f ? (y > -2f ? y + 2f : y) : y;

                Vector2 pos = new Vector2(x, y);
                circleList[i].Active(pos, startSize, endSize, scaleTime, judgeTime);
                return;
            }
        }
    }
}
