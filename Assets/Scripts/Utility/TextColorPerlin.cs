using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TextColorPerlin : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private Color _defaultColor;

    private float _speed = 3;
    private float _timeOffset = 0;

    private void Update()
    {
        _timeOffset += (_speed * Time.deltaTime);
        _text.color = new Color(_defaultColor.r, _defaultColor.g, _defaultColor.b, Util.MapValue(Mathf.Sin(_timeOffset), 1, 0, 0.2f, 1));
    }
}
