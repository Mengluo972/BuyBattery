using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

public enum StateType
{
    Idle,Patrol,Chase,Flip,EndingChase,Attack,Stun,Find,Attract,AttractivePatrol,Track,TrackWaiting,TrackBack
}
[Serializable]
public class Parameter//敌人信息
{
    [Header("观测用敌人状态")]public StateType enemyState;//观测用敌人状态
    [Header("敌人类型")]public EnemyType enemyType;//敌人类型
    [Header("敌人模型")] public EnemyAnimator enemyAnimator;
    [Header("行走速度")]public float moveSpeed;
    [Header("追逐速度")]public float chaseSpeed;
    [Header("巡逻点(无论如何至少要有一个该角色站立的原点)")]public Transform[] partrolPoints;
    // [Header("已弃用")]public FakePatrolNodes[] fakePatrolPoints;//相关假巡逻点的集合，无需考虑先后顺序直接拖进去
    [NonSerialized]public Animator animator;
    [NonSerialized]public int PatrolIndex;
    [NonSerialized]public TriggerListener TriggerListener;
    [NonSerialized]public Vector3 LastPatrolPoint;
    [NonSerialized]public EnemyController EnemyController;
    [NonSerialized]public NavMeshAgent NavMeshAgent;
    [Header("场景中的道具管理器")]public PropManager propManager;
    [Header("玩家角色")]public Transform playerTarget;//可被识别为玩家的物体，这里建议手拖，减少性能消耗
    // public float chaseDistance;//进入找人状态的检测距离
    
    [Header("转向使用的时间")]public float flipTime;//转向使用的时间
    [Header("转向前停留时间")]public float flipWaitTimeBefore;//转向前停留时间
    [Header("转向后停留时间")]public float flipWaitTimeAfter;//转向后停留时间
    [Header("敌人警戒值")]public float alarmValue;//敌人警戒值
    [Header("警戒值增加速度")]public float alarmAccelerationSpeed;//警戒值增加速度
    [Header("警戒值减少速度")]public float alarmDecreaseSpeed;//警戒值减少速度
    [Header("警戒值最大值")]public float alarmMaxValue;//警戒值最大值
    [Header("最大吸引距离（如果敌人为吸引型的人的话才生效）")]public float attractDistance;//最大吸引距离（如果敌人为追逐型的人的话才生效）
    [Header("追踪型敌人是否处于追踪状态")]public bool isChasing = false;//追踪型敌人是否处于追踪状态
}

public enum EnemyType
{
    PatrolEnemy,
    //巡逻型敌人
    //按照固定路线巡逻，发现玩家追逐一段距离，离开距离后回到固定轨道
    AttractEnemy,
    //追逐型敌人
    //追着玩家发出声音，引起其他敌人注意。人类可以做僵尸蹦跳动画
    StandEnemy,
    //站桩型敌人
    //站在固定位置，发现玩家后追逐，离开范围后回到固定轨道
    StunEnemy,
    //定身型敌人
    //发现玩家，玩家固定位置一段时间
    TrackEnemy
    //追踪型敌人
    //当玩家进入房间后一直追逐玩家，玩家离开房间后停止追踪
    
}

public enum EnemyAnimator
{
    colleague,
    intern,
    cat,
    boss,
    maneger,
    guard,
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
        GetAnimation(parameter.animator);
        _meshCollider = transform.GetChild(0).GetComponent<MeshCollider>();//疑似无用
        transform.GetChild(0).AddComponent<TriggerListener>();//触发器监听脚本的添加在这里完成，无需手动添加
        parameter.TriggerListener = transform.GetChild(0).GetComponent<TriggerListener>();
        parameter.NavMeshAgent = GetComponent<NavMeshAgent>();
        
        _states.Add(StateType.Idle,new IdleState(this));
        _states.Add(StateType.Chase,new ChaseState(this));
        _states.Add(StateType.Patrol,new PartrolState(this));
        _states.Add(StateType.Flip,new FlipState(this));
        _states.Add(StateType.EndingChase,new EndingChaseState(this));
        _states.Add(StateType.Attack,new AttackState(this));
        _states.Add(StateType.Stun,new StunState(this));
        _states.Add(StateType.Find,new FindState(this));


        switch (parameter.enemyType)
        {
            case EnemyType.AttractEnemy:
                _states.Add(StateType.AttractivePatrol,new AttractivePatrolState(this));
                _states.Add(StateType.Attract,new AttractState(this));
                TransitionState(StateType.AttractivePatrol);
                return;
            case EnemyType.TrackEnemy:
                _states.Add(StateType.Track,new TrackState(this));
                _states.Add(StateType.TrackWaiting,new TrackWatingState(this));
                _states.Add(StateType.TrackBack,new TrackBackState(this));
                TransitionState(StateType.TrackWaiting);
                return;
            default:
                TransitionState(StateType.Patrol);
                break;
        }
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
        parameter.enemyState = type;
        _currentState = _states[type];
        _currentState.OnEnter();
    }
    
    public void GetAnimation(Animator ani)
    {
        string p="";
        switch (parameter.enemyAnimator)
        {
            case EnemyAnimator.intern:
                p = "Assets/ArtRes/Character/animator/Intern.controller";
                break;
            case EnemyAnimator.boss:
                p = "Assets/ArtRes/Character/animator/Boss.controller";
                break;
            case EnemyAnimator.cat:
                p = "Assets/ArtRes/Character/animator/Cat.controller";
                break;
            case EnemyAnimator.colleague:
                p = "Assets/ArtRes/Character/animator/Colleague.controller";
                break;
            case EnemyAnimator.maneger:
                p = "Assets/ArtRes/Character/animator/Maneger.controller";
                break;
        }
        // ani.runtimeAnimatorController = AssetDatabase.LoadAssetAtPath<RuntimeAnimatorController>(p);
        // Debug.Log(p);
    }

}
