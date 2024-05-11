using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPUISlot : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private Sprite _heartFullSprite;
    [SerializeField] private Sprite _heartEmptySprite;

    public bool heartFull = true;

    public void Start()
    {
        _image.sprite = _heartFullSprite;
    }

    public void SetHeartState(bool state)
    {
        _image.sprite = state ? _heartFullSprite : _heartEmptySprite;
        heartFull = state;
    }
}
