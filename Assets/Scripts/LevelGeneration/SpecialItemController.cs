using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialItemController : MonoBehaviour
{
    public static SpecialItemController i;

    private List<ItemSpawner> _itemSpawners;

    private void Awake()
    {
        if (i == null)
        {
            i = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
