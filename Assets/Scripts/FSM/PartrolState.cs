using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PartrolState : IState
{
    private FSM _manager;
    private Parameter _parameter;
    private RayCastTest _rayCastTest;
    private Rigidbody _rigidbody;//默认FSM组件下手动挂载了Rigidbody组件
    private NavMeshAgent _navMeshAgent;
    public PartrolState(FSM manager)
    {
        _manager = manager;
        _parameter = manager.parameter;
        _rayCastTest = manager.RayCastTest;
        _rigidbody = manager.GetComponent<Rigidbody>();
        _navMeshAgent = manager.parameter.NavMeshAgent;
    }
    public void OnEnter()
    {
        _rayCastTest.IsPatrolTracing = true;
        _rayCastTest.IsChaseTracing = false;
        switch (_parameter.enemyAnimator)
        {
            case EnemyAnimator.colleague:
                _parameter.animator.Play("enemy_colleague@walk");
                break;
            case EnemyAnimator.cat:
                _parameter.animator.Play("metarig_Cat|walk");
                break;
            case EnemyAnimator.boss:
                _parameter.animator.Play("enemy_boss@Walk");
                break;
            case EnemyAnimator.maneger:
                _parameter.animator.Play("enemy_manager@walk");
                break;
            case EnemyAnimator.guard:
                _parameter.animator.Play("enemy_guard@run");
                break;
        }
    }

    public void OnUpdate()
    {
        if(Vector3.Distance(_manager.transform.position,_parameter.partrolPoints[_parameter.PatrolIndex].position)<=_navMeshAgent.stoppingDistance)
        {
            _parameter.PatrolIndex++;//一旦到达就立刻增加索引值，在转向状态中不再额外增加
            if(_parameter.PatrolIndex>=_parameter.partrolPoints.Length)//越界检测
            {
                _parameter.PatrolIndex = 0;
            }
            _manager.TransitionState(StateType.Flip);
            return;
        }
        _navMeshAgent.SetDestination(_parameter.partrolPoints[_parameter.PatrolIndex].position);
        if (!_rayCastTest.IsPlayerDetected) return;//如果没有发现玩家，就不去执行增长警戒值的操作
        if(_parameter.alarmValue>=_parameter.alarmMaxValue)
        {
            _parameter.LastPatrolPoint = _manager.transform.position;
            Debug.Log(_manager.gameObject.name + "发现玩家，进入追逐状态");
            switch (_parameter.enemyType)
            {
                case EnemyType.StunEnemy:
                    _manager.TransitionState(StateType.Stun);
                    break;
                default:
                    _manager.TransitionState(StateType.Chase);
                    break;
            }
            
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
        if (_parameter.TriggerListener.PlayerIsInvincible)
        {
            return;    
        }
        if (_parameter.TriggerListener.IsCaughtPlayer)
        {
            _parameter.alarmValue = 0;
            _manager.TransitionState(StateType.Attack);
        }
    }
}
