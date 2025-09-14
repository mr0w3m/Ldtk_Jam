using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

        List<string> saveItemList = PlayerSaveManager.i.playerSaveData.items.Where(i => i == "lunchbox").ToList();
        if (saveItemList.Count > 0)
        {
            Debug.Log("We Have A lunchbox");
            CheckAdd(saveItemList[0]);
            if (!string.IsNullOrEmpty(PlayerSaveManager.i.playerSaveData.lunchboxItem))
            {
                AddFoodItem(PlayerSaveManager.i.playerSaveData.lunchboxItem);
            }
        }
        else
        {
            Debug.Log("We do not have a lunchbox");
        }
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
        if (Actor.i.crafting.crafting || Actor.i.death.playerDead)
        {
            Debug.Log("Can't Use Lunchbox Now");
            return;
        }

        if (!_foodItemHeld)
        {
            Debug.Log("if not holding food item, LunchBoxInteract");
            Actor.i.interaction.LunchBoxInteract();
        }
        else
        {
            //eat food item
            UseFoodItem();
            Debug.Log("Use Food Item");
        }
    }

    public void AddFoodItem(string id)
    {
        Debug.Log("Added Food Item: " + id);
        _foodItemHeld = true;
        _foodItemString = id;
        Actor.i.inventoryUI.UpdateLunchBoxItem();
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
