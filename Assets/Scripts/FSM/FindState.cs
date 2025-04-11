using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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
    [SerializeField]private float chaseCooldownTime = 2f;//追逐冷却时间
    private float chaseCooldownTimer = 0f;
    private NavMeshAgent _navMeshAgent;
    

    public FindState(FSM manager)
    {
        _manager = manager;
        _parameter = manager.parameter;
        _rayCastTest = manager.RayCastTest;
        _navMeshAgent = manager.GetComponent<NavMeshAgent>();
        _navMeshAgent.stoppingDistance = 1.7f;//只能在一个状态的初始化中进行更改，否则会出现数据混乱
        _navMeshAgent.acceleration = 40f;
    }

    // public void OnEnter()
    // {
    //     Debug.Log("进入找人状态");
    //     _rayCastTest.IsChaseTracing = true;
    //     _rayCastTest.IsPatrolTracing = false;
    //     ReloadChaseList();
    //     NextChaseTarget();
    //     Debug.Log("当前的追逐点数量为" + _chaseQueue.Count+"当前的追逐点为" + _chaseTarget);
    //     // foreach (var vector3 in _chaseQueue)
    //     // {
    //     //     Debug.Log(vector3);
    //     // }
    // }

    public void OnEnter()
    {
        _rayCastTest.IsChaseTracing = true;
        _rayCastTest.IsPatrolTracing = false;
    }

    // public void OnUpdate()
    // {
    //     if (chaseCooldownTimer >= chaseCooldownTime)
    //     {
    //         chaseCooldownTimer = 0f;
    //         ReloadChaseList();
    //         Debug.Log("重新加载追逐点");
    //     }
    //     if (Vector3.Distance(_manager.transform.position,_chaseTarget)<0.5f)
    //     {
    //         NextChaseTarget();
    //         // Debug.Log("进入下一个寻找点");
    //         // Debug.Log($"当前追逐点为{_chaseTarget}");
    //     }
    //     //做玩家是否进入逮人距离的判断
    //
    //     // if (ChaseDistanceCheck())
    //     // {
    //     //     Debug.Log("通过ChaseDistanceCheck进入逮人状态");
    //     //     _manager.TransitionState(StateType.Chase);
    //     // }
    //     if (Vector3.Distance(_manager.transform.position,_parameter.playerTarget.position)<_rayCastTest.chaseDistance)
    //     {
    //         Debug.Log("玩家距离过近，进入逮人状态");
    //         _manager.TransitionState(StateType.Chase);
    //     }
    //     
    //     if (_parameter.alarmValue<=0)
    //     {
    //         _parameter.alarmValue = 0;
    //         _manager.TransitionState(StateType.EndingChase);
    //     }
    //     _parameter.alarmValue-= _parameter.alarmDecreaseSpeed*Time.deltaTime;
    //     _manager.transform.LookAt(_parameter.playerTarget);
    //     _manager.transform.position = Vector3.MoveTowards(_manager.transform.position, _chaseTarget,
    //         _parameter.chaseSpeed * Time.deltaTime);
    //     chaseCooldownTimer += Time.deltaTime;
    //     
    // }

    public void OnUpdate()
    {
        if (Vector3.Distance(_manager.transform.position,_parameter.playerTarget.position)<_rayCastTest.chaseDistance)
        {
            Debug.Log("玩家距离过近，进入逮人状态");
            _manager.TransitionState(StateType.Chase);
        }
        if (_parameter.alarmValue<=0)
        {
            _parameter.alarmValue = 0;
            _manager.TransitionState(StateType.EndingChase);
        }
        _parameter.alarmValue-= _parameter.alarmDecreaseSpeed*Time.deltaTime;
        _manager.transform.LookAt(_parameter.playerTarget);
        _navMeshAgent.SetDestination(_parameter.playerTarget.position);
    }
    public void OnExit()
    {
        _rayCastTest.IsChaseTracing = false;
    }

    public void TriggerCheck()
    {
        if (_parameter.TriggerListener.IsCaughtPlayer)
        {
            _parameter.alarmValue = 0;
            _manager.TransitionState(StateType.Attack);
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
        Vector2 startPos = new Vector2(
            Floor(_manager.transform.position.x - MapInfoController.originPosition.position.x),
            Floor(_manager.transform.position.z - MapInfoController.originPosition.position.z));
        Vector2 endPos = new Vector2(
            Floor(_manager.parameter.playerTarget.transform.position.x - MapInfoController.originPosition.position.x),
            Floor(_manager.parameter.playerTarget.transform.position.z - MapInfoController.originPosition.position.z));
        List<AStarNode> newList = AStarManager.Instance.FindPath(startPos,endPos);
        Debug.Log(newList.Count==0?"寻路失败":"寻路成功");
        List<Vector3> newTransformList = MapInfoController.AStarNodeToTransforms(newList);
        Queue<Vector3> newTargetQueue = new Queue<Vector3>();
        foreach (var vector3 in newTransformList)
        {
            newTargetQueue.Enqueue(new Vector3(vector3.x, _manager.transform.position.y, vector3.z));
            
        }
        newTargetQueue.Dequeue();//出掉一个队列节点试图减少回头的情况
        _chaseQueue = newTargetQueue;
    }

    private void NextChaseTarget()
    {
        if (_chaseQueue.Count<=0)
        {
            _manager.TransitionState(StateType.EndingChase);
            Debug.Log("失去目标,进入结束追逐状态");
        }

        Vector3 rawData = new();
        if (!_chaseQueue.TryDequeue(out rawData))
        {
            Debug.Log("队列为空");
            return;
        }
        Debug.Log($"节点的信息:x:{rawData.x},y:{rawData.y},z:{rawData.z}");
        _chaseTarget = new Vector3(rawData.x, rawData.y, rawData.z);
    }
    private float Floor(float num)
    {
        // if (num > 0) return (int)num + 0.5f;
        // return (int)num - 0.5f;
        if (num > 0) return (int)num;
        return (int)num - 1;
    }

    
}
