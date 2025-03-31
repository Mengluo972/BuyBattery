using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class ChaseState : IState
{
    private FSM _manager;
    private Parameter _parameter;
    private TriggerListener _triggerListener;
    private Transform _targetPosition;
    // private CancellationTokenSource _cts;
    
    public ChaseState(FSM manager)
    {
        _manager = manager;
        _parameter = manager.parameter;
        _triggerListener = manager.transform.GetChild(0).GetComponent<TriggerListener>();
        // _cts = new CancellationTokenSource();
    }
    public void OnEnter()
    {
        // Debug.Log("进入追逐状态");
        ChangeChaseTarget();
        
    }

    public void OnUpdate()
    {
        // Debug.Log("正在追逐");
        if (_parameter.alarmValue<=0)
        {
            _parameter.alarmValue = 0;
            _manager.TransitionState(StateType.EndingChase);
        }
        _parameter.alarmValue-= _parameter.alarmDecreaseSpeed*Time.deltaTime;
        _manager.transform.LookAt(_targetPosition);
        _manager.transform.position = Vector3.MoveTowards(_manager.transform.position, _targetPosition.position,
            _parameter.chaseSpeed * Time.deltaTime);
        if (Vector3.Distance(_manager.transform.position,_targetPosition.position)<0.5f)
        {
            ChangeChaseTarget();
        }
    }
    //寻路逻辑:发现玩家后，获取玩家坐标，在玩家与该敌人之间进行一次寻路，将最近的一个寻路点添加到下一个寻路目标点，每次到达寻路目标点再次获取玩家坐标，进行一次寻路
    //直到接触到玩家跳出状态才会停止寻路
    
    
    
    //需要做将AStarNode转换为坐标的工作,大概在MapInfoController中实现
    //进入状态时调用，只调用一次
    //如果状态更改记得取消
    //不能直接传坐标
    private void ChangeChaseTarget()
    {
        Debug.Log($"玩家位置:{_manager.parameter.playerTarget.position}");
        //如果寻路出了问题，就回来看看这里
        _targetPosition = MapInfoController.AStarNodeToTransforms(AStarManager.Instance.FindPath(
            new Vector2(Floor(_manager.transform.position.x), Floor(_manager.transform.position.z)),
            new Vector2(Floor(_manager.parameter.playerTarget.transform.position.x),
                Floor(_manager.parameter.playerTarget.transform.position.z))))[1];
        
        _targetPosition.position = new Vector3(_targetPosition.position.x, _manager.transform.position.y,
            _targetPosition.position.z);
    }
    
    private float Floor(float num)
    {
        if (num > 0) return (int)num + 0.5f;
        return (int)num - 0.5f;
    }
 
    public void OnExit()
    {
        
    }

    public void TriggerCheck()
    {
        if (_parameter.TriggerListener.IsCaughtPlayer)
        {
            _manager.TransitionState(StateType.Attack);
        }
    }
}
