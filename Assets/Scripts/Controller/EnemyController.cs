using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 敌人控制类，主要与追逐型敌人的范围吸引有关
/// 在每个场景中都要单独创建敌人控制器并完成敌人的绑定
/// </summary>
public class EnemyController : MonoBehaviour
{
    public List<FSM> enemies; //在场景中完成绑定
    private static List<FSM> _staticEnemies;//注意列表的刷新和清除

    void Start()
    {
        foreach (var enemy in enemies)
        {
            enemy.parameter.EnemyController = this;
        }
        _staticEnemies = enemies;//共享同一块内存
    }

    void Update()
    {
    }

    //进行吸引范围的计算，返回距离内的敌人列表
    public List<FSM> GetEnemies(FSM attractiveEnemy, float distance)
    {
        List<FSM> enemiesInRange = new List<FSM>();
        foreach (var enemy in enemies)
        {
            if (Vector3.Distance(enemy.transform.position,attractiveEnemy.transform.position)<=distance)
            {
                enemiesInRange.Add(enemy);
            }
        }
        return enemiesInRange;
    }
    
    public static List<FSM> GetEnemies(Vector3 targetPosition, float distance)
    {
        List<FSM> enemiesInRange = new List<FSM>();
        foreach (var enemy in _staticEnemies)
        {
            if (Vector3.Distance(enemy.transform.position,targetPosition)<=distance)
            {
                enemiesInRange.Add(enemy);
            }
        }
        return enemiesInRange;
    }
}