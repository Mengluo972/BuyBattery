using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RoomControl : MonoBehaviour
{
    [Header("房间内的追踪型敌人")]public List<FSM> enemies;
    private BoxCollider _boxCollider;

    void Start()
    {
        _boxCollider = GetComponent<BoxCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        foreach (var enemy in enemies)
        {
            enemy.parameter.isChasing = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        foreach (var enemy in enemies)
        {
            enemy.parameter.isChasing = false;
        }
    }
}
