using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class A_Backpack : MonoBehaviour
{
    public bool holdingbackpack = false;

    private void Start()
    {
        Actor.i.inventory.ItemAdded += CheckAdd;
        Actor.i.inventory.ItemRemoved += CheckRemove;

        //dont need to do this because the inventory slots are increased and saved...
        
        List<string> saveItemList = PlayerSaveManager.i.playerSaveData.items.Where(i => i == "backpack").ToList();
        if (saveItemList.Count > 0)
        {
            Debug.Log("We Have A backpack");
            //CheckAdd(saveItemList[0]);
            holdingbackpack = true;
        }
        else
        {
            Debug.Log("We do not have a backpack");
        }
        
    }

    private void CheckAdd(string id)
    {
        if (!holdingbackpack && id == "backpack")
        {
            holdingbackpack = true;
            Debug.Log("Added backpack");
            AddBackpack();
        }

    }

    private void CheckRemove(string id)
    {
        if (holdingbackpack && id == "backpack")
        {

            holdingbackpack = false;
            Debug.Log("Removed backpack");
            RemoveBackpack();
        }
    }

    private void AddBackpack()
    {
        Actor.i.inventory.totalInventoryCount += 2;
    }

    private void RemoveBackpack()
    {
        Actor.i.inventory.totalInventoryCount -= 2;


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
