using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A_Lunchbox : MonoBehaviour
{
    [SerializeField] private FoodItemDatabase _foodDatabase;
    [SerializeField] private AudioClip _eatClip;
    [SerializeField] private AudioClip _dmgClip;

    public bool holdingLunchbox = false;
    public bool foodItemHeld
    {
        get
        {
            return _foodItemHeld;
        }
    }

    private bool _foodItemHeld = false;

    public string foodItemString
    {
        get { return _foodItemString; }
    }
    private string _foodItemString;
    
    private void Start()
    {
        Actor.i.inventory.ItemAdded += CheckAdd;
        Actor.i.inventory.ItemRemoved += CheckRemove;

    }

    private void CheckAdd(string id)
    {
        if (!holdingLunchbox && id == "lunchbox")
        {
            holdingLunchbox = true;
            Debug.Log("Added Lunchbox");
            Actor.i.input.RStickDown += UseLunchbox;
            Actor.i.inventoryUI.AddLunchBox();
            Actor.i.inventoryUI.UpdateLunchBoxItem();
        }
    }

    private void CheckRemove(string id)
    {
        if (holdingLunchbox && id == "lunchbox")
        {
            
            holdingLunchbox = false;
            Debug.Log("Removed Lunchbox");
            Actor.i.inventoryUI.RemoveLunchBox();
            Actor.i.input.RStickDown -= UseLunchbox;
        }
    }

    private void UseLunchbox()
    {
        //lunchbox item used or store lunchbox item
        if (Actor.i.crafting.crafting && Actor.i.interaction.ReturnInteraction() != null)
        {
            Debug.Log("Can't Use Lunchbox Now");
            return;
        }

        
        if (_foodItemHeld)
        {
            //eat food item
            UseFoodItem();
            Debug.Log("Use Food Item");
        }
    }

    public void ForceAddLunchbox()
    {
        holdingLunchbox = true;
        Debug.Log("Added Lunchbox");
        Actor.i.input.RStickDown += UseLunchbox;
        Actor.i.inventoryUI.AddLunchBox();
        Actor.i.inventoryUI.UpdateLunchBoxItem();
    }

    public void AddFoodItem(string id)
    {
        Debug.Log("Added Food Item: " + id);
        _foodItemHeld = true;
        _foodItemString = id;
        Actor.i.inventoryUI.UpdateLunchBoxItem();
        //go into inventory and slap an icon on top of the lunchbox icon.
        //also lunchbox should have a y button when we have an item
        //we're going to need a way to custom spawn in inventory slots and sort them
    }

    private void UseFoodItem()
    {
        FoodItemDataObject data = _foodDatabase.ReturnFoodItem(_foodItemString);

        AudioController.control.PlayClip(_eatClip, Random.Range(1.3f, 1.6f), 0.5f);
        Actor.i.hunger.EatFood(data.hungerHealed);
        Actor.i.health.AddHP(data.healthHealed);


        if (data.hungerHealed < 0 || data.healthHealed < 0)
        {
            AudioController.control.PlayClip(_dmgClip);
        }
        _foodItemHeld = false;
        _foodItemString = "";
        Actor.i.inventoryUI.UpdateLunchBoxItem();
    }
}
