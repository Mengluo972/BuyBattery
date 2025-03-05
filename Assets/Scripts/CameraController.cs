using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Quaternion _targetRotation;
    private PlayerController _playerController;

    private void Start()
    {
        _targetRotation = transform.rotation;
        _playerController = transform.parent.parent.GetComponent<PlayerController>();
    }

    public void ChangeRotationClockwise()
    {
        _targetRotation.y += 90;
        Sequence sequence = DOTween.Sequence();
        sequence.Append(transform
            .parent
            .DORotate(new Vector3(transform.parent.rotation.x, _targetRotation.y, transform.parent.rotation.z), 0.5f,
                RotateMode.Fast));
        sequence.AppendCallback(() => { _playerController.OnChangedCameraDirection(); });
    }
    
    public void ChangeRotationAnticlockwise()
    {
        _targetRotation.y -= 90;
        Sequence sequence = DOTween.Sequence();
        sequence.Append(transform.parent.DORotate(
            new Vector3(transform.parent.rotation.x, _targetRotation.y, transform.parent.rotation.z), 0.5f,
            RotateMode.Fast));
        sequence.AppendCallback(() => { _playerController.OnChangedCameraDirection(); });
    }

    private void Update()
    {
        
    }
}
