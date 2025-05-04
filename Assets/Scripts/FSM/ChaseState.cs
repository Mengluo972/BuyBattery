using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// 逮人状态
/// </summary>
public class ChaseState : IState
{
    private FSM _manager;
    private Parameter _parameter;

    private RayCastTest _rayCastTest;
    private NavMeshAgent _navMeshAgent;
    
    public ChaseState(FSM manager)
    {
        _manager = manager;
        _parameter = manager.parameter;
        _rayCastTest = manager.RayCastTest;
        _navMeshAgent = manager.parameter.NavMeshAgent;
        // _navMeshAgent.speed = _parameter.chaseSpeed;
    }
    public void OnEnter()
    {
        // Debug.Log($"{_manager.gameObject.name}进入逮人状态");
        _rayCastTest.IsChaseTracing = true;
        _rayCastTest.IsPatrolTracing = false;
        _navMeshAgent.speed = _parameter.chaseSpeed;
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
    }

    public void OnUpdate()
    {
        if (_parameter.alarmValue<=0)
        {
            _parameter.alarmValue = 0;
            _manager.TransitionState(StateType.EndingChase);
            return;
        }
        //这里可能会隐藏一个bug，玩家在敌人面前，但是玩家与敌人之间有障碍物
        if (!_rayCastTest.IsPlayerDetected)
        {
            _manager.TransitionState(StateType.Find);
            // Debug.Log($"{_manager.gameObject.name}通过!_rayCastTest.IsPlayerDetected进入找人状态");
            return;
        }

        _parameter.alarmValue-= _parameter.alarmDecreaseSpeed*Time.deltaTime;
        _manager.transform.LookAt(_parameter.playerTarget);
        
        _navMeshAgent.SetDestination(_parameter.playerTarget.position);
        
    }
 
    public void OnExit()
    {
        // Debug.Log($"{_manager.gameObject.name}退出逮人状态");
        _rayCastTest.IsChaseTracing = false;
    }

    public void TriggerCheck()
    {
        
        if (_parameter.TriggerListener.IsCaughtPlayer)
        {
            _parameter.alarmValue = 0;
            _manager.TransitionState(StateType.Attack);
            return;
        }
    }
}
