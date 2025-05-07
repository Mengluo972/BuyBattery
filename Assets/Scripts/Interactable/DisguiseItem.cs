using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using System;

public class DisguiseItem : MonoBehaviour,IInteractable
{
    public static event Action PlayerDisguise;
    private bool inTrigger;


    public void TriggerAction()
    {
        SoundManager.Instance.PlaySFX("headBox_long",1,4);
        PlayerDisguise?.Invoke();
        gameObject.SetActive(false);
    }

    public void inTriggerAnimation(bool b)
    {
        //这里写动画
        AnimateOn();
        inTrigger = b;

    }

    private async UniTaskVoid AnimateOn()
    {
        inTrigger = true;
        Debug.Log("没词了，碰到伪装道具。");

        await UniTask.WaitUntil(() => !inTrigger);

        Debug.Log("离开伪装道具交互范围");

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
