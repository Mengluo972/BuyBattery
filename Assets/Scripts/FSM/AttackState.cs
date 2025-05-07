using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;

public class AttackState : IState
{
    public static event Action<EnemyAnimator> DeathEvent;

    private FSM _manager;
    private Parameter _parameter;



    public AttackState(FSM manager)
    {
        _manager = manager;
        _parameter = manager.parameter;
    }
    public void OnEnter()
    {
        Debug.Log("攻击玩家");

        PmcPlayerController playerController = null;
        playerController = _parameter.playerTarget.gameObject.GetComponent<PmcPlayerController>();
        if (playerController.IsSafe)
        {
            playerController.PlayerProtect();
            //_parameter.TriggerListener.PlayerIsInvincible = true;
        }
        else
        {
            _parameter.propManager.PlusCaughtTime();
            Debug.Log("you dead");
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
                case EnemyAnimator.guard:
                    _parameter.animator.Play("enemy_guard@catch");
                    break;
            }

            // PropManager.caughtTime++;
            DeathEvent?.Invoke(_parameter.enemyAnimator);
        }
        
    }

    public void OnUpdate()
    {
        
    }

    public void OnExit()
    {
        
    }

    public void TriggerCheck()
    {
        if (_parameter.TriggerListener.PlayerIsInvincible)
        {
            _parameter.alarmValue = 0;
            _manager.TransitionState(StateType.EndingChase);
            return;
        }
    }
}
