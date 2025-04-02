using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 找人状态
/// </summary>
public class FindState : IState
{
    private FSM _manager;
    private Parameter _parameter;
    private Queue<Vector3> _chaseQueue = new Queue<Vector3>();
    private Vector3 _chaseTarget = Vector3.zero;//如果出现卡顿就自定义向量类型
    private RayCastTest _rayCastTest;
    [SerializeField]private float chaseCooldownTime = 0.5f;//追逐冷却时间
    private float chaseCooldownTimer = 0f;

    public FindState(FSM manager)
    {
        _manager = manager;
        _parameter = manager.parameter;
        _rayCastTest = manager.RayCastTest;
    }

    public void OnEnter()
    {
        Debug.Log("进入找人状态");
        _rayCastTest.IsChaseTracing = true;
        _rayCastTest.IsPatrolTracing = false;
        ReloadChaseList();
    }

    public void OnUpdate()
    {
        if (Vector3.Distance(_manager.transform.position,_chaseTarget)<0.5f)
        {
            NextChaseTarget();
            Debug.Log("进入下一个寻找点");
        }
        //做玩家是否进入逮人距离的判断

        if (ChaseDistanceCheck())
        {
            Debug.Log("通过ChaseDistanceCheck进入逮人状态");
            _manager.TransitionState(StateType.Chase);
        }
        
        if (_parameter.alarmValue<=0)
        {
            _parameter.alarmValue = 0;
            _manager.TransitionState(StateType.EndingChase);
        }
        _parameter.alarmValue-= _parameter.alarmDecreaseSpeed*Time.deltaTime;
        _manager.transform.LookAt(_chaseTarget);
        _manager.transform.position = Vector3.MoveTowards(_manager.transform.position, _chaseTarget,
            _parameter.chaseSpeed * Time.deltaTime);
        chaseCooldownTimer += Time.deltaTime;
        if (chaseCooldownTimer >= chaseCooldownTime)
        {
            chaseCooldownTimer = 0f;
            ReloadChaseList();
        }
    }
    
    private bool ChaseDistanceCheck()
    {
        if (!_rayCastTest.IsPlayerDetected) return false;
        return MapInfoController.BarrierCheck(AStarManager.Instance.FindPath(
            new Vector2(Floor(_manager.transform.position.x), Floor(_manager.transform.position.z)),
            new Vector2(Floor(_manager.parameter.playerTarget.transform.position.x),
                Floor(_manager.parameter.playerTarget.transform.position.z))));
    }
    private void ReloadChaseList()
    {
        // Debug.Log($"玩家位置:{_manager.parameter.playerTarget.position}");
        //如果寻路出了问题，就回来看看这里
        // _targetPosition = MapInfoController.AStarNodeToTransforms(AStarManager.Instance.FindPath(
        //     new Vector2(Floor(_manager.transform.position.x), Floor(_manager.transform.position.z)),
        //     new Vector2(Floor(_manager.parameter.playerTarget.transform.position.x),
        //         Floor(_manager.parameter.playerTarget.transform.position.z))))[1];
        //
        // _targetPosition.position = new Vector3(_targetPosition.position.x, _manager.transform.position.y,
        //     _targetPosition.position.z);

        List<AStarNode> newList = AStarManager.Instance.FindPath(
            new Vector2(Floor(_manager.transform.position.x), Floor(_manager.transform.position.z)),
            new Vector2(Floor(_manager.parameter.playerTarget.transform.position.x),
                Floor(_manager.parameter.playerTarget.transform.position.z)));
        List<Transform> newTransformList = MapInfoController.AStarNodeToTransforms(newList);
        Queue<Vector3> newTargetQueue = new Queue<Vector3>();
        foreach (var transform in newTransformList)
        {
            newTargetQueue.Enqueue(new Vector3(transform.position.x, _manager.transform.position.y, transform.position.y));
            
        }
        _chaseQueue = newTargetQueue;
    }

    private void NextChaseTarget()
    {
        if (_chaseQueue.Count<=0)
        {
            _manager.TransitionState(StateType.EndingChase);
            Debug.Log("失去目标,进入结束追逐状态");
        }
        _chaseTarget = _chaseQueue.Dequeue();
    }
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
