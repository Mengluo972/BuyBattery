using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TrackState : IState
{
    private FSM _manager;
    private Parameter _parameter;
    private NavMeshAgent _navMeshAgent;
    public TrackState(FSM manager)
    {
        _manager = manager;
        _parameter = manager.parameter;
        _navMeshAgent = manager.parameter.NavMeshAgent;
    }
    public void OnEnter()
    {
        switch (_parameter.enemyAnimator)
        {
            case EnemyAnimator.colleague:
                _parameter.animator.Play("enemy_colleague@run");
                break;
            case EnemyAnimator.intern:
                _parameter.animator.Play("enemy_intern@run");
                break;
            case EnemyAnimator.cat:
                _parameter.animator.Play("metarig_Cat|walk");
                break;
            case EnemyAnimator.boss:
                _parameter.animator.Play("enemy_boss@run");
                break;
            case EnemyAnimator.maneger:
                _parameter.animator.Play("enemy_maneger@run");
                break;

        }
        _navMeshAgent.speed = _parameter.chaseSpeed;
    }

    public void OnUpdate()
    {
        if (!_parameter.isChasing)//敌人结束追逐
        {
            _manager.TransitionState(StateType.TrackBack);
            return;
        }
        _navMeshAgent.SetDestination(_parameter.playerTarget.position);
    }

    public void OnExit()
    {
    }

    public void TriggerCheck()
    {
        if (_parameter.TriggerListener.PlayerIsInvincible)
        {
            _parameter.alarmValue = 0;
            _manager.TransitionState(StateType.TrackBack);
            return;
        }
        if (_parameter.TriggerListener.IsCaughtPlayer)
        {
            _parameter.alarmValue = 0;
            _manager.TransitionState(StateType.Attack);
            return;
        }
    }
}
