using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartrolState : IState
{
    private FSM _manager;
    private Parameter _parameter;
    private RayCastTest _rayCastTest;
    private Rigidbody _rigidbody;//默认FSM组件下手动挂载了Rigidbody组件
    public PartrolState(FSM manager)
    {
        _manager = manager;
        _parameter = manager.parameter;
        _rayCastTest = manager.RayCastTest;
        _rigidbody = manager.GetComponent<Rigidbody>();
    }
    public void OnEnter()
    {
        _rayCastTest.IsTracing = true;
        Debug.Log("进入巡逻状态");
    }

    public void OnUpdate()
    {
        if(Vector3.Distance(_manager.transform.position,_parameter.partrolPoints[_parameter.patrolIndex].position)<0.5f)
        {
            _parameter.patrolIndex++;//一旦到达就立刻增加索引值，在转向状态中不再额外增加
            if(_parameter.patrolIndex>=_parameter.partrolPoints.Length)//越界检测
            {
                _parameter.patrolIndex = 0;
            }
            _manager.TransitionState(StateType.Flip);
        }
        _manager.transform.position = Vector3.MoveTowards(_manager.transform.position,
            _parameter.partrolPoints[_parameter.patrolIndex].position, _parameter.moveSpeed * Time.deltaTime);
        if (!_rayCastTest.IsPlayerDetected)return;
        if(_parameter.alarmValue>=_parameter.alarmMaxValue)
        {
            _manager.TransitionState(StateType.Chase);
            Debug.Log(_manager.gameObject.name + "发现玩家，进入追逐状态");
            return;
        }
        _parameter.alarmValue += _parameter.alarmAccelerationSpeed*Time.deltaTime;
    }

    public void OnExit()
    {
        _rayCastTest.IsTracing = false;
        Debug.Log("退出巡逻状态");
    }
}
