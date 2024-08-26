using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenEmote : MonoBehaviour
{
    public float movementAmount = 1f;
    public float speed = 1f;
    public float emotePause = 1f;

    private Vector3 originalPosition;
    private bool isBobbing = true;

    public List<Sprite> bigEmotes = new List<Sprite>();
    private Sprite bigDefaultSprite;

    public List<Sprite> smallEmotes = new List<Sprite>();
    private Sprite smallDefaultSprite;

    public SpriteRenderer bigSpriteRenderer;
    public SpriteRenderer smallSpriteRenderer;

    void Start()
    {
        originalPosition = transform.position;
        StartCoroutine(BobbingCoroutine());
        bigDefaultSprite = bigSpriteRenderer.sprite;
        smallDefaultSprite = smallSpriteRenderer.sprite;
    }

    void Update()
    {
        if (isBobbing)
        {
            float newY = originalPosition.y + Mathf.Sin(Time.time * speed) * movementAmount;
            transform.position = new Vector3(originalPosition.x, newY, originalPosition.z);
        }
    }

    public void EmoteTest()
    {
        StartCoroutine(EmoteCoroutine());
    }

    public IEnumerator EmoteCoroutine()
    {
        //Random random = new Random();
        int randomInt = Random.Range(0, bigEmotes.Count);
        bigSpriteRenderer.sprite = bigEmotes[randomInt];
        smallSpriteRenderer.sprite = smallEmotes[randomInt];

        yield return new WaitForSeconds(emotePause);

        bigSpriteRenderer.sprite = bigDefaultSprite;
        smallSpriteRenderer.sprite = smallDefaultSprite;
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