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

    public GameObject[] icons;
    public GameObject[] publicAds;
    public GameObject[,] ads;
    public int nowTouch;
    public int nowAd;

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
        nowAd = 0;

        ads = new GameObject[4, 2];
        for (int i = 0; i < 4; i++)
        {
            ads[i, 0] = publicAds[i * 2];
            ads[i, 1] = publicAds[i * 2 + 1];
        }

        InitAll();
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

        //if (Input.GetKeyUp("r"))
        //{
        //    StartCoroutine(SwipeIcon("r"));
        //}

        //if (Input.GetKeyUp("l"))
        //{
        //    StartCoroutine(SwipeIcon("l"));
        //}

        //if (Input.GetKeyUp("u"))
        //{
        //    StartCoroutine(SwipeAd("u"));
        //}

        //if (Input.GetKeyUp("d"))
        //{
        //    StartCoroutine(SwipeAd("d"));
        //}
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
            StartCoroutine(SwipeIcon("r"));
        }
        else if (touchHistory[1] == touchHistory[0] - 1 && touchHistory[1] / 3 == touchHistory[0] / 3)      // swipe left
        {
            StartCoroutine(SwipeIcon("l"));
        }
        else if (touchHistory[1] == touchHistory[0] + 3)                                                    // swipe down
        {
            StartCoroutine(SwipeAd("d"));
        }
        else if (touchHistory[1] == touchHistory[0] - 3)                                                    // swipe up
        {
            StartCoroutine(SwipeAd("u"));
        }
        nowTouch = (nowTouch + 4) % 4;

        //UpdateFrame();
    }

    void InitAll()
    {
        icons[(nowTouch + 0) % 4].SetActive(true);
        icons[(nowTouch + 1) % 4].SetActive(true);
        icons[(nowTouch + 2) % 4].SetActive(false);
        icons[(nowTouch + 3) % 4].SetActive(true);

        icons[(nowTouch + 0) % 4].transform.localPosition = new Vector3(0f, 1f, 0f);
        icons[(nowTouch + 1) % 4].transform.localPosition = new Vector3(2f, 0.5f, 0f);
        icons[(nowTouch + 2) % 4].transform.localPosition = new Vector3(0f, 0f, 0f);
        icons[(nowTouch + 3) % 4].transform.localPosition = new Vector3(-2f, 0.5f, 0f);

        icons[(nowTouch + 0) % 4].transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
        icons[(nowTouch + 1) % 4].transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
        icons[(nowTouch + 3) % 4].transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);

        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                ads[i, j].SetActive(false);
            }
        }
    }

    private IEnumerator SwipeIcon(string direction)
    {
        if (direction == "r")
        {
            icons[(nowTouch + 2) % 4].SetActive(true);
            icons[(nowTouch + 2) % 4].transform.localPosition = new Vector3(-2f, 0f, 0f);
            icons[(nowTouch + 2) % 4].transform.localScale = new Vector3(0f, 0f, 0f);

            ads[(nowTouch + 3) % 4, 0].SetActive(true);
            ads[(nowTouch + 3) % 4, 0].transform.localPosition = new Vector3(0f, 1f, 0f);
            ads[(nowTouch + 3) % 4, 0].transform.localScale = new Vector3(0f, 0f, 0f);

            ads[(nowTouch + 0) % 4, nowAd].transform.localPosition = new Vector3(0f, 0f, 0f);
            for (int i = 0; i < 10; i++)
            {
                icons[(nowTouch + 0) % 4].transform.localPosition += new Vector3(0.2f, -0.05f, 0f);
                icons[(nowTouch + 1) % 4].transform.localPosition += new Vector3(0f, -0.05f, 0f);
                icons[(nowTouch + 2) % 4].transform.localPosition += new Vector3(0f, 0.05f, 0f);
                icons[(nowTouch + 3) % 4].transform.localPosition += new Vector3(0.2f, 0.05f, 0f);

                icons[(nowTouch + 0) % 4].transform.localScale += new Vector3(-0.005f, -0.005f, -0.005f);
                icons[(nowTouch + 1) % 4].transform.localScale += new Vector3(-0.02f, -0.02f, -0.02f);
                icons[(nowTouch + 2) % 4].transform.localScale += new Vector3(0.02f, 0.02f, 0.02f);
                icons[(nowTouch + 3) % 4].transform.localScale += new Vector3(0.005f, 0.005f, 0.005f);

                ads[(nowTouch + 0) % 4, nowAd].transform.localScale += new Vector3(-0.05f, -0.05f, -0.05f);
                ads[(nowTouch + 3) % 4, 0].transform.localScale += new Vector3(0.05f, 0.05f, 0.05f);

                yield return new WaitForSeconds(0.1f);
            }

            icons[(nowTouch + 1) % 4].SetActive(false);
            ads[(nowTouch + 0) % 4, nowAd].SetActive(false);

            nowTouch += 3;
            nowAd = 0;
        }
        else if (direction == "l")
        {
            icons[(nowTouch + 2) % 4].SetActive(true);
            icons[(nowTouch + 2) % 4].transform.localPosition = new Vector3(2f, 0f, 0f);
            icons[(nowTouch + 2) % 4].transform.localScale = new Vector3(0f, 0f, 0f);

            ads[(nowTouch + 1) % 4, 0].SetActive(true);
            ads[(nowTouch + 1) % 4, 0].transform.localPosition = new Vector3(0f, 1f, 0f);
            ads[(nowTouch + 1) % 4, 0].transform.localScale = new Vector3(0f, 0f, 0f);

            ads[(nowTouch + 0) % 4, nowAd].transform.localPosition = new Vector3(0f, 1f, 0f);
            for (int i = 0; i < 10; i++)
            {
                icons[(nowTouch + 0) % 4].transform.localPosition += new Vector3(-0.2f, -0.05f, 0f);
                icons[(nowTouch + 1) % 4].transform.localPosition += new Vector3(-0.2f, 0.05f, 0f);
                icons[(nowTouch + 2) % 4].transform.localPosition += new Vector3(0f, 0.05f, 0f);
                icons[(nowTouch + 3) % 4].transform.localPosition += new Vector3(0f, -0.05f, 0f);

                icons[(nowTouch + 0) % 4].transform.localScale += new Vector3(-0.005f, -0.005f, -0.005f);
                icons[(nowTouch + 1) % 4].transform.localScale += new Vector3(0.005f, 0.005f, 0.005f);
                icons[(nowTouch + 2) % 4].transform.localScale += new Vector3(0.02f, 0.02f, 0.02f);
                icons[(nowTouch + 3) % 4].transform.localScale += new Vector3(-0.02f, -0.02f, -0.02f);

                ads[(nowTouch + 0) % 4, nowAd].transform.localScale += new Vector3(-0.05f, -0.05f, -0.05f);
                ads[(nowTouch + 1) % 4, 0].transform.localScale += new Vector3(0.05f, 0.05f, 0.05f);

                yield return new WaitForSeconds(0.1f);
            }

            icons[(nowTouch + 3) % 4].SetActive(false);
            ads[(nowTouch + 0) % 4, nowAd].SetActive(false);

            nowTouch += 1;
            nowAd = 0;
        }
    }

    private IEnumerator SwipeAd(string direction)
    {
        if (direction == "u" && nowAd == 0)
        {
            ads[nowTouch % 4, 1].SetActive(true);
            ads[nowTouch % 4, 1].transform.localScale = new Vector3(0f, 0f, 0f);
            ads[nowTouch % 4, 0].transform.localPosition = new Vector3(0f, 0f, 0f);
            ads[nowTouch % 4, 1].transform.localPosition = new Vector3(0f, 1f, 0f);
            for (int i = 0; i < 10; i++)
            {
                ads[nowTouch % 4, 0].transform.localScale += new Vector3(-0.05f, -0.05f, -0.05f);
                ads[nowTouch % 4, 1].transform.localScale += new Vector3(0.05f, 0.05f, 0.05f);
                yield return new WaitForSeconds(0.1f);
            }

            ads[nowTouch % 4, 0].SetActive(false);
            nowAd = 1;
        }
        else if (direction == "d" && nowAd == 1)
        {
            ads[nowTouch % 4, 0].SetActive(true);
            ads[nowTouch % 4, 0].transform.localScale = new Vector3(0f, 0f, 0f);
            ads[nowTouch % 4, 0].transform.localPosition = new Vector3(0f, 1f, 0f);
            ads[nowTouch % 4, 1].transform.localPosition = new Vector3(0f, 0f, 0f);
            for (int i = 0; i < 10; i++)
            {
                ads[nowTouch % 4, 0].transform.localScale += new Vector3(0.05f, 0.05f, 0.05f);
                ads[nowTouch % 4, 1].transform.localScale += new Vector3(-0.05f, -0.05f, -0.05f);
                yield return new WaitForSeconds(0.1f);
            }

            ads[nowTouch % 4, 1].SetActive(false);
            nowAd = 0;
        }
        else
        {
            for (int i = 0; i < 3; i++)
            {
                ads[nowTouch % 4, nowAd].transform.localScale += new Vector3(-0.05f, -0.05f, -0.05f);
                yield return new WaitForSeconds(0.1f);
            }
            for (int i = 0; i < 3; i++)
            {
                ads[nowTouch % 4, nowAd].transform.localScale += new Vector3(0.05f, 0.05f, 0.05f);
                yield return new WaitForSeconds(0.1f);
            }
        }
    }
}
