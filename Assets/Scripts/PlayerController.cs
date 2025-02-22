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
    private bool _canRun = true;
    [SerializeField]private float cooldownTime;
    private float _cooldownTimer;
    [SerializeField]private float runDuaration;
    private float _runTimer;
    private bool _isRunning = false;
    private bool _isCoolingDown = false;
    private void Start()
    {
        _rb = gameObject.GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            // transform.position += new Vector3(0, 0, 0.1f);
        }
        if (Input.GetKey(KeyCode.S))
        {
            // transform.position += new Vector3(0, 0, -0.1f);
        }
        if (Input.GetKey(KeyCode.A))
        {
            // transform.position += new Vector3(-0.1f, 0, 0);
        }
        if (Input.GetKey(KeyCode.D))
        {
            // transform.position += new Vector3(0.1f, 0, 0);
        }
        //检测到按下左shift键，在合适的状态下进入跑步状态
        if (Input.GetKeyDown(KeyCode.LeftShift)&&_canRun)
        {
            //移动速度变化
            
            //todo...
            
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
