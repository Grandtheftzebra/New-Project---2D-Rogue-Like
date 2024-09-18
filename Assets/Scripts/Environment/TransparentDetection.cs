using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TransparentDetection : MonoBehaviour
{
    [SerializeField, Range(0, 1)] float _transparencySlider = .8f;
    [SerializeField] float _fadeTime = .4f;

    SpriteRenderer _spriteRenderer; // for every object we set via sprite
    Tilemap _tilemap; // for anything that is set via the tile palette


    void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _tilemap = GetComponent<Tilemap>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerController>())
        {
            if (_spriteRenderer)
                StartCoroutine(FadeRoutine(_spriteRenderer, _fadeTime, _spriteRenderer.color.a, _transparencySlider));

            if (_tilemap)
                StartCoroutine(FadeRoutine(_tilemap, _fadeTime, _tilemap.color.a, _transparencySlider));

        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerController>())
        {
            if (_spriteRenderer)
                StartCoroutine(FadeRoutine(_spriteRenderer, _fadeTime, _spriteRenderer.color.a, 1f));

            if (_tilemap)
                StartCoroutine(FadeRoutine(_tilemap, _fadeTime, _tilemap.color.a, 1f));

        }
    }

    IEnumerator FadeRoutine(SpriteRenderer spriteRenderer,
                            float fadeTime,
                            float startValue,
                            float targetTransparency)
    {
        float elapsedTime = 0;
        while (elapsedTime < fadeTime)
        {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startValue, targetTransparency, elapsedTime / fadeTime);
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, newAlpha);
            yield return null;
        }
    }

    IEnumerator FadeRoutine(Tilemap tilemap,
                            float fadeTime,
                            float startValue,
                            float targetTransparency)
    {
        float elapsedTime = 0;
        while (elapsedTime < fadeTime)
        {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startValue, targetTransparency, elapsedTime / fadeTime);
            tilemap.color = new Color(tilemap.color.r, tilemap.color.g, tilemap.color.b, newAlpha);
            yield return null;
        }
    }
}
