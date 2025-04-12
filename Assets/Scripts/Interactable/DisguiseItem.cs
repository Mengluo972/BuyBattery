using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using System;

public class DisguiseItem : MonoBehaviour,IInteractable
{
    public static event Action PlayerDisguise;
    private bool inTrigger;


    public void TriggerAction()
    {
        PlayerDisguise?.Invoke();
        gameObject.SetActive(false);
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
        Debug.Log("û���ˣ�����αװ���ߡ�");

        await UniTask.WaitUntil(() => !inTrigger);

        Debug.Log("�뿪αװ���߽�����Χ");

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
