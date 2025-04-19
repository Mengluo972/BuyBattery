using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : IState
{
    public static event Action DeathEvent;

    private FSM _manager;
    private Parameter _parameter;

    public AttackState(FSM manager)
    {
        _manager = manager;
        _parameter = manager.parameter;
    }
    public void OnEnter()
    {
        Debug.Log("进入攻击状态");
        DeathEvent?.Invoke();
    }

    public void OnUpdate()
    {
        
    }

    public void OnExit()
    {
        
    }

    public void TriggerCheck()
    {
        
    }
}
