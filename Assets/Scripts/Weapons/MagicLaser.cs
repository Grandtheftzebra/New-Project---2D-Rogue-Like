using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicLaser : MonoBehaviour
{
    [SerializeField] private float laserGrowTime;
    
    private SpriteRenderer _spriteRenderer;
    private CapsuleCollider2D _capCollider;
    private float _laserRange;
    
    private bool _isLaserGrowing = true;
    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _capCollider = GetComponent<CapsuleCollider2D>();
    }

    private void Start()
    {
        LaserFaceMouse();
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        Indestructible indestructible = other.gameObject.GetComponent<Indestructible>();
        if (!other.isTrigger && indestructible)
            _isLaserGrowing = false;
        
       
    }

    public void UpdateLaserRange(float range)
    {
        _laserRange = range;
        StartCoroutine(IncreaseLaserLength());
    }

    private IEnumerator IncreaseLaserLength()
    {
        float timePassed = 0.1f;
        
        while (_spriteRenderer.size.x < _laserRange && _isLaserGrowing)
        {
            timePassed += Time.deltaTime;
            float linearTime = timePassed / laserGrowTime;
            _spriteRenderer.size = new Vector2(Mathf.Lerp(1f, _laserRange, linearTime), _spriteRenderer.size.y);
            _capCollider.size = new Vector2(Mathf.Lerp(0, _laserRange, linearTime), _capCollider.size.y);
            _capCollider.offset = new Vector2(_spriteRenderer.size.x / 2, 0);
            yield return null;
        }
        
        StartCoroutine(GetComponent<SpriteFade>().SlowFadeRoutine());
    }
    
    private void LaserFaceMouse()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);
        
        Vector2 direction = transform.position - mousePos;

        transform.right = -direction;
    }
}
