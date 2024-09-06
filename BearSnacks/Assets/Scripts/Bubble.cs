using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubble : MonoBehaviour
{
    private List<Animator> animators = new List<Animator>();
    private List<GameObject> children = new List<GameObject>();

    private bool isOpen = false;
    private bool isAnimating = false;

    public GameObject bubbleParent;

    private AnimationUtils animationUtils;

    void Start()
    {
        Animator[] animatorsArray = bubbleParent.GetComponentsInChildren<Animator>(true);
        animators.AddRange(animatorsArray);

        bubbleParent.SetActive(false);

        animationUtils = gameObject.AddComponent<AnimationUtils>();
        animationUtils.animator = animators[animators.Count-1];
    }

    public void Open()
    {
        if(isOpen == true || animationUtils.isAnimating == true) return;
        bubbleParent.SetActive(true);
        foreach(Animator animator in animators) animator.SetTrigger("Open");

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
        foreach(Animator animator in animators) animator.SetTrigger("Close");

        StartCoroutine(CloseCoroutine());
    }

    private IEnumerator CloseCoroutine()
    {
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
