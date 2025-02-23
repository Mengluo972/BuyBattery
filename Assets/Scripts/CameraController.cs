using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Quaternion _targetRotation;

    private void Start()
    {
        _targetRotation = transform.rotation;
    }

    public void ChangeRotationClockwise()
    {
        _targetRotation.y += 90;
        transform.parent.DORotate(new Vector3(transform.parent.rotation.x,_targetRotation.y,transform.parent.rotation.z),0.5f,RotateMode.Fast);
    }
    
    public void ChangeRotationAnticlockwise()
    {
        _targetRotation.y -= 90;
        transform.parent.DORotate(new Vector3(transform.parent.rotation.x,_targetRotation.y,transform.parent.rotation.z),0.5f,RotateMode.Fast);
    }

    private void Update()
    {
        
    }
}
