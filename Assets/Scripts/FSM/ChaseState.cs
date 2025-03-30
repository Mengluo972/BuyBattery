using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class ChaseState : IState
{
    private FSM _manager;
    private Parameter _parameter;
    private TriggerListener _triggerListener;
    private Transform _targetPosition;
    
    public ChaseState(FSM manager)
    {
        _manager = manager;
        _parameter = manager.parameter;
        _triggerListener = manager.transform.GetChild(0).GetComponent<TriggerListener>();
    }
    public void OnEnter()
    {
        // Debug.Log("进入追逐状态");
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
    }
    //需要做将AStarNode转换为坐标的工作,大概在MapInfoController中实现
    //进入状态时调用，只调用一次
    //如果状态更改记得取消
    private async void ChangeChaseTarget(List<AStarNode> path)
    {
        if (path.Count<=0) return;

        for (int i = 0; i < path.Count; i++)
        {
            // await UniTask.WaitUntil(()=>Vector3.Distance(_manager.transform.position,path[i])<0.5f);
        }
        
        
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
