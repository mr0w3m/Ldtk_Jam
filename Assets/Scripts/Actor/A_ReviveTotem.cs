using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A_ReviveTotem : MonoBehaviour
{
    public bool _holdingReviveTotem;

    private bool _usedTotem = false;

    private void Start()
    {
        Actor.i.inventory.ItemAdded += CheckAdd;
        Actor.i.inventory.ItemRemoved += CheckRemove;
    }

    private void CheckAdd(string id)
    {
        if (!_holdingReviveTotem && id == "revivetotem")
        {
            _holdingReviveTotem = true;
            Actor.i.death.playerDied += UseTotem;
            Debug.Log("Added Totem");
        }
    }

    private void CheckRemove(string id)
    {
        if (_holdingReviveTotem && id == "revivetotem")
        {
            Actor.i.death.playerDied -= UseTotem;
            _holdingReviveTotem = false;
            Debug.Log("Removed Totem");
        }
    }

    private void UseTotem()
    {
        //Use the totem, revive the player in their place
        Actor.i.hunger.EatFood(Actor.i.hunger.totalStartHunger);
        Actor.i.death.Revive();
        Actor.i.inventory.RemoveItem("revivetotem");
        _holdingReviveTotem = false;
    }
}
