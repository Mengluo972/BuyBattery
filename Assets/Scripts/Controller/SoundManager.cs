using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager
{

    private static SoundManager _instance;
    public static SoundManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new SoundManager();
            }
            return _instance;
        }
    }
    //防止外部调用构造
    private SoundManager(){}
}
