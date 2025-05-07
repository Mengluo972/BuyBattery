using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class LyingItem : MonoBehaviour,IInteractable
{
    public bool inTrigger;
    public bool isTalking;
    public GameObject talkUI;

    public void TriggerAction()
    {
        ShowTalkUI();
    }

    public void inTriggerAnimation(bool b)
    {
        //����д����
        AnimateOn();
        inTrigger = b;

    }

    private async UniTaskVoid AnimateOn()
    {
        inTrigger = true;
        Debug.Log("������ô����");

        await UniTask.WaitUntil(() => !inTrigger);

        Debug.Log("��̣���");

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
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
