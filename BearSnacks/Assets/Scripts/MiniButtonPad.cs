using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ButtonPad))]
public class MiniButtonPad : MonoBehaviour
{
    [HideInInspector]
    public List<List<GameObject>> buttonList;
    public GameObject miniButtonsParent;
    public Sprite pressedSprite;
    [HideInInspector]
    public Sprite unpressedSprite;
    private ButtonPad parentPad;

    void Start()
    {
        parentPad = GetComponent<ButtonPad>();
        buttonList = parentPad.InitButtonList(miniButtonsParent);
        unpressedSprite = buttonList[0][0].GetComponent<SpriteRenderer>().sprite;

        parentPad.onPress.AddListener(PressButton);
        parentPad.onRelease.AddListener(ReleaseButton);
    }

    private void PressButton()
    {
        buttonList[parentPad.currentLocation.x][parentPad.currentLocation.y].GetComponent<SpriteRenderer>().sprite = pressedSprite;
    }

    private void ReleaseButton()
    {
        buttonList[parentPad.currentLocation.x][parentPad.currentLocation.y].GetComponent<SpriteRenderer>().sprite = unpressedSprite;
    }
}
