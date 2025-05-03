using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum RoomTriggerType
{
    InRoom,OutRoom
}
public class RoomTrigger : MonoBehaviour
{
    [Header("此房间内会检测玩家进入的敌人,与追踪型敌人相关")]public List<FSM> enemies;
    [Header("房间触发器类型")]public RoomTriggerType roomTriggerType;
    private void OnTriggerStay(Collider other)
    {
        switch (roomTriggerType)
        {
            case RoomTriggerType.InRoom:
                if (other.CompareTag("Player"))
                {
                    foreach (var enemy in enemies)
                    {
                        enemy.parameter.isChasing = true;
                    }
                }

                break;
            case RoomTriggerType.OutRoom:
                if (other.CompareTag("Player"))
                {
                    foreach (var enemy in enemies)
                    {
                        enemy.parameter.isChasing = false;
                    }
                }

                break;
        }
        
    }
}
