using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class TrackWatingState : IState
{
    private FSM _manager;
    private Parameter _parameter;
    private Vector3 _originalAngle;
    public TrackWatingState(FSM manager)
    {
        _manager = manager;
        _parameter = manager.parameter;
        _originalAngle = _manager.transform.forward;
    }
    
    
    public void OnEnter()
    {
        switch (_parameter.enemyAnimator)
        {
            case EnemyAnimator.colleague:
                _parameter.animator.Play("enemy_colleague@Idle");
                break;
            case EnemyAnimator.intern:
                _parameter.animator.Play("enemy_intern@idle1");
                break;
            case EnemyAnimator.cat:
                _parameter.animator.Play("metarig_Cat|stand");
                break;
            case EnemyAnimator.boss:
                _parameter.animator.Play("enemy_boss@Idle");
                break;
            case EnemyAnimator.maneger:
                _parameter.animator.Play("enemy_manager@idle");
                break;

        }

        _manager.transform.DOLookAt(towards: _originalAngle,duration: _parameter.flipWaitTimeAfter);
    }

    public void OnUpdate()
    {
        if (!_parameter.isChasing) return;
        _manager.TransitionState(StateType.Track);
    }

    public void OnExit()
    {
        
    }

    public void TriggerCheck()
    {
        if (_parameter.TriggerListener.IsCaughtPlayer)
        {
            _parameter.alarmValue = 0;
            _manager.TransitionState(StateType.Attack);
        }
    }
}
