using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class SafeItem : MonoBehaviour
{
    public static event Action PlayerSafe;
    private bool inTrigger;


    public void TriggerAction()
    {
        PlayerSafe?.Invoke();
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
        Debug.Log("�����֣�����˫��");

        await UniTask.WaitUntil(() => !inTrigger);

        Debug.Log("��ʯ�ͻ���");

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
