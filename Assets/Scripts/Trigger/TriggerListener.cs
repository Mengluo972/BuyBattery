using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerListener : MonoBehaviour
{
    [NonSerialized] public bool IsCaughtPlayer;

    private void Awake()
    {
        IsCaughtPlayer = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            IsCaughtPlayer = true;
        }
    }
}
