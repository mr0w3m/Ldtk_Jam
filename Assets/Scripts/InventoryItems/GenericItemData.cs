using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GenericItemData
{
    public string id;
    public Sprite sprite;
    public ThrowableObject _throwablePrefab;

    public GenericItemData(GenericItemData data)
    {
        id = data.id;
        sprite = data.sprite;
    }
    public GenericItemData(string s, Sprite sp)
    {
        id = s;
        sprite = sp;
    }

    public GenericItemData(string s)
    {
        id = s;
    }
}