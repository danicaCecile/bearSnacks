using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LightController2D : MonoBehaviour
{
    private UnityEngine.Rendering.Universal.Light2D light;
    private float defaultIntensity;
    private Color defaultColor;

    public void Start()
    {
        light = GetComponent<UnityEngine.Rendering.Universal.Light2D>();
        defaultIntensity = light.intensity;
        defaultColor = light.color;
    }

    public void Blink(float duration, int numberOfTimes)
    {
        StartCoroutine(BlinkCoroutine(duration, numberOfTimes));
    }

    public void Off(float duration)
    {
        ChangeIntensity(duration, 0f);
    }

    public void On(float duration)
    {
        ChangeIntensity(duration, defaultIntensity);
    }

    public void ChangeIntensity(float duration, float targetIntensity)
    {   
        StartCoroutine(ChangeIntensityCoroutine(duration, targetIntensity));
    }

    public void ChangeColor(float duration, List<Color> colors, bool resetColor)
    {
        List<Color> newColors = new List<Color>(colors);
        if(resetColor == true) newColors.Add(defaultColor);
        StartCoroutine(ChangeColorCoroutine(duration, newColors));
    }

    public void ChangeColor(float duration, Color color)
    {
        StartCoroutine(ChangeColorCoroutine(duration, color));
    }

    public void ResetColor(float duration)
    {
        ChangeColor(duration, defaultColor);
    }

    public void ResetIntensity(float duration)
    {
        ChangeIntensity(duration, defaultIntensity);
    }
    
    private IEnumerator BlinkCoroutine(float duration, int numberOfTimes)
    {
        float totalBlinkTime = duration / numberOfTimes;
        float halfBlinkTime = totalBlinkTime / 2f;

        float startIntensity = 0f;
        float secondIntensity = 0f;

        if(light.intensity == defaultIntensity) startIntensity = defaultIntensity;
        else secondIntensity = defaultIntensity;
        
        for (int i = 0; i < numberOfTimes; i++)
        {
            yield return StartCoroutine(ChangeIntensityCoroutine(halfBlinkTime, secondIntensity));
            yield return StartCoroutine(ChangeIntensityCoroutine(halfBlinkTime, startIntensity));
        }
    }

    private IEnumerator ChangeIntensityCoroutine(float duration, float targetIntensity)
    {
        float startIntensity = light.intensity;
        float elapsedTime = 0f;
        while(elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float newIntensity = Mathf.Lerp(startIntensity, targetIntensity, elapsedTime / duration);
            light.intensity = newIntensity;
            yield return null;
        }

        light.intensity = targetIntensity;
    }

    private IEnumerator ChangeColorCoroutine(float duration, Color targetColor)
    {
        Color startColor = light.color;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            light.color = Color.Lerp(startColor, targetColor, elapsedTime / duration);
            yield return null;
        }

        light.color = targetColor; 
    }

    private IEnumerator ChangeColorCoroutine(float duration, List<Color> colors)
    {
        float shiftDuration = duration/colors.Count;

        for (int i = 0; i < colors.Count; i++)
        {
            yield return StartCoroutine(ChangeColorCoroutine(shiftDuration, colors[i]));
        }
    }
}
