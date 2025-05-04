using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerListener : MonoBehaviour
{
    [NonSerialized] public bool IsCaughtPlayer;
    [NonSerialized] public bool PlayerIsInvincible;

    private void Awake()
    {
        IsCaughtPlayer = false;
        PlayerIsInvincible = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("InvinciblePlayer"))
        {
            PlayerIsInvincible = true;
        }
        if (other.gameObject.CompareTag("Player"))
        {
            IsCaughtPlayer = true;
        }
    }
}
