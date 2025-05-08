using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class MoneyItem : MonoBehaviour,IInteractable
{
    public static event Action GetMoney;

    public void inTriggerAnimation(bool b)
    {
        //throw new System.NotImplementedException();
    }

    public void TriggerAction()
    {
        GetMoney?.Invoke();
        gameObject.SetActive(false);
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
