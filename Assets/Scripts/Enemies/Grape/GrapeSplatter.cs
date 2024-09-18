using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapeSplatter : MonoBehaviour
{
    private SpriteFade _spriteFade;
    
    private void Awake()
    {
        _spriteFade = GetComponent<SpriteFade>();
    }
    
    void Start()
    {
        StartCoroutine(_spriteFade.SlowFadeRoutine());
        
        Invoke(nameof(DisableCollider), 0.2f);
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerHealth player = other.gameObject.GetComponent<PlayerHealth>();
        player?.TakeDamage(1, transform);
    }

    private void DisableCollider()
    {
        GetComponent<CapsuleCollider2D>().enabled = false;
    }
    
    
}
