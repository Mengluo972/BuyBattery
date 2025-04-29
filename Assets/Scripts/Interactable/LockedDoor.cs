using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class LockedDoor : MonoBehaviour,IDoorControl
{
    public int DoorNumber;
    public GameObject DoorCollider;
    public bool isLocked=true;
    public Animator ani;
    private bool inTrigger;

    public void DoorOpen()
    {
        if (!isLocked)
        {
            AnimateOn();
            
        }
    }

    public void DoorClose()
    {
        inTrigger = false;
    }


    private void OnEnable()
    {
        KeyItem.DoorUnLock += UnLock;
        
    }

    private void OnDisable()
    {
        KeyItem.DoorUnLock -= UnLock;
    }

    private void UnLock(int Taget)
    {
        if (Taget == DoorNumber)
        {
            isLocked = false;
            DoorCollider.SetActive(false);
        }
    }

    private async UniTaskVoid AnimateOn()
    {
        inTrigger = true;
        Debug.Log("表锅我开门了喔");
        ani.Play("doorGlassBone|doorGlass_open");

        await UniTask.WaitUntil(() => !inTrigger);

        Debug.Log("表锅我关门了喔");
        ani.Play("doorGlassBone|doorGlass_close");

    }

    // Start is called before the first frame update
    void Start()
    {
        DoorCollider = transform.Find("DoorCollider").gameObject;
        ani = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
}
