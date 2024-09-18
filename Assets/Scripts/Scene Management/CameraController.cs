using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : Singleton<CameraController>
{
    CinemachineVirtualCamera _vcam;

    private void Start()
    {
        SetPlayerFollowCamera(PlayerController.Instance.gameObject);
    }
    public void SetPlayerFollowCamera(GameObject player)
    {
        _vcam = FindObjectOfType<CinemachineVirtualCamera>();
        _vcam.Follow = player.transform;
    }
}