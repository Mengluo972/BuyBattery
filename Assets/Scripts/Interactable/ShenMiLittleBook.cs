using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using System;

public class ShenMiLittleBook : MonoBehaviour,IInteractable
{
    public static event Action EasterEgg;
    private bool inTrigger;


    public void TriggerAction()
    {
        EasterEgg?.Invoke();
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
        Debug.Log("����ʲô���ʵ�����һ�¡�");

        await UniTask.WaitUntil(() => !inTrigger);

        Debug.Log("�����ˣ����ˡ�");

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
