using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseFollow : MonoBehaviour
{
    private void Update()
    {
        FollowMouse();
    }

    /// <summary>
    /// Read mousePos via Input System this will give us the value in screen position (so it depends on the users end device).
    /// Transform Screen position into world coordinates (think of it as making it standardize)
    /// Store the direction (think of it more as the distance) from the player to the mousePos.
    /// Assign that value to the transform.right (it's local x axis). In our case this will rotate the weapon 
    /// </summary>
    private void FollowMouse()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);
        
        Vector2 direction = transform.position - mousePos;
        
        Debug.Log($"Direction: {direction}" );

        transform.right = direction;
    }
}
