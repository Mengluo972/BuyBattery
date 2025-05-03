using System.Collections;
using System.Collections.Generic;
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
    
    void Start()
    {
        _collider = GetComponent<BoxCollider>();
        _collider.isTrigger = true;
    }

    void Update()
    {
        
    }

    public void TriggerAction()
    {
        if (!_isMoving)
        {
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
        
    }
}
