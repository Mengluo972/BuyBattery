using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEditor;
using UnityEngine;

public class CoffeeMachine : MonoBehaviour,IInteractable
{
    public static event Action CoffeeSave;
    private bool inTrigger;
    public PropManager propManager;


    public void TriggerAction()
    {
        CoffeeSave?.Invoke();
        propManager.SaveGame();
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
        Debug.Log("咖啡机不错，摸摸。");

        await UniTask.WaitUntil(() => !inTrigger);

        Debug.Log("人好，咖啡机坏。");

    }

    // Start is called before the first frame update
    //void Start()
    //{}

    // Update is called once per frame
    //void Update(){}
}
