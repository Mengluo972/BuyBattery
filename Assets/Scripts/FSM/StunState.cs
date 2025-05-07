using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Pool;
/// <summary>
/// 逮人状态的变体
/// </summary>
public class StunState : IState
{
    private FSM _manager;
    private Parameter _parameter;
    private RayCastTest _rayCastTest;
    private NavMeshAgent _navMeshAgent;
    private PmcPlayerController _playerController;
    public StunState(FSM manager)
    {
        _manager = manager;
        _parameter = manager.parameter;
        _rayCastTest = manager.RayCastTest;
        _navMeshAgent = manager.parameter.NavMeshAgent;
        _playerController = manager.parameter.playerTarget.GetComponent<PmcPlayerController>();
    }
    public void OnEnter()
    {
        _rayCastTest.IsChaseTracing = true;
        _rayCastTest.IsPatrolTracing = false;
        _playerController.IsMoveAble = false;
        _playerController.IsRunable = false;
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
        // if (!_rayCastTest.IsPlayerDetected)
        // {
        //     _manager.TransitionState(StateType.Find);
        //     return;
        // }

        _parameter.alarmValue-= _parameter.alarmDecreaseSpeed*Time.deltaTime;
        _manager.transform.LookAt(_parameter.playerTarget);
        _navMeshAgent.SetDestination(_parameter.playerTarget.position);
    }

    public void OnExit()
    {
        _rayCastTest.IsChaseTracing = false;
        _playerController.enabled = true;
        _playerController.IsMoveAble = true;
        _playerController.IsRunable = true;
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
