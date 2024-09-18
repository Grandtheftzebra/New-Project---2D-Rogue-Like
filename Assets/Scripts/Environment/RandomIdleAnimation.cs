using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomIdleAnimation : MonoBehaviour
{
    private Animator _anim;

    private void Awake()
    {
        _anim = GetComponent<Animator>();
    }

    private void Start()
    {
        AnimatorStateInfo _state = _anim.GetCurrentAnimatorStateInfo(0);
        _anim.Play(_state.fullPathHash, -1, UnityEngine.Random.Range(0f, 1f));
    }
}
