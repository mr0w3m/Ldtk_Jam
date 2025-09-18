using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class A_Crafting : MonoBehaviour
{
    [SerializeField] private A_Input _input;
    [SerializeField] private A_Inventory _inventory;
    [SerializeField] private A_InventoryUI _inventoryUI;
    [SerializeField] private A_CraftingUI _ui;
    [SerializeField] private A_Collision _collision;
    [SerializeField] private AudioClip _craftClip;
    [SerializeField] private AudioClip _failCraftClip;

    private bool _crafting;

    private string _ingredientA;
    private string _ingredientB;

    private int _craftingStep = 0;
    private int _ing1Index = -1;


    public bool crafting
    {
        get
        {
            if (_crafting)
            {
                return true;
            }
            else //crafting is false and cooldown is less than 0 we good
            {
                return false;
            }
        }
    }

    private void Start()
    {
        _input.YDown += ToggleCrafting;
        _input.ADown += SelectItem;
        _input.YDown += Craft;
    }
    
    private void ToggleCrafting()
    {
        if (!_crafting)
        {
            OpenCrafting();
        }
        else if (_craftingStep != 3)
        {
            CloseCrafting();
        }
    }

    private void OpenCrafting()
    {
        if (!_collision.Grounded)
        {
            return;
        }
        _crafting = true;
        _ui.SetUIState(_crafting);
        Actor.i.paused = _crafting;
    }

    private void CloseCrafting()
    {
        _ingredientA = "";
        _ingredientB = "";
        _ui.SetIngredientItem(true, _ingredientA);
        _ui.SetIngredientItem(false, _ingredientB);
        _ui.SetCraftedItem("");
        _craftingStep = 0;
        _ing1Index = -1;
        _crafting = false;
        _ui.SetUIState(_crafting);
        Actor.i.paused = _crafting;
    }

    private void SelectItem()
    {
        if (!_crafting)
        {
            return;
        }

        if (!_inventory.SelectedItemNotNull())
        {
            return;
        }

        if (_craftingStep == 0)
        {
            _ingredientA = _inventory.ReturnSelectedItem();
            _ing1Index = _inventoryUI.selectedInt;
            _ui.SetIngredientItem(true, _ingredientA);
            _craftingStep = 1;
            _inventoryUI.TabRight();
        }
        else if (_craftingStep == 1)
        {
            if (_ing1Index != _inventoryUI.selectedInt)
            {
                _ingredientB = _inventory.ReturnSelectedItem();
                _ui.SetIngredientItem(false, _ingredientB);
                _craftingStep = 2;

                if (!string.IsNullOrEmpty( ReturnCraftedItem()))
                {
                    _ui.SetCraftedItem(ReturnCraftedItem());
                    _ui.ShowCraftable();
                    _craftingStep = 3;
                }
                else
                {
                    ToggleCrafting();
                    AudioController.control.PlayClip(_failCraftClip);
                }
            }
            else
            {
                Debug.Log("Can't select the same item!");
            }
        }
    }

    private void Craft()
    {
        if (_craftingStep == 3)
        {
            _inventory.RemoveItem(_ingredientA);
            _inventory.RemoveItem(_ingredientB);
            _inventory.AddItemToInventory(ReturnCraftedItem());
            AudioController.control.PlayClip(_craftClip);
            CloseCrafting();
        }
    }


    private string ReturnCraftedItem()
    {
        string returnItem = "";
        if ((_ingredientA == "stick" && _ingredientB == "rock") || (_ingredientA == "rock" && _ingredientB == "stick"))
        {
            return "spear";
        }
        else if ((_ingredientA == "rope" && _ingredientB == "spear") || (_ingredientA == "spear" && _ingredientB == "rope"))
        {
            return "ropespear";
        }
        else if ((_ingredientA == "rope" && _ingredientB == "rock") || (_ingredientA == "rock" && _ingredientB == "rope"))
        {
            return "anchor";
        }
        else if ((_ingredientA == "stick" && _ingredientB == "rope") || (_ingredientA == "rope" && _ingredientB == "stick"))
        {
            return "torch";
        }
        else if (_ingredientA == "stick" && _ingredientB == "stick")
        {
            return "doublestick";
        }
        else if (_ingredientA == "rock" && _ingredientB == "rock")
        {
            return "doublerock";
        }
        else if (_ingredientA == "rock" && _ingredientB == "doublestick")
        {
            return "spear";
        }
        else if (_ingredientA == "doublestick" && _ingredientB == "rock")
        {
            return "spear";
        }
        return returnItem;
    }
}
