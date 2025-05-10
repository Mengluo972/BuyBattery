using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class LyingItem : MonoBehaviour,IInteractable
{
    public bool inTrigger;
    public bool isTalking;
    public GameObject talkUI;
    private GameObject buttonTips;

    public void TriggerAction()
    {
        ShowTalkUI();
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
        Debug.Log("唉你怎么似了");
        buttonTips.SetActive(true);
        ChangeTip.ChangePlayTips("- 好像可以先藏进去 -");

        await UniTask.WaitUntil(() => !inTrigger);

        Debug.Log("冲刺，冲");
        buttonTips.SetActive(false);
        ChangeTip.ChangePlayTips("");

    }

    private async UniTaskVoid ShowTalkUI()
    {
        if (isTalking) { return; }

        talkUI.SetActive(true);
        isTalking = true;

        await UniTask.Delay(10000);

        talkUI.SetActive(false);
        isTalking=false;

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
