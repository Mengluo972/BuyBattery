using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using System;

public class ShenMiLittleBook : MonoBehaviour,IInteractable
{
    public static event Action EasterEgg;
    private bool inTrigger;


    public void TriggerAction()
    {
        EasterEgg?.Invoke();
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
        Debug.Log("这是什么？彩蛋？看一下。");

        await UniTask.WaitUntil(() => !inTrigger);

        Debug.Log("不看了，走了。");

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
