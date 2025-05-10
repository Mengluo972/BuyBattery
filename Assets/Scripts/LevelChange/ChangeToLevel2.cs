using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeToLevel2 : MonoBehaviour ,IInteractable
{
    // private BoxCollider boxCollider;
    private UIManeger uIManeger;
    private GameObject player;
    public int tagetLevel=2;
    private GameObject buttonTips;

    private void Start()
    {
        uIManeger=GameObject.Find("MainPanel").GetComponent<UIManeger>();
        player = GameObject.Find("player").gameObject;
        buttonTips = transform.Find("ButtonTips").gameObject;
        buttonTips.SetActive(false);
    }

    // private void OnTriggerStay(Collider other)
    // {
    //     
    // }

    public void TriggerAction()
    {
        player.tag = "HiddenPlayer";
        uIManeger.LoadLevelScene(tagetLevel);
    }

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
        ChangeTip.ChangePlayTips("- 喝杯咖啡，迎接下一天吧 -");
        Debug.Log("");

        await UniTask.WaitUntil(() => !inTrigger);

        buttonTips.SetActive(false);
        ChangeTip.ChangePlayTips("");
        Debug.Log("");

    }
}
