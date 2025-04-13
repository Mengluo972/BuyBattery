using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttractivePatrolState : IState
{
    private FSM _manager;
    private Parameter _parameter;
    public AttractivePatrolState(FSM manager)
    {
        _manager = manager;
        _parameter = manager.parameter;
    }

    public void OnEnter()
    {
        
    }

    public void OnUpdate()
    {
        if(Vector3.Distance(_manager.transform.position,_parameter.partrolPoints[_parameter.PatrolIndex].position)<0.5f)
        {
            _parameter.PatrolIndex++;//一旦到达就立刻增加索引值，在转向状态中不再额外增加
            if(_parameter.PatrolIndex>=_parameter.partrolPoints.Length)//越界检测
            {
                _parameter.PatrolIndex = 0;
            }
            _manager.TransitionState(StateType.Flip);
        }
        _manager.transform.position = Vector3.MoveTowards(_manager.transform.position,
            _parameter.partrolPoints[_parameter.PatrolIndex].position, _parameter.moveSpeed * Time.deltaTime);
    }

    public void OnExit()
    {
        
    }

    public void TriggerCheck()
    {
        
    }
}
