using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PmcCameraController : MonoBehaviour
{
    public Transform playerTransform;
    [Header("与物体的距离")]
    public float distance = 5f;   // 摄像机与物体的距离
    private float height = 3f;
    [Header("鼠标灵敏度")]
    public float mouseSensitivity = 2f; // 鼠标灵敏度
    [Header("垂直角度范围")]
    public float maxCurrentY;
    public float minCurrentY;
    [Header("相机模式")]
    [Range(1,2)]
    public int mode;

    [Header("开局角度(?)")]
    public float startX = -90f;
    public float startY = 45f;
    
    [Header("摄像机Y轴偏移")]
    public float offsetY = 0f;

    private float currentX = -90f;  //水平角度
    private float currentY = 45f; //垂直角度

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // 锁鼠标
        currentX = startX;
        currentY = startY;
        playerTransform = GameObject.Find("player").transform;

    }

    void Update()
    {
        if (Time.timeScale!=0)
        {
            Cursor.lockState = CursorLockMode.Locked;
            CameraSet(mode);
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
        }
        
    }

    public void CameraSet(int m)
    {
        switch (mode)
        {
            case 1:
                CameraSet1();
                break;

            case 2:
                CameraSet2();
                break;

        }

    }


    public void CameraSet2()//不固定水平距离
    {
        currentX += Input.GetAxis("Mouse X") * mouseSensitivity;
        currentY -= Input.GetAxis("Mouse Y") * mouseSensitivity;
        currentY = Mathf.Clamp(currentY, minCurrentY, maxCurrentY);

        if (playerTransform == null) return;

        Vector3 dir = new Vector3(0, 0, -distance);
        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
        Vector3 position = playerTransform.position + rotation * dir + Vector3.up * height;

        transform.position = position;
        // transform.LookAt(playerTransform.position);
        transform.LookAt(new Vector3(playerTransform.position.x, playerTransform.position.y+offsetY, playerTransform.position.z));
    }

    public void CameraSet1()//固定水平距离
    {
        currentX += Input.GetAxis("Mouse X") * mouseSensitivity;
        currentY -= Input.GetAxis("Mouse Y") * mouseSensitivity;
        currentY = Mathf.Clamp(currentY, minCurrentY, maxCurrentY); 


        if (playerTransform == null) return;

        float verticalAngleRad = currentY * Mathf.Deg2Rad;
        float height = distance * Mathf.Tan(verticalAngleRad);
        float horizontalOffset = distance;

        // 将水平偏移转换为世界坐标方向
        Vector3 horizontalDir = Quaternion.Euler(0, currentX, 0) * Vector3.forward;
        Vector3 position = playerTransform.position
                         - horizontalDir * horizontalOffset
                         + Vector3.up * height;

        transform.position = position;
        // transform.LookAt(playerTransform.position);
        transform.LookAt(new Vector3(playerTransform.position.x, playerTransform.position.y+offsetY, playerTransform.position.z));
    }

}
