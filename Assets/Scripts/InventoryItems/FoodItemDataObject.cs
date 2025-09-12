using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "InventoryData/FoodItemDataObject")]
public class FoodItemDataObject : ScriptableObject
{
    public string itemID;
    public int hungerHealed;
    public int healthHealed;
    public Sprite sprite;
}
