
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;

public class EndingChaseState : IState
{
    private FSM _manager;
    private Parameter _parameter;
    // private Transform _getBackTarget;
    // private bool _startFakePatrol;
    // private float _lookAtTime = 0.3f;
    private Vector3 _getBackTarget;
    private NavMeshAgent _navMeshAgent;
    
    public EndingChaseState(FSM manager)
    {
        _manager = manager;
        _parameter = manager.parameter;
        _navMeshAgent = manager.parameter.NavMeshAgent;
    }
    public void OnEnter()
    {
        _getBackTarget = _parameter.LastPatrolPoint;
        // _getBackTarget = _parameter.partrolPoints[0].position;
        // Debug.Log("进入结束追逐状态");
    }

    public void OnUpdate()
    {
        if (Vector3.Distance(_manager.transform.position, _getBackTarget) < 1.7f)
        {
            _manager.TransitionState(StateType.Patrol);
            // Debug.Log($"{_manager.gameObject.name}返回巡逻状态");
        }
        _navMeshAgent.SetDestination(_getBackTarget);
        _manager.transform.LookAt(_getBackTarget);
    }

    public void OnExit()
    {
        
    }

    public void TriggerCheck()
    {
        if (_parameter.TriggerListener.PlayerIsInvincible)
        {
            _parameter.alarmValue = 0;
            return;
        }
        if (_parameter.TriggerListener.IsCaughtPlayer)
        {
            _parameter.alarmValue = 0;
            _manager.TransitionState(StateType.Attack);
        }
    }
}