using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : IState
{
    private FSM _manager;
    private Parameter _parameter;
    private TriggerListener _triggerListener;
    public ChaseState(FSM manager)
    {
        _manager = manager;
        _parameter = manager.parameter;
        _triggerListener = manager.transform.GetChild(0).GetComponent<TriggerListener>();
    }
    public void OnEnter()
    {
        Debug.Log("进入追逐状态");
    }

    public void OnUpdate()
    {
        // Debug.Log("正在追逐");
        if (_parameter.alarmValue<=0)
        {
            _parameter.alarmValue = 0;
            _manager.TransitionState(StateType.EndingChase);
        }
        _parameter.alarmValue-= _parameter.alarmDecreaseSpeed*Time.deltaTime;
        _manager.transform.LookAt(_parameter.playerTarget.position);
        _manager.transform.position = Vector3.MoveTowards(_manager.transform.position, _parameter.playerTarget.position,
            _parameter.chaseSpeed * Time.deltaTime);
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
