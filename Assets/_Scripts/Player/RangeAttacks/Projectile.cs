using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [field: SerializeField] public float FireForce { get; private set; } = 10;
    [field: SerializeField] public int Damage { get; private set; } = 5;
   

    public Rigidbody2D RB { get; set; }
    
}
