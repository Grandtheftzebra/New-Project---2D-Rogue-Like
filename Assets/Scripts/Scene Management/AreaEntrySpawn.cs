using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaEntrySpawn : MonoBehaviour
{
    [SerializeField] string _transitionName;

    void Start()
    {
        if (SceneManagement.Instance.SceneTransitionName == _transitionName)
        {
            PlayerController.Instance.transform.position = this.transform.position;
            CameraController.Instance.SetPlayerFollowCamera(PlayerController.Instance.gameObject);
            
            UI_Fade.Instance.FadeFromBlack();
        }
    }
}
 