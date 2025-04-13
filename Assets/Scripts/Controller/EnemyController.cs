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

    void Start()
    {
        enemies = new List<FSM>();
        foreach (var enemy in enemies)
        {
            enemy.parameter.EnemyController = this;
        }
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
}