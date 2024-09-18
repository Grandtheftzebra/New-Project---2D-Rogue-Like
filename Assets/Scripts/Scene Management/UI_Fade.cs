using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;

public class UI_Fade : Singleton<UI_Fade>
{
    [SerializeField] Image _fadeImage;
    [SerializeField] float _fadeSpeed = 1f;

    IEnumerator _fadeRoutine;

    public void FadeToBlack()
    {
        if (_fadeRoutine != null)
            StopCoroutine(_fadeRoutine);
        
        _fadeRoutine = FadeRoutine(1f);
        StartCoroutine(_fadeRoutine);
    }
    
    public void FadeFromBlack()
    {
        if (_fadeRoutine != null)
            StopCoroutine(_fadeRoutine);
        
        _fadeRoutine = FadeRoutine(0f);
        StartCoroutine(_fadeRoutine);
    }

    IEnumerator FadeRoutine(float targetAlpha)
    {
        while (!Mathf.Approximately(_fadeImage.color.a, targetAlpha))
        {
            float alpha = Mathf.MoveTowards(_fadeImage.color.a, targetAlpha, _fadeSpeed * Time.deltaTime);
            _fadeImage.color = new Color(_fadeImage.color.r, _fadeImage.color.g, _fadeImage.color.b, alpha);
            yield return null;
        }
    }
}