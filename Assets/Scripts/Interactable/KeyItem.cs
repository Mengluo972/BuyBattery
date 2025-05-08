using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyItem : MonoBehaviour, IInteractable
{
    private int keyNumber=0;//暂时不启用
    public static event Action<int> DoorUnLock;

    public void inTriggerAnimation(bool b)
    {
        // throw new System.NotImplementedException();
    }

    public void TriggerAction()
    {
        
        gameObject.SetActive(false);
    }

    private void OnDisable()
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
