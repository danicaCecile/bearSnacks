using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenEmote : MonoBehaviour
{
    public float emotePause = 1f;

    public List<Sprite> bigEmotes = new List<Sprite>();
    private Sprite bigDefaultSprite;

    public List<Sprite> smallEmotes = new List<Sprite>();
    private Sprite smallDefaultSprite;

    public SpriteRenderer bigSpriteRenderer;
    public SpriteRenderer smallSpriteRenderer;

    void Start()
    {
        bigDefaultSprite = bigSpriteRenderer.sprite;
        smallDefaultSprite = smallSpriteRenderer.sprite;
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
}