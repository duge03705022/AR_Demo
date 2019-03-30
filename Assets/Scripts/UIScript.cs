using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIScript : MonoBehaviour
{
    public GameParameter gameParameter;
    public RFIBManager rFIBManager;

    public int notTouchTime;
    public int[] touchHistory;
    public Dictionary<string, bool> ifTouch;
    public int touchCount;
    public bool ifAction;

    public GameObject[] frames;
    public GameObject[] ads;
    public int nowTouch;

    // Start is called before the first frame update
    void Start()
    {
        notTouchTime = 0;
        touchHistory = new int[9];
        ifTouch = new Dictionary<string, bool>();
        for (int i = 0; i < touchHistory.Length; i++)
        {
            touchHistory[i] = -1;
        }
        foreach (var dic in gameParameter.posDic)
        {
            ifTouch.Add(dic.Key, false);
        }
        touchCount = 0;
        ifAction = false;

        nowTouch = 0;
        UpdateFrame();
    }

    // Update is called once per frame
    void Update()
    {
        CountTouchTime();

        if (false)
        {
            string str = "";
            for (int i = 0; i < touchHistory.Length; i++)
            {
                str += touchHistory[i] + " ";
            }
            Debug.Log(str);
        }

        if (touchCount == 2 && !ifAction)
        {
            ifAction = true;
            DoAction();
        }
    }

    void CountTouchTime()
    {
        bool thisTouch = false;
        foreach (var dic in gameParameter.posDic)
        {
            if (rFIBManager.tagSensing[dic.Key] && !ifTouch[dic.Key])
            {
                notTouchTime = 0;
                touchHistory[touchCount] = dic.Value;
                touchCount++;
                ifTouch[dic.Key] = true;
                thisTouch = true;
            }
        }
        if (!thisTouch)
        {
            notTouchTime++;
        }
        if (notTouchTime > 35)
        {
            for (int i = 0; i < touchHistory.Length; i++)
            {
                touchHistory[i] = -1;
            }
            foreach (var dic in gameParameter.posDic)
            {
                ifTouch[dic.Key] = false;
            }
            touchCount = 0;
            ifAction = false;
        }
    }

    void DoAction()
    {
        if (touchHistory[1] == touchHistory[0] + 1 && touchHistory[1] / 3 == touchHistory[0] / 3)           // swipe right
        {
            nowTouch++;
        }
        else if (touchHistory[1] == touchHistory[0] - 1 && touchHistory[1] / 3 == touchHistory[0] / 3)      // swipe left
        {
            nowTouch--;
        }
        else if (touchHistory[1] == touchHistory[0] + 3)                                                    // swipe down
        {
            nowTouch += 2;
        }
        else if (touchHistory[1] == touchHistory[0] - 3)                                                    // swipe up
        {
            nowTouch -= 2;
        }
        nowTouch = (nowTouch + 4) % 4;

        UpdateFrame();
    }

    void UpdateFrame()
    {
        for (int i = 0; i < frames.Length; i++)
        {
            frames[i].SetActive(false);
        }
        frames[nowTouch].SetActive(true);

        StartCoroutine(UpdateAd());
    }

    IEnumerator UpdateAd()
    {
        for (int i = 0; i < ads.Length; i++)
        {
            ads[i].SetActive(false);
        }

        yield return new WaitForSeconds(1f);

        ads[nowTouch].SetActive(true);
    }
}
