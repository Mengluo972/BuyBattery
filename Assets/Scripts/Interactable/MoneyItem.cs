using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class MoneyItem : MonoBehaviour,IInteractable
{
    public static event Action GetMoney;
    public GameObject banner;
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
        Debug.Log("是钱");

        await UniTask.WaitUntil(() => !inTrigger);

        buttonTips.SetActive(false);
        ChangeTip.ChangePlayTips("");
        Debug.Log("不要钱了");

    }

    public void TriggerAction()
    {
        banner.SetActive(false);
        
        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        GetMoney?.Invoke();
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
