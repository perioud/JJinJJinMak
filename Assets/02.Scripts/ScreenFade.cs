using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Events;

public class ScreenFade : MonoBehaviour
{
    public Image fadeImage;
    public float fadeDuration = 1f;
    public UnityEvent OnFadeComplete;

    private bool isFading = false;

    private void Start()
    {
        if (fadeImage == null)
        {
            fadeImage = GetComponent<Image>();
        }
        fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, 0f);
    }

    public void FadeIn()
    {
        if (!isFading)
        {
            StartCoroutine(Fade(0f, 1f));
        }
    }

    public void FadeOut()
    {
        if (!isFading)
        {
            StartCoroutine(Fade(1f, 0f));
        }
    }

    private IEnumerator Fade(float startAlpha, float endAlpha)
    {
        isFading = true;
        float elapsedTime = 0f;
        Color color = fadeImage.color;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / fadeDuration);
            fadeImage.color = new Color(color.r, color.g, color.b, alpha);
            yield return null;
        }

        fadeImage.color = new Color(color.r, color.g, color.b, endAlpha);
        OnFadeComplete?.Invoke();
        isFading = false;
    }
}
