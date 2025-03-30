using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ShootStage : IState
{
    private ObjectPool<GameObject> _bulletPool;
    private FSM _manager;
    private Parameter _parameter;
    public ShootStage(FSM manager)
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

    public void TriggerCheck()
    {
    }
}
