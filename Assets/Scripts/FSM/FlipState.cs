using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class FlipState : IState
{
    private FSM _manager;
    private Parameter _parameter;
    private float _timer;
    private RayCastTest _rayCastTest;
    private int _flipWaitStage;//一共4阶段，值为0时等待第一段，值为1时开始转向，值为2时进行转向，值为3时等待第二段
    
    public FlipState(FSM manager)
    {
        _manager = manager;
        _parameter = manager.parameter;
        _flipWaitStage = 0;
        _rayCastTest = manager.GetComponent<RayCastTest>();
    }
    
    public void OnEnter()
    {
        _flipWaitStage = 0;
        _rayCastTest.IsTracing = true;
        // Debug.Log("进入转向状态");
    }

    public void OnUpdate()
    {
        switch (_flipWaitStage)
        {
            case 0:
                _timer += Time.deltaTime;
                if (_timer >= _parameter.flipWaitTimeBefore)
                {
                    _timer = 0;
                    _flipWaitStage = 1;
                }
                break;
            case 1:
                _flipWaitStage = 2;
                _manager.transform
                    .DOLookAt(_parameter.partrolPoints[_parameter.PatrolIndex].position, _parameter.flipTime)
                    .OnComplete(() => _flipWaitStage = 3);
                break;
            case 2://该阶段只等待回调
                break;
            case 3:
                _timer += Time.deltaTime;
                if (_timer>=_parameter.flipWaitTimeAfter)
                {
                    _timer = 0;
                    _manager.TransitionState(StateType.Patrol);
                }
                break;
            default:
                Debug.Log("FlipState中的_flipWaitStage值错误");
                _manager.TransitionState(StateType.Idle);
                break;
        }
        if (!_rayCastTest.IsPlayerDetected)return;
        if(_parameter.alarmValue>=_parameter.alarmMaxValue)
        {
            _manager.TransitionState(StateType.Chase);
            Debug.Log(_manager.gameObject.name + "发现玩家，进入追逐状态");
            return;
        }
        _parameter.alarmValue += _parameter.alarmAccelerationSpeed*Time.deltaTime;
    }
    
    public static Vector3 DirectionCaculate(Vector3 startPos,Vector3 endPos)
    {
        Vector3 direction = endPos - startPos;
        return direction.normalized;
    }

    public void OnExit()
    {
        _rayCastTest.IsTracing = false;
        // Debug.Log("退出转向状态");
    }

    public void TriggerCheck()
    {
        if (_parameter.TriggerListener.IsCaughtPlayer)
        {
            _manager.TransitionState(StateType.Attack);
        }
    }
}
