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
        ChangeTip.ChangePlayTips("- �ϰ���ҿ۵Ĺ��� -");
        Debug.Log("��Ǯ");

        await UniTask.WaitUntil(() => !inTrigger);

        buttonTips.SetActive(false);
        ChangeTip.ChangePlayTips("");
        Debug.Log("��ҪǮ��");

    }

    public void TriggerAction()
    {
        banner.SetActive(false);
        
        gameObject.SetActive(false);
    }

    private async UniTaskVoid ElevatorOnTips()
    {
        await UniTask.Delay(200);

        ChangeTip.ChangePlayTips("- ������Ʊ�Ҿʹ�ְ -");

        await UniTask.Delay(5000);

        ChangeTip.ChangePlayTips("");

    }

    private void OnDisable()
    {
        GetMoney?.Invoke();
    }

    // Start is called before the first frame update
    void Start()
    {
        buttonTips = transform.Find("ButtonTips").gameObject;
        buttonTips.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
