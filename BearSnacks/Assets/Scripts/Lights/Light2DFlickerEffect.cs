using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFlicker2D : MonoBehaviour
{

    private UnityEngine.Rendering.Universal.Light2D light;

    public float minIntensity = 0.5f;
    public float maxIntensity = 1.5f;
    public float maxFlickerSpeed = 0.15f;
    public float minFlickerSpeed = 0.05f;

    public bool isActive = true;

    private void Start()
    {
        light = GetComponent<UnityEngine.Rendering.Universal.Light2D>();
        StartCoroutine(Flicker());
    }

    void OnEnable()
    {
        light = GetComponent<UnityEngine.Rendering.Universal.Light2D>();
        StartCoroutine(Flicker());
    }

    private IEnumerator Flicker()
    {
        while(isActive)
        {
            float newIntensity = Random.Range(minIntensity, maxIntensity);
            light.intensity = newIntensity;

            float waitTime = Random.Range(minFlickerSpeed, maxFlickerSpeed);
            yield return new WaitForSeconds(waitTime);
        }
    }
}
