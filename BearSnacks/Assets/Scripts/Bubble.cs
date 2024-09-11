using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Bubble : MonoBehaviour
{
    private List<Animator> animators = new List<Animator>();
    private List<GameObject> children = new List<GameObject>();

    private bool isOpen = false;
    private bool isAnimating = false;

    public GameObject bubbleParent;

    private AnimationUtils animationUtils;

    public UnityEvent onOpen;
    public UnityEvent onClose;

    [HideInInspector]
    public float closeDelay = 0.5f;
    
    void Start()
    {
        Animator[] animatorsArray = bubbleParent.GetComponentsInChildren<Animator>(true);
        animators.AddRange(animatorsArray);

        animationUtils = gameObject.AddComponent<AnimationUtils>();
        animationUtils.animator = animators[animators.Count-1];

        bubbleParent.SetActive(false);
    }

    public void Open()
    {
        if(isOpen == true || animationUtils.isAnimating == true) return;
        bubbleParent.SetActive(true);
        foreach(Animator animator in animators) animator.SetTrigger("Open");
        onOpen.Invoke();
        
        StartCoroutine(OpenCoroutine());
    }

    private IEnumerator OpenCoroutine()
    {
        yield return StartCoroutine(animationUtils.WaitForAnimationEnd());

        isOpen = true;
    }

    public void Close()
    {
        if(isOpen == false || animationUtils.isAnimating == true) return;
        onClose.Invoke();
        StartCoroutine(CloseCoroutine());
    }

    private IEnumerator CloseCoroutine()
    {
        yield return new WaitForSeconds(closeDelay);
        
        foreach(Animator animator in animators) animator.SetTrigger("Close");
        yield return StartCoroutine(animationUtils.WaitForAnimationEnd());

        bubbleParent.SetActive(false);
        isOpen = false;
    }

    public void Toggle()
    {
        if(isOpen == true) Close();
        else Open();
    }
}
