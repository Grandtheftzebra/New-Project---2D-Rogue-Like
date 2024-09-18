using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    [SerializeField] float _parallaxOffset = -0.15f;

    Camera _camera;
    Vector2 _startPos;
    Vector2 TravelDis => (Vector2)_camera.transform.position - _startPos;

    void Awake()
    {
        _camera = Camera.main;
    }

    void Start()
    {
        _startPos = transform.position;
    }

    void FixedUpdate() // When you try to move things, FixedUpdate is better than Update
    {
        transform.position = _startPos + TravelDis * _parallaxOffset;
    }
}


/* Why we use _startPos here:
We're using startPos in two different ways here.

In Travel, we're using startPos to get the delta from the camera's position to the start position.

In FixedUpdate, we're taking that delta and multiplying it by the offset, then adding it to the startPos.

If the startPos is zero, then no, you don't need the startPos because travel would be the position of the camera, and we'd set it to zero + the position of the camera + the offset.

If, however, the startPosition was NOT 0,0,0, then the calculation would be off without the actual startPosition.

*/
