using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class Obstacle : MonoBehaviour,IInteractable
{
    [Header("物品初始位置")]public Transform originPosition;
    [Header("物品移动后位置")] public Transform targetPosition;
    [Header("物品的移动时间")]public float time;
    private bool _isMoving = false;
    private int _place = 0;
    private BoxCollider _collider;
    private GameObject buttonTips;
    
    void Start()
    {
        _collider = GetComponent<BoxCollider>();
        _collider.isTrigger = true;
        buttonTips = transform.Find("ButtonTips").gameObject;
        buttonTips.SetActive(false);
    }

    void Update()
    {
        
    }

    public void TriggerAction()
    {
        if (!_isMoving)
        {
            SoundManager.Instance.PlaySFX("screen",1,6);
            StartCoroutine(MoveToTarget());
        }
    }
    
    private IEnumerator MoveToTarget()
    {
        _isMoving = true;
        if (_place==0)
        {
            _place = 1;
            transform.DOMove(targetPosition.position, time);
            yield return new WaitForSeconds(time);
            _collider.isTrigger = false;
            _collider.providesContacts = true;
        }
        else
        {
            _place = 0;
            transform.DOMove(originPosition.position, time);
            yield return new WaitForSeconds(time);
            _collider.isTrigger = true;
            _collider.providesContacts = false;
        }
        _isMoving = false;
    }

    public void inTriggerAnimation(bool b)
    {
        AnimateOn();
        inTrigger = b;
    }

    private bool inTrigger;
    private async UniTaskVoid AnimateOn()
    {
        inTrigger = true;
        buttonTips.SetActive(true);
        ChangeTip.ChangePlayTips("- 此处禁止通行！ -");

        await UniTask.WaitUntil(() => !inTrigger);

        buttonTips.SetActive(false);
        ChangeTip.ChangePlayTips("");

    }
}
