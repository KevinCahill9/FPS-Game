// Note: This script may include code or patterns modified from Unity tutorials.
// It has been modified and extended to suit the requirments of the project.
// Source: https://www.youtube.com/watch?v=1uW-GbHrtQc

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScreenBlack : MonoBehaviour
{
    public Image fadeImage;
    public float fadeDuration = 7.0f;

    public void StartFade()
    {
        StartCoroutine(FadeOut());
    }

    public void ResetFade() 
    {
        StartCoroutine(FadeIn());
    }

    private IEnumerator FadeOut()
    {
        float timer = 0f;
        Color startColor = fadeImage.color;
        Color endColor = new Color(0f, 0f, 0f, 1f); 

        while (timer < fadeDuration)
        {
            fadeImage.color = Color.Lerp(startColor, endColor, timer / fadeDuration);
            timer += Time.deltaTime;
            yield return null;
        }

        fadeImage.color = endColor;
    }

    private IEnumerator FadeIn() 
    {
        float timer = 0f;
        Color startColor = fadeImage.color;
        Color endColor = new Color(0f, 0f, 0f, 0f); 

        while (timer < fadeDuration)
        {
            fadeImage.color = Color.Lerp(startColor, endColor, timer / fadeDuration);
            timer += Time.deltaTime;
            yield return null;
        }

        fadeImage.color = endColor;
    }
}
