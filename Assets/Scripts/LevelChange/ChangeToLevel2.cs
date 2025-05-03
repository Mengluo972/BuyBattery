using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeToLevel2 : MonoBehaviour
{
    private BoxCollider boxCollider;

    private void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            
        }
    }
}
