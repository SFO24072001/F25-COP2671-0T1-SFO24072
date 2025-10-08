using System.Collections;
using UnityEngine;

public class ObjectFader : MonoBehaviour
{
    private float _fadeInSeconds = 0.25f;
    private float _fadeOutSeconds = 0.35f;
    private float _targetAlpha = 0.45f;
    private SpriteRenderer _spriteRenderer;
    
    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        if (_spriteRenderer == null)
        {
            var renderers = GetComponentsInChildren<SpriteRenderer>();
            foreach (var sr in renderers)
            {
                if (sr.enabled == true)
                {
                    _spriteRenderer = sr;
                    break;
                }

            }
        }
    }

    public void FadeOut() 
    {
        StartCoroutine(FadeOutRoutine());
    }
    private IEnumerator FadeOutRoutine()
    {
        var currentAlpha = _spriteRenderer.color.a;
        var distance = currentAlpha - _targetAlpha;

        while(currentAlpha - _targetAlpha > 0.01f)
        {
            currentAlpha = currentAlpha - distance / _fadeOutSeconds * Time.deltaTime;
            _spriteRenderer.color = new Color(1f, 1f, 1f, currentAlpha);
            yield return null;
        }
        _spriteRenderer.color = new Color(1f, 1f, 1f, _targetAlpha);
    }
    public void FadeIn() 
    {
        StartCoroutine(FadeInRoutine());
    }
    private IEnumerator FadeInRoutine()
    {
        var currentAlpha = _spriteRenderer.color.a;
        var distance = 1f - currentAlpha;

        while (1f - currentAlpha > 0.01f)
        {
            currentAlpha = currentAlpha + distance / _fadeInSeconds * Time.deltaTime;
            _spriteRenderer.color = new Color(1f, 1f, 1f, currentAlpha);
            yield return null;
        }
        _spriteRenderer.color = new Color(1f, 1f, 1f, 1f);
    }
}
