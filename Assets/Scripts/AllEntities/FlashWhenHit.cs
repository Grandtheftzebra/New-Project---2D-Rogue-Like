using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashWhenHit : MonoBehaviour
{
    [SerializeField] Material _material;
    [SerializeField] float _materialFlashTime = .2f;

    SpriteRenderer _spriteRenderer;
    Material _defaultMaterial;

    void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _defaultMaterial = _spriteRenderer.material;
    }

    public IEnumerator FlashRoutine()
    {
        _spriteRenderer.material = _material;

        yield return new WaitForSeconds(_materialFlashTime);

        _spriteRenderer.material = _defaultMaterial;
    }
}
