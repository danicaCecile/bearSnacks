using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ClickableSprite : MonoBehaviour
{
    public UnityEvent OnPress;
    public UnityEvent OnRelease;

    private void OnMouseDown()
    {
        OnPress.Invoke();
    }

    private void OnMouseUp()
    {
        OnRelease.Invoke();
    }
}
