using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class PrinterMachine : MonoBehaviour,IInteractable
{
    [Header("吸引范围")]
    public float attractDistance = 5f;
    [Header("吸引时间")]
    public float attractTime = 3f;
    
    private Transform _playerCache;
    private bool _inAttract;
    private GameObject buttonTips;

    public void Start()
    {
        buttonTips = transform.Find("ButtonTips").gameObject;
        buttonTips.SetActive(false);
    }

    public void TriggerAction()
    {
        if (_inAttract)
        {
            print("打印机已在吸引中");
            return;
        }
        SoundManager.Instance.PlaySFX("SFX_Props_printer");
        StartCoroutine(AttractEnemiesInRange());
    }

    private IEnumerator AttractEnemiesInRange()
    {
        PrinterNoise Particle = transform.GetChild(0).GetComponent<PrinterNoise>();
        Particle.printerOn();
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
        Particle.printerOff();
    }

    public void inTriggerAnimation(bool b)
    {
        print("进入打印机交互范围");
        AnimateOn();
        inTrigger = b;
    }

    private bool inTrigger;
    private async UniTaskVoid AnimateOn()
    {
        inTrigger = true;
        buttonTips.SetActive(true);
        ChangeTip.ChangePlayTips("- 发出噪音把同事吸引过来 -");

        await UniTask.WaitUntil(() => !inTrigger);

        buttonTips.SetActive(false);
        ChangeTip.ChangePlayTips("");

    }

}
