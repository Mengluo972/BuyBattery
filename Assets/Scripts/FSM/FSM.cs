using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StateType
{
    Idle,Patrol,Chase
}
[Serializable]
public class Parameter
{
    public float moveSpeed;
    public float chaseSpeed;
    public Transform[] partrolPoints;
    public Animator animator;
}
public class FSM : MonoBehaviour
{
    public Parameter parameter;
    private IState _currentState;
    private Dictionary<StateType,IState> _states = new Dictionary<StateType, IState>();
    private MeshCollider _meshCollider;//疑似无用
    void Start()
    {
        _states.Add(StateType.Idle,new IdleState(this));
        _states.Add(StateType.Chase,new ChaseState(this));
        _states.Add(StateType.Patrol,new PartrolState(this));
        
        TransitionState(StateType.Idle);
        
        parameter.animator = GetComponent<Animator>();
        _meshCollider = transform.GetChild(0).GetComponent<MeshCollider>();//疑似无用
    }

    void Update()
    {
        _currentState.OnUpdate();
    }

    public void TransitionState(StateType type)
    {
        if(_currentState!=null)
        {
            _currentState.OnExit();
        }
        _currentState = _states[type];
        _currentState.OnEnter();
    }
    
}
