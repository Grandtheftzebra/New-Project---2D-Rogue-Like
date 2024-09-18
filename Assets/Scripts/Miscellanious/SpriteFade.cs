using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class SpriteFade : MonoBehaviour
{
    [SerializeField] private float _fadeTime = .4f;
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    /// <summary>
    /// Slowly fades the sprite to 0 alpha making it invisible and then destroys the game object
    /// </summary>
    public IEnumerator SlowFadeRoutine()
    {
        float startValue = _spriteRenderer.color.a;
        float elapsedTime = 0;
        while (elapsedTime < _fadeTime)
        {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startValue, 0f, elapsedTime / _fadeTime);
            _spriteRenderer.color = new Color(_spriteRenderer.color.r, _spriteRenderer.color.g, _spriteRenderer.color.b, newAlpha);
            yield return null;
        }
        
        Destroy(gameObject);
    }
}