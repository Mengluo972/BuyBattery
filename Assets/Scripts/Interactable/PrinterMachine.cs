using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PrinterMachine : MonoBehaviour,IInteractable
{
    [Header("吸引范围")]
    public float attractDistance = 5f;
    [Header("吸引时间")]
    public float attractTime = 3f;
    
    private Transform _playerCache;
    private bool _inAttract;

    public void TriggerAction()
    {
        if (_inAttract)
        {
            print("打印机已在吸引中");
            return;
        }
        StartCoroutine(AttractEnemiesInRange());
    }

    private IEnumerator AttractEnemiesInRange()
    {
        _inAttract = true;
        List<FSM> list =  EnemyController.GetEnemies(transform.position,attractDistance);
        List<float> alarmDecreaseSpeeds = new List<float>();
        _playerCache = list.First().parameter.playerTarget;
        foreach (var enemyInRange in list)
        {
            enemyInRange.parameter.playerTarget = transform;
            alarmDecreaseSpeeds.Add(enemyInRange.parameter.alarmDecreaseSpeed);
            enemyInRange.parameter.alarmDecreaseSpeed = 0;
            enemyInRange.parameter.alarmValue = enemyInRange.parameter.alarmMaxValue;
            enemyInRange.parameter.LastPatrolPoint = enemyInRange.transform.position;
            enemyInRange.TransitionState(StateType.Chase);
        }
        print($"等待{attractTime}秒");
        yield return new WaitForSeconds(attractTime);
        print("等待结束");
        foreach (var enemy in list)
        {
            enemy.parameter.alarmDecreaseSpeed = alarmDecreaseSpeeds[list.IndexOf(enemy)];
            enemy.parameter.alarmValue = 0;
            enemy.TransitionState(StateType.EndingChase);
            enemy.parameter.playerTarget = _playerCache;
        }
        _inAttract = false;
    }

    public void inTriggerAnimation(bool b)
    {
        print("进入打印机交互范围");
    }
}
