using UnityEngine;

public class AnimatedSpriteMask : MonoBehaviour
{
    public SpriteRenderer target;
    private SpriteMask mask;

    void Start()
    {
        mask = GetComponent<SpriteMask>();
    }

    void Update()
    {
        mask.sprite = target.sprite;
    }
}