using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class VendingDisplay : MonoBehaviour
{
    //-------------------------- Applies to everything --------------------------------
    private AnimationUtils animationUtils;

    void Start()
    {
        spriteRenderer = transform.Find("Renderer").GetComponent<SpriteRenderer>();

        overlayAnimator = transform.Find("Overlay").GetComponent<Animator>();

        spriteRenderer.sprite = defaultSprite;
        screenLight.gameObject.SetActive(false);

        animationUtils = gameObject.AddComponent<AnimationUtils>();
        animationUtils.animator = overlayAnimator;

        lightStartScale = screenLight.localScale;

        bubble = transform.parent.transform.parent.transform.parent.transform.parent.GetComponent<Bubble>();
        bubble.onClose.AddListener(Off);
    }

    void OnDisable()
    {
        animationUtils.isAnimating = false;
        isOn = false;
        isDisplaying = false;
    }

    void Update()
    {
        if (isLightChangingX)
        {
            Debug.Log("Changing X");
            elapsedTimeX += Time.deltaTime;

            float fractionX = Mathf.Clamp01(elapsedTimeX / timeToComplete);
            screenLight.localScale = Vector3.Lerp(screenLight.localScale, targetX, fractionX);

            if (screenLight.localScale.x == targetX.x)
            {
                Debug.Log("Done changing X");
                isLightChangingX = false;
                if(isTurningOn == true) 
                {
                    Debug.Log("Activating Y");
                    isLightChangingY = true;
                }
                elapsedTimeX = 0f;
            }
        }

        if (isLightChangingY)
        {
            Debug.Log("Changing Y");
            elapsedTimeY += Time.deltaTime;

            float fractionY = Mathf.Clamp01(elapsedTimeY / timeToComplete);
            screenLight.localScale = Vector3.Lerp(screenLight.localScale, targetY, fractionY);

            if (screenLight.localScale.y == targetY.y)
            {
                Debug.Log("Done changing Y");
                isLightChangingY = false;
                if(isTurningOn == false) 
                {
                    Debug.Log("Activating X");
                    isLightChangingX = true;
                }
                elapsedTimeY = 0f;
            }
        }
    }

    /*-------------------- Part 1: Handles displaying pictures and text on the display ----------------------------*/
    public List<Sprite> displayOptions;
    public Sprite defaultSprite;
    private SpriteRenderer spriteRenderer;

    private UnityEvent DisplayRandomEvent;
    [HideInInspector]
    public int currentSpriteIndex;

    private bool isDisplaying = false;

    public void DisplayForSeconds(float time, Sprite sprite)
    {
        if(isDisplaying == true) return;
        StartCoroutine(DisplayForSecondsCoroutine(time, sprite));
    }

    public void DisplayRandomForSeconds(float time)
    {
        if(isDisplaying == true) return;
        Sprite sprite = displayOptions[Random.Range(0, displayOptions.Count)];
        StartCoroutine(DisplayForSecondsCoroutine(time, sprite));
    }

    private IEnumerator DisplayForSecondsCoroutine(float time, Sprite sprite)
    {
        isDisplaying = true;
        spriteRenderer.sprite = sprite;

        yield return new WaitForSeconds(time);

        spriteRenderer.sprite = defaultSprite;
        isDisplaying = false;
    }

    /*-------------------- Part 2: Handles turning display on and off  ----------------------------*/
    private Animator overlayAnimator;

    private bool isOn = false;

    private Bubble bubble;

    public void On()
    {
        if(isOn == true || animationUtils.isAnimating == true) return;
        isOn = true;

        StartCoroutine(OnCoroutine());
    }

    private IEnumerator OnCoroutine()
    {
        overlayAnimator.SetTrigger("On");
        
        yield return StartCoroutine(animationUtils.SetAnimationLength());
        bubble.closeDelay = animationUtils.length;

        yield return StartCoroutine(AnimateLightOn());
        yield return StartCoroutine(animationUtils.WaitForAnimationEnd());
    }

    public void Off()
    {
        if(isOn == false || animationUtils.isAnimating == true) return;
        StartCoroutine(OffCoroutine());
    }

    private IEnumerator OffCoroutine()
    {
        overlayAnimator.SetTrigger("Off");
        yield return StartCoroutine(AnimateLightOff());
        yield return StartCoroutine(animationUtils.WaitForAnimationEnd());
        screenLight.gameObject.SetActive(false);
        isOn = false;
        bubble.closeDelay = 0f;
    }

    public void Toggle()
    {
        if(isOn == true) Off();
        else On();
    }

    /*----------------------------- Part 3: Handle stuff about lights ---------------------------------------*/
    public Transform screenLight;
    public float startingLightWidth = 0.03f;

    private bool isLightChangingX = false;
    private bool isLightChangingY = false;

    private Vector3 targetX;
    private Vector3 targetY;
    private Vector3 lightStartScale;

    private float timeToComplete;
    private float elapsedTimeX;
    private float elapsedTimeY;
    private bool isTurningOn = false;

    public IEnumerator AnimateLightOn()
    {
        float startingY = startingLightWidth;
        float startingX = 0f;
        screenLight.localScale = new Vector3(startingX, startingY, lightStartScale.z);

        targetX = new Vector3(lightStartScale.x, startingY, lightStartScale.z);
        targetY = new Vector3(lightStartScale.x, lightStartScale.y, lightStartScale.z);

        screenLight.gameObject.SetActive(true);
        yield return StartCoroutine(animationUtils.SetAnimationLength());

        timeToComplete = animationUtils.length;

        isTurningOn = true;
        isLightChangingX = true;
    }

    public IEnumerator AnimateLightOff()
    {
        float startingY = screenLight.localScale.x;
        float startingX = screenLight.localScale.y;
        screenLight.localScale = new Vector3(startingX, startingY, lightStartScale.z);

        targetY = new Vector3(screenLight.localScale.x, startingLightWidth, lightStartScale.z);
        targetX = new Vector3(0f, startingLightWidth, lightStartScale.z);

        yield return StartCoroutine(animationUtils.SetAnimationLength());
        timeToComplete = animationUtils.length;

        isTurningOn = false;
        isLightChangingY = true;
    }
}
