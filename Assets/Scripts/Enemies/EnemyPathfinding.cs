using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPathfinding : MonoBehaviour
{
    [SerializeField] float _moveSpeed = 5f;

    Vector2 _moveDirection;
    Knockback _knockback;

    Rigidbody2D _rb;

    void Awake()
    {
        _knockback = GetComponent<Knockback>(); // Need to get the component to change the bool in the fixedUpdate
        _rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (_knockback.IsKnockedBack) // When we are getting knocked back Move will not be executed and we the knockback effect is applied correctly.
            return;                   // Otherwise with both Methods trying to alter the _rb the knockback would've been overwritten because of the fixedUpdate.
        
        Move();
        
        // Flips the sprite to face the direction it's moving
        if (_moveDirection.x < 0)
            transform.localScale = new Vector3(-1, 1, 1);
        else //if (_moveDirection.x > 0) Instructor has it that way, but for me it gets buggy, so I just let it stand with else
            transform.localScale = new Vector3(1, 1, 1);
    }

    void Move()
    {
        _rb.MovePosition(_rb.position + _moveDirection * (_moveSpeed * Time.fixedDeltaTime));
    }

    public void MoveTo(Vector2 targetPosition)
    {
        _moveDirection = targetPosition;
    }
    
    public void StopMoving()
    {
        _moveDirection = Vector2.zero;
    }
}
