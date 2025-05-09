using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SoundType
{
    Fail,
    FU,
    Main,
    Stealth,
    Success
}
public class SoundManager
{
    private static readonly string soundPath = "Sound/";
    private static readonly string bgmName = "BGM_";
    private static SoundManager _instance;
    private static Dictionary<SoundType, AudioClip> bgms;
    private static Dictionary<string, AudioClip> sfxs;
    private static AudioSource _audioBGM;
    private static AudioSource _audioSFX;
    public static SoundManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new SoundManager();
                bgms = new Dictionary<SoundType, AudioClip>();
                sfxs = new Dictionary<string, AudioClip>();
                _audioBGM = GameObject.Find("BGMPlayer").GetComponent<AudioSource>();
                _audioSFX = GameObject.Find("SFXPlayer").GetComponent<AudioSource>();
                bgms.Add(SoundType.Fail, Resources.Load<AudioClip>(soundPath + bgmName+"Fail"));
                bgms.Add(SoundType.Success, Resources.Load<AudioClip>(soundPath + bgmName+"Success"));
                bgms.Add(SoundType.FU, Resources.Load<AudioClip>(soundPath +bgmName+ "FU"));
                bgms.Add(SoundType.Main, Resources.Load<AudioClip>(soundPath +bgmName+ "Main"));
                bgms.Add(SoundType.Stealth, Resources.Load<AudioClip>(soundPath + "Stealth"));

                for (int i = 1; i <= 4; i++)
                {
                    sfxs.Add("SFX_Props_cat_0"+i, Resources.Load<AudioClip>(soundPath + "SFX_Props_cat_0" + i));
                }

                for (int i = 1; i <= 7; i++)
                {
                    sfxs.Add("SFX_Props_coffeeMachine_0" + i, Resources.Load<AudioClip>(soundPath + "SFX_Props_coffeeMachine_0" + i));
                }

                for (int i = 1; i <= 6; i++)
                {
                    sfxs.Add("SFX_Props_door_0"+i,Resources.Load<AudioClip>(soundPath + "SFX_Props_door_0" + i));
                }

                for (int i = 1; i <= 6; i++)
                {
                    sfxs.Add("SFX_Props_doorClose_0"+i,Resources.Load<AudioClip>(soundPath + "SFX_Props_doorClose_0" + i));
                }

                for (int i = 1; i <= 4; i++)
                {
                    sfxs.Add("SFX_Props_headBox_long_0"+i,Resources.Load<AudioClip>(soundPath + "SFX_Props_headBox_0" + i));
                }
                
                for (int i = 1; i <= 3; i++)
                {
                    sfxs.Add("SFX_Props_headBox_short_0"+i,Resources.Load<AudioClip>(soundPath + "SFX_Props_headBox_0" + i));
                }
                
                sfxs.Add("SFX_Props_printer",Resources.Load<AudioClip>(soundPath + "SFX_Props_printer"));

                for (int i = 1; i <= 6; i++)
                {
                    sfxs.Add("SFX_Props_screen_0"+i,Resources.Load<AudioClip>(soundPath + "SFX_Props_screen_0" + i));
                }

                for (int i = 1; i <= 6; i++)
                {
                    sfxs.Add("SFX_Props_trashCan_0"+i,Resources.Load<AudioClip>(soundPath + "SFX_Props_trashCan_0" + i));
                }

                for (int i = 1; i <= 6; i++)
                {
                    sfxs.Add("SFX_UI_0"+i,Resources.Load<AudioClip>(soundPath + "SFX_UI_0" + i));
                }
            }
            return _instance;
        }
    }
    //防止外部调用构造
    private SoundManager(){}

    private void SourceCheck()
    {
        if (_audioSFX == null)
        {
            GameObject go = GameObject.Instantiate(Resources.Load<GameObject>("PlayerObj/SFXPlayer"));
            _audioSFX = go.GetComponent<AudioSource>();
        }

        if (_audioBGM == null)
        {
            GameObject go = GameObject.Instantiate(Resources.Load<GameObject>("PlayerObj/BGMPlayer"));
            _audioBGM = go.GetComponent<AudioSource>();
        }
    }

    public void PlayBGM(SoundType type)
    {
        SourceCheck();
        if (!bgms.TryGetValue(type,out AudioClip clip))
        {
            Debug.Log($"{type}音效播放失败");
            return;
        }
        _audioBGM.clip = clip;
        _audioBGM.Play();
    }

    public void StopBGM()
    {
        SourceCheck();
        _audioBGM.Stop();
    }

    public void PauseBGM()
    {
        SourceCheck();
        _audioBGM.Pause();
    }

    public void BeginBGM()
    {
        SourceCheck();
        _audioBGM.UnPause();
    }

    public void SetBGMLoop(bool isLoop)
    {
        SourceCheck();
        _audioBGM.loop = isLoop;
    }

    public bool GetBGMState()
    {
        return _audioBGM.isPlaying;
    }
    /// <summary>
    /// 播放音效
    /// </summary>
    /// <param name="name">直接输入音效的文件名</param>
    public void PlaySFX(string name)
    {
        SourceCheck();
        if (!sfxs.TryGetValue(name, out AudioClip clip))
        {
            Debug.Log($"{name}音效播放失败");
            return;
        }
        _audioSFX.clip = clip;
        _audioSFX.Play();
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="name">简略名称</param>
    /// <param name="type">Props为1,UI为2</param>
    /// <param name="maxIndex">最多拥有的数量</param>
    public void PlaySFX(string name,int type,int maxIndex)
    {
        SourceCheck();
        Debug.Log("进入音效播放方法");
        string SFXName = "";
        switch (type)
        {
            case 1:
                SFXName = "SFX_Props_" +name+"_0";
                break;
            case 2:
                SFXName = "SFX_UI_" +name+"0";
                break;
            // default:
            //     Debug.Log("音效类型错误");
            //     break;
        }
        int index = Random.Range(1, maxIndex + 1);
        if (!sfxs.TryGetValue(SFXName+index, out AudioClip clip))
        {
            Debug.Log($"{SFXName+index}音效播放失败");
            return;
        }
        _audioSFX.clip = clip;
        _audioSFX.Play();
    }

    public void StopSFX()
    {
        SourceCheck();
        _audioSFX.Stop();
    }
}
