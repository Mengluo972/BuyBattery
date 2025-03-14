
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingChaseState : IState
{
    private FSM _manager;
    private Parameter _parameter;
    
    public EndingChaseState(FSM manager)
    {
        _manager = manager;
        _parameter = manager.parameter;
    }
    public void OnEnter()
    {
        throw new System.NotImplementedException();
    }

    public void OnUpdate()
    {
        throw new System.NotImplementedException();
    }

    public void OnExit()
    {
        throw new System.NotImplementedException();
    }
}
