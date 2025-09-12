using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySlot_Lunchbox : InventorySlot
{
    [SerializeField] private GameObject _inputIcon;

    public void ToggleInputIcon(bool state)
    {
        _inputIcon.SetActive(state);
    }
}
