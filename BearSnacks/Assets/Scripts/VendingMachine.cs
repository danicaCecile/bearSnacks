using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VendingMachine : MonoBehaviour
{
    public ButtonPad buttonPad;
    public Coin coin;
    public Screen screen;

    void Start()
    {
        coin.onInsert.AddListener(TurnOn);
        buttonPad.onRelease.AddListener(SelectItem);
    }

    private void TurnOn()
    {

    }

    private void SelectItem()
    {
        
    }
}
