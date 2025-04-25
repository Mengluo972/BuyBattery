using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockedDoor : MonoBehaviour,IInteractable
{
    public int DoorNumber;
    private bool isUnlocked=false;
    private bool isOpen=false;

    public void inTriggerAnimation(bool b)
    {
        if (isOpen)
        {

        }

    }

    public void TriggerAction()
    {
        if (isUnlocked)
        {
            isOpen = true;
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
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
