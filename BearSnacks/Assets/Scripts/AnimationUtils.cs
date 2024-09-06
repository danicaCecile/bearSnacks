using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationUtils : MonoBehaviour
{
    public Animator animator;
    public bool isAnimating = false;
    public float length = 3f;
    private AnimatorStateInfo stateInfo;

    public IEnumerator WaitForAnimationEnd()
    {
        isAnimating = true;
        yield return StartCoroutine(SafelySetStateInfo());
        yield return new WaitForSeconds(stateInfo.length);
        isAnimating = false;
    }

    public IEnumerator SetAnimationLength()
    {
        yield return StartCoroutine(SafelySetStateInfo());
        length = stateInfo.length;
    }

    private IEnumerator SafelySetStateInfo()
    {
        yield return new WaitForEndOfFrame();
        while (animator.IsInTransition(0)) yield return null;

        stateInfo = animator.GetCurrentAnimatorStateInfo(0);
    }
}
