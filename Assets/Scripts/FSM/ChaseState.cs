using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : IState
{
    private FSM _manager;
    private Parameter _parameter;
    public ChaseState(FSM manager)
    {
        _manager = manager;
        _parameter = manager.parameter;
    }
    public void OnEnter()
    {
        
    }

    public void OnUpdate()
    {
        
    }

    public void OnExit()
    {
    }
}
