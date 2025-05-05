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
    private static Dictionary<SoundType, AudioClip> _clips;
    private AudioSource _audio;
    public static SoundManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new SoundManager();
                _clips = new Dictionary<SoundType, AudioClip>();
                _clips.Add(SoundType.Fail, Resources.Load<AudioClip>(soundPath + "Fail"));
                _clips.Add(SoundType.Success, Resources.Load<AudioClip>(soundPath + "Success"));
                _clips.Add(SoundType.FU, Resources.Load<AudioClip>(soundPath + "FU"));
                _clips.Add(SoundType.Main, Resources.Load<AudioClip>(soundPath + "Main"));
                _clips.Add(SoundType.Stealth, Resources.Load<AudioClip>(soundPath + "Stealth"));
            }
            return _instance;
        }
    }
    //防止外部调用构造
    private SoundManager(){}

    public void Play(SoundType type)
    {
        if (!_clips.TryGetValue(type,out AudioClip clip))
        {
            Debug.Log($"{type}音效播放失败");
            return;
        }
        _audio.clip = clip;
        _audio.Play();
    }

    public void SetLoop(bool isLoop)
    {
        _audio.loop = isLoop;
    }
}
