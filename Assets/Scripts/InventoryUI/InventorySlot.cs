using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    [SerializeField] private Image _sprite;
    [SerializeField] private GameObject _highlight;

    private bool _occupied;
    public bool occupied
    {
        get { return _occupied; }
    }

    public void Init(GenericItemData data)
    {
        if (data != null)
        {
            _occupied = true;
            SetSprite(data.sprite);
        }
        else
        {
            _occupied = false;
            SetSprite(null);
        }
        _highlight.SetActive(false);
    }
    public void Init(string id, Sprite sprite)
    {
        if (!string.IsNullOrEmpty(id))
        {
            _occupied = true;
            SetSprite(sprite);
        }
        else
        {
            _occupied = false;
            SetSprite(null);
        }
        _highlight.SetActive(false);
    }

    public void Select(bool state)
    {
        _highlight.SetActive(state);
    }

    private void SetSprite(Sprite s)
    {
        if (s != null)
        {
            _sprite.sprite = s;
            _sprite.enabled = true;
        }
        else
        {
            _sprite.enabled = false;
        }
    }
}
