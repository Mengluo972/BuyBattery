using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public enum StateType
{
    Idle,Patrol,Chase,Flip,EndingChase,Attack,Shoot,Find
}
[Serializable]
public class Parameter//敌人信息
{
    public float moveSpeed;
    public float chaseSpeed;
    public Transform[] partrolPoints;
    public FakePatrolNodes[] fakePatrolPoints;//相关假巡逻点的集合，无需考虑先后顺序直接拖进去
    public Animator animator;
    [NonSerialized]public int PatrolIndex;
    [NonSerialized]public TriggerListener TriggerListener;
    public Transform lastPatrolPoint;
    public Transform playerTarget;//可被识别为玩家的物体，这里建议手拖，减少性能消耗

    public float flipTime;//转向使用的时间
    public float flipWaitTimeBefore;//转向前停留时间
    public float flipWaitTimeAfter;//转向后停留时间
    public float alarmValue;//敌人警戒值
    public float alarmAccelerationSpeed;//警戒值增加速度
    public float alarmDecreaseSpeed;//警戒值减少速度
    public float alarmMaxValue;//警戒值最大值
}
public class FSM : MonoBehaviour//每一个具有巡逻状态的敌人都会有一个FSM组件，且需要有一个具有collider触发器的子物体
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
        transform.GetChild(0).AddComponent<TriggerListener>();//触发器监听脚本的添加在这里完成，无需手动添加
        parameter.TriggerListener = transform.GetChild(0).GetComponent<TriggerListener>();
        
        _states.Add(StateType.Idle,new IdleState(this));
        _states.Add(StateType.Chase,new ChaseState(this));
        _states.Add(StateType.Patrol,new PartrolState(this));
        _states.Add(StateType.Flip,new FlipState(this));
        _states.Add(StateType.EndingChase,new EndingChaseState(this));
        _states.Add(StateType.Attack,new AttackState(this));
        _states.Add(StateType.Shoot,new ShootStage(this));
        _states.Add(StateType.Find,new FindState(this));
        
        
        TransitionState(StateType.Patrol);
        
        
    }

    void Update()
    {
        _currentState.TriggerCheck();
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
