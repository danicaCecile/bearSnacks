using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Door : MonoBehaviour
{
    private Animator animator;
    private bool isOpen = false;
    private BoxCollider2D collider;

    public UnityEvent onCollection;

    private AnimationUtils animationUtils;

    void Start()
    {
        animator = GetComponent<Animator>();
        collider = GetComponent<BoxCollider2D>();
        collider.enabled = false;

        if (animator == null)
        {
            Debug.LogError("Animator component not found on this GameObject!");
        }

        animationUtils = gameObject.AddComponent<AnimationUtils>();
        animationUtils.animator = animator;
    }

    public void Open()
    {
        if(isOpen == true) return;
        isOpen = true;
        animator.SetTrigger("Open");
        StartCoroutine(animationUtils.WaitForAnimationEnd());
        collider.enabled = true;
    }

    public void Close()
    {
        if(isOpen == false) return;
        isOpen = false;
        collider.enabled = false;
        animator.SetTrigger("Close");
    }

    private void OnMouseUp()
    {
        onCollection.Invoke();
        Close();
    }
}
