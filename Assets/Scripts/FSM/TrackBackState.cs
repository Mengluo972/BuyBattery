using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
/// <summary>
/// 追踪型敌人返回原点的状态
/// </summary>
public class TrackBackState : IState
{
    private FSM _manager;
    private Parameter _parameter;
    private NavMeshAgent _navMeshAgent;
    
    public TrackBackState(FSM manager)
    {
        _manager = manager;
        _parameter = manager.parameter;
        _navMeshAgent = _parameter.NavMeshAgent;
    }

    public void OnEnter()
    {
        _navMeshAgent.speed = _parameter.moveSpeed;        
    }

    public void OnUpdate()
    {
        if (Vector3.Distance(_manager.transform.position,_parameter.partrolPoints[0].position)<=_navMeshAgent.stoppingDistance)
        {
            _manager.TransitionState(StateType.TrackWaiting);
            return;
        }
        _navMeshAgent.SetDestination(_parameter.partrolPoints[0].position);
    }

    public void OnExit()
    {
    }

    public void TriggerCheck()
    {
        if (_parameter.TriggerListener.PlayerIsInvincible)
        {
            return;
        }
        if (_parameter.TriggerListener.IsCaughtPlayer)
        {
            _parameter.alarmValue = 0;
            _manager.TransitionState(StateType.Attack);
        }
    }
}
