using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class DoorItem : MonoBehaviour,IDoorControl
{
    //public GameObject DoorCollider;
    private Animator ani;
    private bool inTrigger;
    private BoxCollider box;

    public void DoorClose()
    {
        inTrigger = false;
    }

    public void DoorOpen()
    {
        AnimateOn();
    }

    private async UniTaskVoid AnimateOn()
    {
        inTrigger = true;
        // box.isTrigger = true;
        Debug.Log("表锅我开门了喔");
        ani.Play("doorGlassBone|doorGlass_open");

        await UniTask.WaitUntil(() => !inTrigger);

        Debug.Log("表锅我关门了喔");
        ani.Play("doorGlassBone|doorGlass_close");
        // box.isTrigger = false;

    }

    // Start is called before the first frame update
    void Start()
    {
        //DoorCollider = transform.Find("DoorCollider").gameObject;
        ani=GetComponent<Animator>();
        box = GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
