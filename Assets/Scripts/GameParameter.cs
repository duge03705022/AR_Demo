using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameParameter : MonoBehaviour
{
    public Dictionary<string, int> posDic;

    public string[] posTag;

    // Start is called before the first frame update
    void Start()
    {
        posDic = new Dictionary<string, int>();

        for (int i = 0; i < posTag.Length; i++)
        {
            posDic.Add(posTag[i], i);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
