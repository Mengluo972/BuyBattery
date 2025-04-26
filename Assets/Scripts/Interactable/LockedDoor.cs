using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockedDoor : MonoBehaviour,IInteractable
{
    public int DoorNumber;
    public GameObject DoorCollider;
    private bool isUnlocked=false;
    private bool isOpen=false;

    public void inTriggerAnimation(bool b)
    {
        if (isOpen)
        {
            //这里加开门的动画
            //todo...

            DoorCollider.SetActive(!b);
        }

    }

    public void TriggerAction()
    {
        if (isUnlocked)
        {
            isOpen = true;
            inTriggerAnimation(true);
        }
        
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
            isUnlocked = true;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        DoorCollider = transform.Find("DoorCollider").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
