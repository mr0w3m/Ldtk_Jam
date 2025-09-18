using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlertUI_Module : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private Image _img;

    private float _timeToDeath = 1f;

    public void Init(string s, Sprite sprite)
    {
        _text.text = s;
        _img.sprite = sprite;
    }

    private void Update()
    {
        //transform.position = Vector2.MoveTowards(transform.position, (Vector2)transform.position + Vector2.up, Time.deltaTime);
        if (_timeToDeath > 0)
        {

            _timeToDeath -= Time.deltaTime;

        }
        else
        {
            RemoveThis();
        }
    }

    private void RemoveThis()
    {
        Destroy(this.gameObject);
    }
}
