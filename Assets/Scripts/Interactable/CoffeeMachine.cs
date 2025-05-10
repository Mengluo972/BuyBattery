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
    private GameObject buttonTips;


    public void TriggerAction()
    {
        StartCoroutine(PlaySound());
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
        buttonTips.SetActive(true);
        ChangeTip.ChangePlayTips("- 喝杯咖啡存个档吧 -");

        await UniTask.WaitUntil(() => !inTrigger);

        Debug.Log("人好，咖啡机坏。");
        buttonTips.SetActive(false);
        ChangeTip.ChangePlayTips("");

    }

    private IEnumerator PlaySound()
    {
        Debug.Log("成功进入PlaySound交互方法");
        SoundManager.Instance.PlaySFX("coffeeMachine",1,7);
        Debug.Log("音效调用完成");
        yield return new WaitForSeconds(2);
        SoundManager.Instance.StopSFX();
    }

    // Start is called before the first frame update
    void Start()
    { buttonTips = transform.Find("ButtonTips").gameObject; buttonTips.SetActive(false); }

    // Update is called once per frame
    //void Update(){}
}
