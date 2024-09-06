using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteSwap : MonoBehaviour
{
    private SpriteRenderer target;
    public Sprite primarySprite;
    public Sprite secondarySprite;

    void Start()
    {
        target = GetComponent<SpriteRenderer>();
        primarySprite = target.sprite;
    }

    public void Swap(Sprite newSprite)
    {
        if(target == null) Debug.LogError("Sprite swap requires sprite renderer component to be attached to its game object.");
        target.sprite = newSprite;
    }

    public void TemporarySpriteSwap(float pause)
    {
        StartCoroutine(TemporarySpriteSwapCoroutine(pause));
    }

    private IEnumerator TemporarySpriteSwapCoroutine(float pause)
    {
        Swap(secondarySprite);

        yield return new WaitForSeconds(pause);

        Swap(primarySprite);
    }
}
