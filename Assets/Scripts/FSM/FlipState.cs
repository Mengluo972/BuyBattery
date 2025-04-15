using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
/// <summary>
/// 通用转向状态
/// </summary>
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
        _rayCastTest.IsPatrolTracing = true;
        _rayCastTest.IsChaseTracing = false;
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
                    switch (_parameter.enemyType)
                    {
                        case EnemyType.PatrolEnemy:
                            _manager.TransitionState(StateType.Patrol);
                            break;
                        case EnemyType.AttractEnemy:
                            _manager.TransitionState(StateType.AttractivePatrol);
                            break;
                    }
                    
                }
                break;
            default:
                Debug.Log("FlipState中的_flipWaitStage值错误");
                _manager.TransitionState(StateType.Idle);
                break;
        }

        if (!_rayCastTest.IsPlayerDetected) return;//如果没有发现玩家，就不去执行增长警戒值的操作
        if(_parameter.alarmValue>=_parameter.alarmMaxValue)
        {
            _parameter.LastPatrolPoint = _manager.transform.position;
            Debug.Log(_manager.gameObject.name + "发现玩家，进入追逐状态");
            if (_parameter.enemyType==EnemyType.AttractEnemy)
            {
                Debug.Log("当前敌人为追逐型敌人，正在吸引周围的敌人前来追逐玩家");
                List<FSM> list = _parameter.EnemyController.GetEnemies(_manager,_parameter.attractDistance);
                foreach (var fsm in list)
                {
                    fsm.parameter.alarmValue = fsm.parameter.alarmMaxValue;
                    fsm.TransitionState(StateType.Chase);
                }

                return;
            }
            _manager.TransitionState(StateType.Chase);
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
        _rayCastTest.IsPatrolTracing = false;
        // Debug.Log("退出转向状态");
    }

    public void TriggerCheck()
    {
        if (_parameter.TriggerListener.IsCaughtPlayer)
        {
            _parameter.alarmValue = 0;
            _manager.TransitionState(StateType.Attack);
        }
    }
}
