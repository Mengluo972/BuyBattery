using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class ViewCollider : MonoBehaviour
{
    private FSM _fsm;
    private MeshCollider _meshCollider;
    [SerializeField] private float alertValue;
    private bool _isEnter;
    [SerializeField] private float alertAccelerateValue;
    private bool _isChasing;
    void Start()
    {
        _fsm = gameObject.GetComponentInParent<FSM>();
        _isEnter = false;
        alertValue = 0f;
        _isChasing = false;
    }

    void Update()
    {
        if (alertValue >= 1f)
        {
            _fsm.TransitionState(StateType.Chase);
        }
        else
        {
            if (_isEnter)
            {
                alertValue += alertAccelerateValue;
                _isChasing = true;
            }
            else
            {
                alertValue -= alertAccelerateValue;
            }
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        _isEnter = true;
    }

    private void OnTriggerStay(Collider other)
    {
        throw new NotImplementedException();
    }

    private void OnTriggerExit(Collider other)
    {
        _isEnter = false;
    }
}
