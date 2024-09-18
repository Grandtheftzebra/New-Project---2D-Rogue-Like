using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Enemy Data")]
public class EnemySO : ScriptableObject
{
    public int Health;
    public int AttackPower;
    public int AttackSpeed;
    public int MoveSpeed;

    public float PatrolRange;
    public float TriggeredRange;
    
    private void Start()
    {
        
    }

    private void Update()
    {
        
    }
}
