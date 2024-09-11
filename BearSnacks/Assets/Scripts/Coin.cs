using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Coin : MonoBehaviour
{
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private float duration = 0.35f;
    
    private float targetYPosition = -1.55f; // Target Y position for the object to move to

    public UnityEvent onInsert;
    private AnimationUtils animationUtils;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        // Set the initial color and alpha of the sprite
        Color color = spriteRenderer.color;
        color.a = 0f;
        spriteRenderer.color = color;

        animationUtils = gameObject.AddComponent<AnimationUtils>();
        animationUtils.animator = animator;
    }

    public void InsertCoin()
    {
        StartCoroutine(FadeInAndMove());
    }

    private IEnumerator FadeInAndMove()
    {
        // Initial setup
        Color color = spriteRenderer.color;
        Vector3 initialPosition = transform.position;
        float initialYPosition = initialPosition.y;
        
        // Gradually fade in and move the object
        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            // Calculate the new alpha
            float alpha = t / duration;
            color.a = alpha;
            spriteRenderer.color = color;

            // Calculate the new Y position
            float newY = Mathf.Lerp(initialYPosition, targetYPosition, t / duration);
            transform.position = new Vector3(initialPosition.x, newY, initialPosition.z);

            // Wait for the next frame
            yield return null;
        }

        // Ensure final values
        color.a = 1f;
        spriteRenderer.color = color;
        transform.position = new Vector3(initialPosition.x, targetYPosition, initialPosition.z);

        animator.SetTrigger("Turn");
        yield return StartCoroutine(animationUtils.WaitForAnimationEnd());
        onInsert.Invoke();

    }
}
