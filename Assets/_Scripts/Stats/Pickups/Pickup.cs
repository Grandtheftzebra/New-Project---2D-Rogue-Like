using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Pickup : MonoBehaviour, IVisitor
{
    protected abstract void GainPickUpEffect(Entity entity);
    
    public void Visit<T>(T visitable) where T : Component, IVisitable
    {
        if (visitable is Entity entity)
        {
            GainPickUpEffect(entity);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        other.GetComponent<IVisitable>()?.Accept(this);
        
        Destroy(gameObject);
    }
}