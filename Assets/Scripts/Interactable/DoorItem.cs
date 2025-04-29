using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class DoorItem : MonoBehaviour,IDoorControl
{
    public GameObject DoorCollider;
    public Animator ani;
    private bool inTrigger;

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
        Debug.Log("表锅我开门了喔");
        ani.Play("doorGlassBone|doorGlass_openOut");

        await UniTask.WaitUntil(() => !inTrigger);

        Debug.Log("表锅我关门了喔");
        ani.Play("doorGlassBone|doorGlass_openIn");

    }

    // Start is called before the first frame update
    void Start()
    {
        //DoorCollider = transform.Find("DoorCollider").gameObject;
        ani=GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
