using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Cysharp.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class PmcPlayerController : MonoBehaviour
{
    [Header("拖入物体")]
    [SerializeField] private GameObject HeadBoxItem;
    [SerializeField] private GameObject BigBoxItem;

    [Header("速度设置")]
    [SerializeField] private float nomalSpeed;

    [Header("交互键位")]
    [SerializeField] private KeyCode InterKey;

    [Header("疾跑相关")]
    [SerializeField] private float coolDownTime;
    [SerializeField] private float runDuaration;
    [SerializeField] private float runSpeed;

    [Header("伪装相关")]
    [SerializeField] private float DisguiseDuaration;
    [SerializeField] private float reduceSpeed;

    [Header("保护相关")]
    [SerializeField] private float safeSpeed;

    [Header("平滑转向相关")]
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
    private BoxCollider _collider;
    private Animator _animator;


    [NonSerialized] public bool _inAction = false;
    [NonSerialized] public bool IsMoveAble = true;
    [NonSerialized] public bool IsRunable = true;
    [NonSerialized] public bool IsDisguised = false;
    [NonSerialized] public bool IsSafe = false;

    // Start is called before the first frame update
    void Start()
    {
        _cameraDirectionTransform = GameObject.Find("Main Camera").transform;
        _collider = gameObject.GetComponent<BoxCollider>();
        cc = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();

        HeadBoxItem.SetActive(false);
        BigBoxItem.SetActive(false);

    }

    private void OnEnable()
    {
        DisguiseItem.PlayerDisguise += () => PlayerDisguise();
        HideItem.PlayerHide += () => PlayerHide();
        AttackState.DeathEvent += () => PlayerDead();
        SafeItem.PlayerSafe += () => PlayerSafe();
    }

    private void OnDisable()
    {
        DisguiseItem.PlayerDisguise -= () => PlayerDisguise();
        HideItem.PlayerHide -= () => PlayerHide();
        AttackState.DeathEvent -= () => PlayerDead();
        SafeItem.PlayerSafe -= () => PlayerSafe();
    }

    // Update is called once per frame
    void Update()
    {
        RunningChange();
        if (IsMoveAble)
        {
            PlayerMove();
        }
        
    }

    public void PlayerMove()
    {
        Vector3 move = GetInput();
        float speed = setSpeed();
        move = move * speed * Time.deltaTime;

        string ani;
        if (move != Vector3.zero)
        {
            SetPlayerRotation();
            _inAction = false;
            if (_isRunning)
            {
                ani = "rig_player|walk";
                //_animator.Play("rig_player|walk");
            }
            else
            {
                ani = "rig_player|slide";
                //_animator.Play("rig_player|slide");
            }
            _animator.Play(ani);
            AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
            if (stateInfo.IsName(ani) && stateInfo.normalizedTime >= 1.0f)
            {
                _animator.Play(ani, -1, 0f); // 从开头重新播放
            }
        }
        else
        {
            if (!_inAction)
            {
                ani = "rig_player|Idle";
                _animator.Play(ani);
            }
            AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
            if (stateInfo.normalizedTime >= 1.0f)
            {
                _inAction = false;
                _animator.Play("rig_player|Idle", -1, 0f); // 从开头重新播放
            }
            //_animator.Play("rig_player|Idle");

        }

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
        if (IsSafe)
        {
            return safeSpeed;
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

    private async UniTaskVoid PlayerDisguise()
    {
        DisguiseChange();
        gameObject.tag = "HiddenPlayer";
        float t = Time.time;

        _animator.Play("rig_player|getHeadBox");
        _inAction = true;

        await UniTask.WaitUntil(() => (Time.time - t > DisguiseDuaration|| Input.GetKeyDown(InterKey)));

        DisguiseChange();
        gameObject.tag = "Player";
    }

    public void DisguiseChange()
    {
        //禁用碰撞体，拒绝交互
        _collider.enabled = IsDisguised;

        IsDisguised = !IsDisguised;

        _isRunning = false;
        _canRun = !IsDisguised;

        //启用大箱子
        BigBoxItem.SetActive(IsDisguised);
        //transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, -transform.localScale.z);
    }

    private async UniTaskVoid PlayerSafe()
    {
        SafeChange();

        _animator.Play("rig_player|getHeadBox");
        _inAction = true;

        //gameObject.tag = "HiddenPlayer";
        float t = Time.time;

        await UniTask.WaitUntil(() => (Time.time - t > DisguiseDuaration|| Input.GetKeyDown(InterKey)));

        SafeChange();
        //gameObject.tag = "Player";
    }

    public void SafeChange()
    {
        //禁用碰撞体，拒绝交互
        //_collider.enabled = IsSafe;

        IsSafe = !IsSafe;

        _isRunning = false;
        _canRun = !IsSafe;

        //启用头戴式箱子
        HeadBoxItem.SetActive(IsSafe);
        //transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, -transform.localScale.z);
    }

    private async UniTaskVoid PlayerHide()
    {
        //HideChange();
        gameObject.tag = "HiddenPlayer";
        _animator.Play("rig_player|idleToHide");

        IsMoveAble = false;
        _collider.enabled = false;

        await UniTask.WaitUntil(() => {
            var stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
            return stateInfo.normalizedTime >= 1.0f;
        });

        gameObject.SetActive(false);

        await UniTask.WaitUntil(() => (Input.GetKeyDown(InterKey)));

        gameObject.SetActive(true);
        _animator.Play("rig_player|hideToIdle");

        await UniTask.WaitUntil(() => {
            var stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
            return stateInfo.normalizedTime >= 1.0f;
        });

        IsMoveAble = true;
        _collider.enabled = true;
        //HideChange();
        gameObject.tag = "Player";

    }

    public void HideChange()//无前摇隐藏，未启用
    {
        IsMoveAble = !IsMoveAble;
        gameObject.SetActive(IsMoveAble);
        _collider.enabled = IsMoveAble;
    }

    public void PlayerDead()
    {
        _animator.Play("rig_player|scared");
    }

}
