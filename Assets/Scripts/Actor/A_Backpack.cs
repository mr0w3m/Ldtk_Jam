using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A_Backpack : MonoBehaviour
{
    public bool holdingbackpack = false;

    private void Start()
    {
        Actor.i.inventory.ItemAdded += CheckAdd;
        Actor.i.inventory.ItemRemoved += CheckRemove;
    }

    private void CheckAdd(string id)
    {
        if (!holdingbackpack && id == "backpack")
        {
            holdingbackpack = true;
            Debug.Log("Added backpack");
            AddBackpack();
        }

        Debug.Log("Check Add");
    }

    private void CheckRemove(string id)
    {
        if (holdingbackpack && id == "backpack")
        {

            holdingbackpack = false;
            Debug.Log("Removed backpack");
            RemoveBackpack();
        }
        Debug.Log("Check Remove");
    }

    private void AddBackpack()
    {
        Actor.i.inventory.totalInventoryCount += 2;
    }

    private void RemoveBackpack()
    {
        Actor.i.inventory.totalInventoryCount -= 2;

        Debug.Log("Player inventory item count: " + (Actor.i.inventory.inventoryItemStrings.Count - 1));
        Debug.Log("Total inventory: " + Actor.i.inventory.totalInventoryCount);

        if (Actor.i.inventory.inventoryItemStrings.Count - 1 > Actor.i.inventory.totalInventoryCount)
        {
            //drop extra items
            for (int i = 0; i < ((Actor.i.inventory.inventoryItemStrings.Count - 1) - Actor.i.inventory.totalInventoryCount); i++)
            {
                Actor.i.throwing.DropItem(Actor.i.inventory.inventoryItemStrings[i]);
            }
        }
    }
}
