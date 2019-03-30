using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frame : MonoBehaviour
{
    public GameParameter gameParameter;
    public string[] RFIDTag;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayClip()
    {
        GetComponent<AudioSource>().Play();
    }
}
