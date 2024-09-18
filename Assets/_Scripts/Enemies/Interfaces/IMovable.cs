

using UnityEngine;

public interface IMovable
{
    Rigidbody2D RB { get; set; }
    void MoveEnemy(Vector2 velocity);
}