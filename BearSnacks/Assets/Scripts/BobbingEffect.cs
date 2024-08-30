using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BobbingEffect : MonoBehaviour
{
    public float movementAmount = 1f;
    public float speed = 1f;

    private Vector3 originalPosition;
    private bool isBobbing = true;

    void Start()
    {
        originalPosition = transform.position;
        StartCoroutine(BobbingCoroutine());
    }

    void Update()
    {
        if (isBobbing)
        {
            float newY = originalPosition.y + Mathf.Sin(Time.time * speed) * movementAmount;
            transform.position = new Vector3(originalPosition.x, newY, originalPosition.z);
        }
    }

    private IEnumerator BobbingCoroutine()
    {
        while (isBobbing)
        {
            float newY = originalPosition.y + Mathf.Sin(Time.time * speed) * movementAmount;
            transform.position = new Vector3(originalPosition.x, newY, originalPosition.z);
            yield return null;
        }
    }
}
