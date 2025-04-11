using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

public class PmcPlayerController : MonoBehaviour
{
    [SerializeField] private float nomalSpeed;
    [SerializeField] private float runSpeed;
    [SerializeField] private float reduceSpeed;

    [SerializeField] private float coolDownTime;
    [SerializeField] private float runDuaration;

    [SerializeField] private float smoothSpeed;
    [SerializeField] private float smoothAngle;

    private CharacterController cc;
    private float _curMaxVelocity;
    private bool _canRun = true;
    private float _cooldownTimer;
    private float _runTimer;
    private bool _isRunning = false;
    private bool _isCoolingDown = false;
    private Transform _cameraDirectionTransform;
    private float _cameraX;
    private Transform _playerModel;


    [NonSerialized] public bool IsMoveAble = true;
    [NonSerialized] public bool IsRunable = true;
    [NonSerialized] public bool IsDisguised = false;



    // Start is called before the first frame update
    void Start()
    {
        _cameraDirectionTransform = transform.Find("Main Camera");
        cc = GetComponent<CharacterController>();

    }

    // Update is called once per frame
    void Update()
    {
        RunningChange();
        PlayerMove();
    }

    public void PlayerMove()
    {
        Vector3 move = GetInput();
        float speed = setSpeed();
        move = move * speed * Time.deltaTime;
        if (move != Vector3.zero) { SetPlayerRotation(); }
        cc.Move(move);
    }

    public Vector3 GetInput()
    {
        Vector3 input = new Vector3();
        if (Input.GetKey(KeyCode.W))
        {
            input += (transform.forward);
        }
        if (Input.GetKey(KeyCode.S))
        {
            input -= (transform.forward);
        }
        if (Input.GetKey(KeyCode.D))
        {
            input += (transform.right);
        }
        if (Input.GetKey(KeyCode.A))
        {
            input -= (transform.right);
        }
        return input;
    }

    public float setSpeed()
    {   
        if (!IsMoveAble)
        {
            return 0;
        }

        if (_isRunning)
        {
            return runSpeed;
        }
        if (IsDisguised)
        {
            return reduceSpeed;
        }

        return nomalSpeed;
    }

    public void SetPlayerRotation()
    {
        float playerY = transform.eulerAngles.y;
        float cameraY =_cameraDirectionTransform.eulerAngles.y;
        float angleDiff = Mathf.Abs(Mathf.DeltaAngle(playerY, cameraY));
        Quaternion finalRotation = Quaternion.Euler(0, _cameraDirectionTransform.eulerAngles.y, 0);
        if (angleDiff > smoothAngle)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, finalRotation, smoothSpeed*Time.deltaTime);
        }
        else
        {
            transform.rotation = finalRotation;
        }
    }

    public void RunningChange()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && _canRun && IsMoveAble)//跑步启动
        {
            _canRun = false;
            _runTimer = runDuaration;
            _isRunning = true;
        }
        if (_isRunning)//正在跑步，检测剩余时间
        {
            _runTimer -= Time.deltaTime;
            if (_runTimer <= 0)
            {
                _isRunning = false;
                _cooldownTimer = coolDownTime;
                _isCoolingDown = true;
            }
        }
        if (_isCoolingDown)//正在冷却，检测剩余时间
        {
            _cooldownTimer -= Time.deltaTime;
            if (_cooldownTimer <= 0)
            {
                _isCoolingDown = false;
                _canRun = true;
            }
        }
    }

    public void DisguiseChange()
    {
        IsDisguised = !IsDisguised;

        _canRun = !IsDisguised;
    }

    public void HideChange()
    {
        IsMoveAble = !IsMoveAble;
    }

}
