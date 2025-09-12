using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "InventoryData/Database_FoodItems")]
public class FoodItemDatabase : ScriptableObject
{
    public List<FoodItemDataObject> foodItems = new List<FoodItemDataObject>();

    public FoodItemDataObject ReturnFoodItem(string id)
    {
        return foodItems.FirstOrDefault(p => p.itemID == id);
    }
}
