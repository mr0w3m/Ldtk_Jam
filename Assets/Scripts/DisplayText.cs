using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DisplayText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;

    public void Init(string text, float time)
    {
        _text.text = text;
        Destroy(this.gameObject, time);
    }
}