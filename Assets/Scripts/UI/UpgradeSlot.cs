using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeSlot : MonoBehaviour
{
    [SerializeField] private GameObject _highlight;

    public void Start()
    {
        _highlight.SetActive(false);
    }

    public void Select(bool state)
    {
        _highlight.SetActive(state);
    }
}
