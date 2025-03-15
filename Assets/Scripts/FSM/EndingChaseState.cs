
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingChaseState : IState
{
    private FSM _manager;
    private Parameter _parameter;
    private Transform _getBackTarget;
    private bool _startFakePatrol;
    
    public EndingChaseState(FSM manager)
    {
        _manager = manager;
        _parameter = manager.parameter;
    }
    public void OnEnter()
    {
        _startFakePatrol = false;
        Debug.Log("进入结束追逐状态");
        _getBackTarget = _parameter.partrolPoints[_parameter.PatrolIndex];
        float distance = Vector3.Distance(_manager.transform.position, _getBackTarget.position);
        foreach (var transform in _parameter.partrolPoints)
        {
            if (Vector3.Distance(_manager.transform.position, transform.position) < distance)
            {
                _getBackTarget = transform;
            }
        }

        foreach (var fakePatrolPoint in _parameter.fakePatrolPoints)
        {
            if (Vector3.Distance(_manager.transform.position, fakePatrolPoint.transform.position) < distance)
            {
                _getBackTarget = fakePatrolPoint.transform;
                _startFakePatrol = true;
            }
            
        }
    }

    public void OnUpdate()
    {
        if (_startFakePatrol)//开始假巡逻
        {
            if (Vector3.Distance(_manager.transform.position,_getBackTarget.position)<0.5f)//是否到达假巡逻点
            {
                //这边记得加上转向
                
                
                if (!_getBackTarget.TryGetComponent(out FakePatrolNodes fakePatrolNodes))//是否回到了真巡逻点
                {
                    //回到了真巡逻点
                    float distance = Vector3.Distance(_manager.transform.position,
                        _parameter.partrolPoints[_parameter.PatrolIndex].position);
                    for (int i = 0; i < _parameter.partrolPoints.Length; i++)
                    {
                        if (Vector3.Distance(_manager.transform.position,_parameter.partrolPoints[i].position)<distance)
                        {
                            _parameter.PatrolIndex = i;
                        }
                    }
                    _manager.TransitionState(StateType.Patrol);
                }
                else
                {
                    _getBackTarget = _getBackTarget.GetComponent<FakePatrolNodes>().parentNode;
                }
            }
            _manager.transform.position = Vector3.MoveTowards(_manager.transform.position, _getBackTarget.position,
                _parameter.moveSpeed * Time.deltaTime);
        }
        else
        {
            float distance = Vector3.Distance(_manager.transform.position,
                _parameter.partrolPoints[_parameter.PatrolIndex].position);
            for (int i = 0; i < _parameter.partrolPoints.Length; i++)
            {
                if (Vector3.Distance(_manager.transform.position,_parameter.partrolPoints[i].position)<distance)
                {
                    _parameter.PatrolIndex = i;
                }
            }
            _manager.TransitionState(StateType.Patrol);
        }
    }

    public void OnExit()
    {
    }

    public void TriggerCheck()
    {
        if (_parameter.TriggerListener.IsCaughtPlayer)
        {
            _manager.TransitionState(StateType.Attack);
        }
    }
}
