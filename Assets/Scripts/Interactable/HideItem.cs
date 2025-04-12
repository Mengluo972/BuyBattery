using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using System;

public class HideItem : MonoBehaviour,IInteractable
{
    public static event Action PlayerHide;
    private bool inTrigger;


    public void TriggerAction()
    {
        PlayerHide?.Invoke();
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
        Debug.Log("�����ߴ�С����");

        await UniTask.WaitUntil(() => !inTrigger);

        Debug.Log("���ºι��Ƚ���");

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
