using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttractState : IState
{
    private FSM _manager;
    private Parameter _parameter;

    public AttractState(FSM manager)
    {
        _parameter = manager.parameter;
        _manager = manager;
    }

    public void OnEnter()
    {
        List<FSM> enemies = _parameter.EnemyController.GetEnemies(_manager, _parameter.attractDistance);
        foreach (var enemy in enemies)
        {
            enemy.TransitionState(StateType.Chase);
        }
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
