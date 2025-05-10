using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class SafeItem : MonoBehaviour,IInteractable
{
    public static event Action PlayerSafe;
    private bool inTrigger;
    private GameObject buttonTips;
    // public Transform player;


    public void TriggerAction()
    {
        SoundManager.Instance.PlaySFX("headBox_short",1,3);
        PlayerSafe?.Invoke();
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
        Debug.Log("握握手，握握双手");
        buttonTips.SetActive(true);
        ChangeTip.ChangePlayTips("- 戴上它可以伪装成同事 -");

        await UniTask.WaitUntil(() => !inTrigger);
        
        Debug.Log("牢石耐击术");
        buttonTips.SetActive(false);
        ChangeTip.ChangePlayTips("");

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
