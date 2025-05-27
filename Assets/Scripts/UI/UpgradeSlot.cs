using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeSlot : MonoBehaviour
{
    [SerializeField] private GameObject _highlight;

    private bool _selectedState = false;

    public void Start()
    {
        _highlight.SetActive(_selectedState);
    }

    public void Select(bool state)
    {
        _selectedState = state;
        _highlight.SetActive(_selectedState);
    }
}
