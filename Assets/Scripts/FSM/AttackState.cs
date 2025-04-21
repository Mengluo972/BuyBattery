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
        switch (_parameter.enemyAnimator)
        {
            case EnemyAnimator.colleague:
                _parameter.animator.Play("enemy_colleague@catch");
                break;
            case EnemyAnimator.intern:
                _parameter.animator.Play("enemy_intern@catch");
                break;
            case EnemyAnimator.boss:
                _parameter.animator.Play("enemy_boss@catch");
                break;
            case EnemyAnimator.maneger:
                _parameter.animator.Play("enemy_manager@catch1");
                break;

        }
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
