using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VendingMachine : MonoBehaviour
{
    public ButtonPad buttonPad;

    void Start()
    {
        buttonPad.onRelease.AddListener(SelectItem);
    }

    private void SelectItem()
    {
        
    }
}
