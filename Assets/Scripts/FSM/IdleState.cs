using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : IState
{
    private FSM _manager;
    private Parameter _parameter;

    public IdleState(FSM manager)
    {
        _manager = manager;
        _parameter = manager.parameter;
    }
    public void OnEnter()
    {
        Debug.Log(_manager.gameObject.name + "正处于Idle状态，这是一个未设定的状态");
    }

    public void OnUpdate()
    {
    }

    public void OnExit()
    {
    }
}
