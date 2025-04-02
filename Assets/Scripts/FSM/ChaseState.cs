using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
/// <summary>
/// 逮人状态
/// </summary>
public class ChaseState : IState
{
    private FSM _manager;
    private Parameter _parameter;

    private RayCastTest _rayCastTest;
    // private TriggerListener _triggerListener;
    // private Transform _targetPosition;

    // private float _chaseCooldownTime = 0.5f;//追逐冷却时间
    // private float _chaseTimer = 0f;//追逐冷却计时器
    // private CancellationTokenSource _cts;
    
    public ChaseState(FSM manager)
    {
        _manager = manager;
        _parameter = manager.parameter;
        _rayCastTest = manager.RayCastTest;
        // _triggerListener = manager.transform.GetChild(0).GetComponent<TriggerListener>();
        // _cts = new CancellationTokenSource();
    }
    public void OnEnter()
    {
        Debug.Log("进入逮人状态");
        _rayCastTest.IsChaseTracing = true;
        _rayCastTest.IsPatrolTracing = false;
    }

    public void OnUpdate()
    {
        // Debug.Log("正在追逐");
        if (_parameter.alarmValue<=0)
        {
            _parameter.alarmValue = 0;
            _manager.TransitionState(StateType.EndingChase);
            return;
        }
        //这里可能会隐藏一个bug，玩家在敌人面前，但是玩家与敌人之间有障碍物
        if (!_rayCastTest.IsPlayerDetected)
        {
            _manager.TransitionState(StateType.Find);
            Debug.Log("通过!_rayCastTest.IsPlayerDetected进入追人状态");
            return;
        }
        // //这个逻辑尚不明确
        // if (MapInfoController.BarrierCheck(AStarManager.Instance.FindPath(
        //         new Vector2(Floor(_manager.transform.position.x), Floor(_manager.transform.position.z)),
        //         new Vector2(Floor(_manager.parameter.playerTarget.transform.position.x),
        //             Floor(_manager.parameter.playerTarget.transform.position.z)))))
        // {
        //     _manager.TransitionState(StateType.Find);
        //     Debug.Log("通过MapInfoController.BarrierCheck进入追人状态");
        // }
        
        _parameter.alarmValue-= _parameter.alarmDecreaseSpeed*Time.deltaTime;
        _manager.transform.LookAt(_parameter.playerTarget);
        _manager.transform.position = Vector3.MoveTowards(_manager.transform.position, _parameter.playerTarget.position,
            _parameter.chaseSpeed * Time.deltaTime);
        
        // _chaseTimer += Time.deltaTime;
        // if (Vector3.Distance(_manager.transform.position,_parameter.playerTarget.position)<0.5f&&_chaseTimer >= _chaseCooldownTime)
        // {
        //     _chaseTimer = 0f;
        // }
    }
    //寻路逻辑:发现玩家后，获取玩家坐标，在玩家与该敌人之间进行一次寻路，将最近的一个寻路点添加到下一个寻路目标点，每隔一段时间再次获取玩家坐标，进行一次寻路
    //直到接触到玩家跳出状态才会停止寻路
    
    
    
    //需要做将AStarNode转换为坐标的工作,大概在MapInfoController中实现
    //进入状态时调用，只调用一次
    //如果状态更改记得取消
    //不能直接传坐标
    
    private float Floor(float num)
    {
        if (num > 0) return (int)num + 0.5f;
        return (int)num - 0.5f;
    }
 
    public void OnExit()
    {
        _rayCastTest.IsChaseTracing = false;
    }

    public void TriggerCheck()
    {
        if (_parameter.TriggerListener.IsCaughtPlayer)
        {
            _manager.TransitionState(StateType.Attack);
        }
    }
}
