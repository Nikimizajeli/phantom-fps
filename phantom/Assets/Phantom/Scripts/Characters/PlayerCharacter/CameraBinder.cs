using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.Serialization;

public class CameraBinder : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    [SerializeField] private float distance = 1.0f;

    private Transform _mainCameraTransform;
    
    private void Start()
    {
        if (Camera.main != null)
        {
            _mainCameraTransform = Camera.main.transform;
        }
    }

    void Update()
    {
        transform.rotation = _mainCameraTransform.rotation;
    }
}
