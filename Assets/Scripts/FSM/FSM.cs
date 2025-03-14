using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StateType
{
    Idle,Patrol,Chase,Flip,EndingChase
}
[Serializable]
public class Parameter//敌人信息
{
    public float moveSpeed;
    public float chaseSpeed;
    public Transform[] partrolPoints;
    public Animator animator;
    public int patrolIndex;
    public Transform lastPatrolPoint;

    public float flipTime;//转向使用的时间
    public float flipWaitTimeBefore;//转向前停留时间
    public float flipWaitTimeAfter;//转向后停留时间
    public float alarmValue;//敌人警戒值
    public float alarmAccelerationSpeed;//警戒值增加速度
    public float alarmMaxValue;//警戒值最大值
}
public class FSM : MonoBehaviour//每一个具有巡逻状态的敌人都会有一个FSM组件
{
    public Parameter parameter;
    private IState _currentState;
    private Dictionary<StateType,IState> _states = new Dictionary<StateType, IState>();
    private MeshCollider _meshCollider;//疑似无用
    [NonSerialized]public RayCastTest RayCastTest;
    void Start()
    {
        RayCastTest = GetComponent<RayCastTest>();
        parameter.animator = GetComponent<Animator>();
        _meshCollider = transform.GetChild(0).GetComponent<MeshCollider>();//疑似无用
        
        _states.Add(StateType.Idle,new IdleState(this));
        _states.Add(StateType.Chase,new ChaseState(this));
        _states.Add(StateType.Patrol,new PartrolState(this));
        _states.Add(StateType.Flip,new FlipState(this));
        _states.Add(StateType.EndingChase,new EndingChaseState(this));
        
        // TransitionState(StateType.Idle);
        TransitionState(StateType.Patrol);

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
