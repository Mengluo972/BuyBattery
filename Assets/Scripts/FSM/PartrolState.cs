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
        _rayCastTest.IsPatrolTracing = true;
        _rayCastTest.IsChaseTracing = false;
    }

    public void OnUpdate()
    {
        if(Vector3.Distance(_manager.transform.position,_parameter.partrolPoints[_parameter.PatrolIndex].position)<0.5f)
        {
            _parameter.PatrolIndex++;//一旦到达就立刻增加索引值，在转向状态中不再额外增加
            if(_parameter.PatrolIndex>=_parameter.partrolPoints.Length)//越界检测
            {
                _parameter.PatrolIndex = 0;
            }
            _manager.TransitionState(StateType.Flip);
        }
        _manager.transform.position = Vector3.MoveTowards(_manager.transform.position,
            _parameter.partrolPoints[_parameter.PatrolIndex].position, _parameter.moveSpeed * Time.deltaTime);
        if (!_rayCastTest.IsPlayerDetected) return;//如果没有发现玩家，就不去执行增长警戒值的操作
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
        _rayCastTest.IsPatrolTracing = false;
    }

    public void TriggerCheck()
    {
        if (_parameter.TriggerListener.IsCaughtPlayer)
        {
            _manager.TransitionState(StateType.Attack);
        }
    }
}
