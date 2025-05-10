using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class KeyItem : MonoBehaviour, IInteractable
{
    private int keyNumber=0;//暂时不启用
    public static event Action<int> DoorUnLock;
    private GameObject buttonTips;

    public void inTriggerAnimation(bool b)
    {
        AnimateOn();
        inTrigger = b;
    }

    private bool inTrigger;
    private async UniTaskVoid AnimateOn()
    {
        inTrigger = true;
        buttonTips.SetActive(true);
        ChangeTip.ChangePlayTips("- 交互后一段时间内隐身 -");
        Debug.Log("碰到钥匙");

        await UniTask.WaitUntil(() => !inTrigger);

        buttonTips.SetActive(false);
        ChangeTip.ChangePlayTips("");
        Debug.Log("离开钥匙");

    }

    public void TriggerAction()
    {
        
        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        DoorUnLock?.Invoke(keyNumber);
    }


    // Start is called before the first frame update
    void Start()
    {
        buttonTips = transform.Find("ButtonTips").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
