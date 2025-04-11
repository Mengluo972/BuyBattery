using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PmcCameraController : MonoBehaviour
{
    public Transform playerTransform;
    [Header("������ľ���")]
    public float distance = 5f;   // �����������ľ���
    private float height = 3f;
    [Header("���������")]
    public float mouseSensitivity = 2f; // ���������
    [Header("��ֱ�Ƕȷ�Χ")]
    public float maxCurrentY;
    public float minCurrentY;
    [Header("���ģʽ")]
    [Range(1,2)]
    public int mode;

    [Header("���ֽǶ�(?)")]
    public float startX = -90f;
    public float startY = 45f;

    private float currentX = -90f;  //ˮƽ�Ƕ�
    private float currentY = 45f; //��ֱ�Ƕ�

    void Start()
    {
        //Cursor.lockState = CursorLockMode.Locked; // �����
        currentX = startX;
        currentY = startY;
        playerTransform = transform.parent;

    }

    void Update()
    {
        CameraSet(mode);
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


    public void CameraSet2()//���̶�ˮƽ����
    {
        currentX += Input.GetAxis("Mouse X") * mouseSensitivity;
        currentY -= Input.GetAxis("Mouse Y") * mouseSensitivity;
        currentY = Mathf.Clamp(currentY, minCurrentY, maxCurrentY);

        if (playerTransform == null) return;

        Vector3 dir = new Vector3(0, 0, -distance);
        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
        Vector3 position = playerTransform.position + rotation * dir + Vector3.up * height;

        transform.position = position;
        transform.LookAt(playerTransform.position);
    }

    public void CameraSet1()//�̶�ˮƽ����
    {
        currentX += Input.GetAxis("Mouse X") * mouseSensitivity;
        currentY -= Input.GetAxis("Mouse Y") * mouseSensitivity;
        currentY = Mathf.Clamp(currentY, minCurrentY, maxCurrentY); 


        if (playerTransform == null) return;

        float verticalAngleRad = currentY * Mathf.Deg2Rad;
        float height = distance * Mathf.Tan(verticalAngleRad);
        float horizontalOffset = distance;

        // ��ˮƽƫ��ת��Ϊ�������귽��
        Vector3 horizontalDir = Quaternion.Euler(0, currentX, 0) * Vector3.forward;
        Vector3 position = playerTransform.position
                         - horizontalDir * horizontalOffset
                         + Vector3.up * height;

        transform.position = position;
        transform.LookAt(playerTransform.position);
    }

}
