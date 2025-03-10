using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody _rb;
    
    [SerializeField]private float defaultVelocity;
    [SerializeField]private float bonusVelocity;
    [SerializeField]private float acceleration;
    [SerializeField]private float cooldownTime;
    [SerializeField]private float runDuaration;
    
    private float _curMaxVelocity;
    private bool _canRun = true;
    private float _cooldownTimer;
    private float _runTimer;
    private bool _isRunning = false;
    private bool _isCoolingDown = false;
    private Transform _cameraDirectionTransform;
    private Vector3 _cameraLeft;
    private Vector3 _cameraRight;
    private void Start()
    {
        _rb = gameObject.GetComponent<Rigidbody>();
        _cameraDirectionTransform = transform.GetChild(0).transform;
        _cameraLeft = Vector3.Cross(_cameraDirectionTransform.forward, new Vector3(0f, 1f, 0f)).normalized;
        _cameraRight = -Vector3.Cross(_cameraDirectionTransform.forward, new Vector3(0f, 1f, 0f)).normalized;
        _curMaxVelocity = defaultVelocity;
    }
    //镜头移动时需要进行调用来更新移动方向
    public void OnChangedCameraDirection()
    {
        _cameraLeft = Vector3.Cross(_cameraDirectionTransform.forward, new Vector3(0f, 1f, 0f)).normalized;
        _cameraRight = -Vector3.Cross(_cameraDirectionTransform.forward, new Vector3(0f, 1f, 0f)).normalized;
        // Debug.Log("Left向量:"+_cameraLeft);
        // Debug.Log("Right向量:"+_cameraRight);
    }

    public void Broadcast()
    {
        Debug.Log("Left向量:"+_cameraLeft);
        Debug.Log("Right向量:"+_cameraRight);
    }

    void Update()
    {
        if (!(_rb.velocity.sqrMagnitude>=_curMaxVelocity))
        {
            if (Input.GetKey(KeyCode.W))
            {
                if (_rb.velocity != _cameraDirectionTransform.forward.normalized * _curMaxVelocity)
                    _rb.velocity += _cameraDirectionTransform.forward.normalized * acceleration;
            }

            if (Input.GetKey(KeyCode.S))
            {
                if (_rb.velocity != -_cameraDirectionTransform.forward.normalized * _curMaxVelocity)
                    _rb.velocity -= _cameraDirectionTransform.forward.normalized * acceleration;
            }

            if (Input.GetKey(KeyCode.A))
            {
                if (_rb.velocity != _cameraLeft * _curMaxVelocity)
                    _rb.velocity += _cameraLeft * acceleration;
            }

            if (Input.GetKey(KeyCode.D))
            {
                if (_rb.velocity != _cameraRight * _curMaxVelocity)
                    _rb.velocity += _cameraRight * acceleration;
            }
        }

        //检测到按下左shift键，在合适的状态下进入跑步状态
        if (Input.GetKeyDown(KeyCode.LeftShift)&&_canRun)
        {
            //移动速度变化
            
            //todo...
            _curMaxVelocity += bonusVelocity;
            _canRun = false;
            _runTimer = runDuaration;
            _isRunning = true;
        }
        //进入跑步状态后对跑步持续时间进行计时
        if (_isRunning)
        {
            _runTimer-=Time.deltaTime;
            if(_runTimer<=0)
            {
                //移动速度变化
                
                //todo...
                _curMaxVelocity = defaultVelocity;
                _isRunning = false;
                _cooldownTimer = cooldownTime;
                _isCoolingDown = true;
            }
        }
        //进入冷却状态后对冷却时间进行计时
        if (_isCoolingDown)
        {
            _cooldownTimer-=Time.deltaTime;
            if (_cooldownTimer<=0)
            {
                _isCoolingDown = false;
                _canRun = true;
            }
        }
    }
}
