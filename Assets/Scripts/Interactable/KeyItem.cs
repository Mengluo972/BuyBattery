using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyItem : MonoBehaviour, IInteractable
{
    public int keyNumber;
    public static event Action<int> DoorUnLock;

    public void inTriggerAnimation(bool b)
    {
        throw new System.NotImplementedException();
    }

    public void TriggerAction()
    {
        DoorUnLock?.Invoke(keyNumber);
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
