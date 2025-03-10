using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartrolState : IState
{
    private FSM _manager;
    private Parameter _parameter;
    private RayCastTest _rayCastTest;
    public PartrolState(FSM manager)
    {
        _manager = manager;
        _parameter = manager.parameter;
        _rayCastTest = manager.RayCastTest;
    }
    public void OnEnter()
    {
        _rayCastTest.IsTracing = true;
        // Debug.Log("_rayCastTest是否为null:"+(_rayCastTest==null));
        // Debug.Log("_rayCastTest.IsTracing的值为:"+_rayCastTest.IsTracing);
    }

    public void OnUpdate()
    {
        if (!_rayCastTest.IsPlayerDetected)return;
        if(_parameter.alarmValue>=_parameter.alarmMaxValue)
        {
            _manager.TransitionState(StateType.Chase);
            Debug.Log(_manager.gameObject.name + "发现玩家，进入追逐状态");
            return;
        }
        _parameter.alarmValue += _parameter.alarmAccelerationSpeed;
    }

    public void OnExit()
    {
        _rayCastTest.IsTracing = false;
    }
}
