using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(menuName = "InventoryData/GenericItemDataList")]
public class GenericItemDataList : ScriptableObject
{
    public List<GenericItemDataObject> genericItems = new List<GenericItemDataObject>();

    public GenericItemData ReturnItemData(string id)
    {
        return genericItems.FirstOrDefault(i => i.data.id == id).data;
    }
}
