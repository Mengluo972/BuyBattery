using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class EndingControl : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    private UIManeger uIManeger;
    private bool isBacking = false;
    void Start()
    {
        uIManeger=GameObject.Find("MainPanel").GetComponent<UIManeger>();
        videoPlayer.Play();
    }

    void Update()
    {
        // if (videoPlayer.isPlaying == false)
        // {
        //     if (isBacking==false)
        //     {
        //         uIManeger.BackMainMenu();
        //         isBacking = true;
        //     }
        // }
        if (videoPlayer.frame==(long)(videoPlayer.frameCount-1))
        {
            uIManeger.BackMainMenu();
        }
    }
}
