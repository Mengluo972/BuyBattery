using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class RayCastTest : MonoBehaviour
{
    [SerializeField] private byte rayCount = 10;//不建议条数在10以下
    [SerializeField] private float fieldOfView = 90f;
    [SerializeField] private float viewDistance = 25f;//从巡逻状态进入逮人状态的检测距离
    [SerializeField] private float rayRadius = 10f;//警戒值开始增长的距离
    public float chaseDistance = 10f;//从追人状态进入逮人状态的检测距离
    [NonSerialized] public bool IsPatrolTracing;//是否开启巡逻射线追踪
    [NonSerialized] public bool IsChaseTracing;//是否开启追击射线追踪
    [NonSerialized] public bool IsPlayerDetected;//是否检测到玩家
    private bool _singleScanDetected;//单次扫描是否检测到玩家

    void Start()
    {
    }

    void Update()
    {
        PatrolRayTrace();
        ChaseRayTrace();
    }

    private void ChaseRayTrace()
    {
        if (!IsChaseTracing) return;
        _singleScanDetected = false;
        IsPlayerDetected = false;
        switch (rayCount%2)
        {
            case 0://偶数条射线
                for (int i = 0; i < rayCount / 2; i++)
                {
                    //右侧
                    Quaternion rotation =
                        Quaternion.AngleAxis(fieldOfView / 2 / rayCount * (i + 1), new Vector3(0f, 1f, 0f));
                    Vector3 direction = rotation * transform.forward;
                    Ray ray = new Ray(transform.position, direction);
                    RaycastHit raycastHit = new RaycastHit();
                    if (Physics.Raycast(ray, out raycastHit, viewDistance))
                    {
                        if (_singleScanDetected) break;
                        if (raycastHit.collider.gameObject.CompareTag("Player"))
                        {
                            // print("检测到玩家");
                            IsPlayerDetected = true;
                            _singleScanDetected = true;
                        }
                    }
                    //左侧
                    Quaternion rotation1 =
                        Quaternion.AngleAxis(-fieldOfView / 2 / rayCount * (i + 1), new Vector3(0f, 1f, 0f));
                    Vector3 direction1 = rotation1 * transform.forward;
                    Ray ray1 = new Ray(transform.position, direction1);
                    RaycastHit raycastHit1 = new RaycastHit();
                    if (Physics.Raycast(ray1, out raycastHit1, viewDistance))
                    {
                        if (_singleScanDetected) break;
                        if (raycastHit1.collider.gameObject.CompareTag("Player"))
                        {
                            IsPlayerDetected = true;
                            _singleScanDetected = true;
                        }
                    }
                }
                break;
            case 1://奇数条射线
                for (int i = 0; i < rayCount / 2; i++)
                {
                    //右侧
                    Quaternion rotation =
                        Quaternion.AngleAxis(fieldOfView / 2 / rayCount * (i + 1), new Vector3(0f, 1f, 0f));
                    Vector3 direction = rotation * transform.forward;
                    Ray ray = new Ray(transform.position, direction);
                    RaycastHit raycastHit = new RaycastHit();
                    if (Physics.Raycast(ray,out raycastHit,viewDistance))
                    {
                        if (_singleScanDetected) break;
                        if (raycastHit.collider.gameObject.CompareTag("Player"))
                        {
                            IsPlayerDetected = true;
                            _singleScanDetected = true;
                        }
                    }
                    //左侧
                    Quaternion rotation1 =
                        Quaternion.AngleAxis(-fieldOfView / 2 / rayCount * (i + 1), new Vector3(0f, 1f, 0f));
                    Vector3 direction1 = rotation1 * transform.forward;
                    Ray ray1 = new Ray(transform.position, direction1);
                    RaycastHit raycastHit1 = new RaycastHit();
                    if (Physics.Raycast(ray1,out raycastHit1,viewDistance))
                    {
                        if (_singleScanDetected) break;
                        if (raycastHit1.collider.gameObject.CompareTag("Player"))
                        {
                            IsPlayerDetected = true;
                            _singleScanDetected = true;
                        }
                    }
                }
                Ray ray0 = new Ray(transform.position, transform.forward);
                RaycastHit raycastHit0 = new RaycastHit();
                if (Physics.Raycast(ray0,out raycastHit0,viewDistance))
                {
                    if (_singleScanDetected) break;
                    if (raycastHit0.collider.gameObject.CompareTag("Player"))
                    {
                        IsPlayerDetected = true;
                        _singleScanDetected = true;
                    }
                }
                break;
        }
    }

    private void PatrolRayTrace()
    {
        if (!IsPatrolTracing) return;
        _singleScanDetected = false;
        IsPlayerDetected = false;
        switch (rayCount%2)
        {
            case 0://偶数条射线
                for (int i = 0; i < rayCount / 2; i++)
                {
                    //右侧
                    Quaternion rotation =
                        Quaternion.AngleAxis(fieldOfView / 2 / rayCount * (i + 1), new Vector3(0f, 1f, 0f));
                    Vector3 direction = rotation * transform.forward;
                    Ray ray = new Ray(transform.position, direction);
                    RaycastHit raycastHit = new RaycastHit();
                    if (Physics.Raycast(ray, out raycastHit, rayRadius))
                    {
                        if (_singleScanDetected) break;
                        if (raycastHit.collider.gameObject.CompareTag("Player"))
                        {
                            // print("检测到玩家");
                            IsPlayerDetected = true;
                            _singleScanDetected = true;
                        }
                    }
                    //左侧
                    Quaternion rotation1 =
                        Quaternion.AngleAxis(-fieldOfView / 2 / rayCount * (i + 1), new Vector3(0f, 1f, 0f));
                    Vector3 direction1 = rotation1 * transform.forward;
                    Ray ray1 = new Ray(transform.position, direction1);
                    RaycastHit raycastHit1 = new RaycastHit();
                    if (Physics.Raycast(ray1, out raycastHit1, rayRadius))
                    {
                        if (_singleScanDetected) break;
                        if (raycastHit1.collider.gameObject.CompareTag("Player"))
                        {
                            IsPlayerDetected = true;
                            _singleScanDetected = true;
                        }
                    }
                }
                break;
            case 1://奇数条射线
                for (int i = 0; i < rayCount / 2; i++)
                {
                    //右侧
                    Quaternion rotation =
                        Quaternion.AngleAxis(fieldOfView / 2 / rayCount * (i + 1), new Vector3(0f, 1f, 0f));
                    Vector3 direction = rotation * transform.forward;
                    Ray ray = new Ray(transform.position, direction);
                    RaycastHit raycastHit = new RaycastHit();
                    if (Physics.Raycast(ray,out raycastHit,rayRadius))
                    {
                        if (_singleScanDetected) break;
                        if (raycastHit.collider.gameObject.CompareTag("Player"))
                        {
                            IsPlayerDetected = true;
                            _singleScanDetected = true;
                        }
                    }
                    //左侧
                    Quaternion rotation1 =
                        Quaternion.AngleAxis(-fieldOfView / 2 / rayCount * (i + 1), new Vector3(0f, 1f, 0f));
                    Vector3 direction1 = rotation1 * transform.forward;
                    Ray ray1 = new Ray(transform.position, direction1);
                    RaycastHit raycastHit1 = new RaycastHit();
                    if (Physics.Raycast(ray1,out raycastHit1,rayRadius))
                    {
                        if (_singleScanDetected) break;
                        if (raycastHit1.collider.gameObject.CompareTag("Player"))
                        {
                            IsPlayerDetected = true;
                            _singleScanDetected = true;
                        }
                    }
                }
                Ray ray0 = new Ray(transform.position, transform.forward);
                RaycastHit raycastHit0 = new RaycastHit();
                if (Physics.Raycast(ray0,out raycastHit0,rayRadius))
                {
                    if (_singleScanDetected) break;
                    if (raycastHit0.collider.gameObject.CompareTag("Player"))
                    {
                        IsPlayerDetected = true;
                        _singleScanDetected = true;
                    }
                }
                break;
        }
    }
}
