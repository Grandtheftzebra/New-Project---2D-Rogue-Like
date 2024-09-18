using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleSOBase : ScriptableObject
{
    protected Enemy Enemy;
    protected Transform Transform;
    protected GameObject GameObject;

    protected Transform PlayerTransform;

    public virtual void Initialize(GameObject gameObject, Enemy enemy)
    {
        this.GameObject = gameObject;
        this.Enemy = enemy;
        Transform = gameObject.transform;
        PlayerTransform = UnityEngine.GameObject.FindGameObjectWithTag("Player").transform;
    }
}
