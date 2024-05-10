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

    private bool _crafting;

    private string _ingredientA;
    private string _ingredientB;

    private int _craftingStep = 0;
    private int _ing1Index = -1;

    private float _afterCraftingCooldown;
    private float _afterCraftingCooldownTime = 0.2f;

    public bool crafting
    {
        get 
        { 
            if (_afterCraftingCooldown > 0)
            {
                return true;
            }
            else if (_crafting)
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
        //_input.SelectDown += ToggleCrafting;
        _input.StartDown += ToggleCrafting;
        _input.ADown += SelectItem;
        _input.XDown += Craft;
    }
    
    private void Update()
    {
        if (_afterCraftingCooldown > 0)
        {
            _afterCraftingCooldown -= Time.deltaTime;
        }
    }

    private void OnCraftingEnded()
    {
        _afterCraftingCooldown = _afterCraftingCooldownTime;
    }

    private void ToggleCrafting()
    {
        if (!_crafting)
        {
            OpenCrafting();
        }
        else
        {
            OnCraftingEnded();
            CloseCrafting();
        }
        _ui.SetUIState(_crafting);
    }

    private void OpenCrafting()
    {
        if (!_collision.Grounded)
        {
            return;
        }
        _crafting = true;
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
        if ( _craftingStep == 3)
        {
            _inventory.RemoveItem(_ingredientA);
            _inventory.RemoveItem(_ingredientB);
            _inventory.AddItemToInventory(ReturnCraftedItem());
            ToggleCrafting();
        }
    }


    private string ReturnCraftedItem()
    {
        string returnItem = "";
        if ((_ingredientA == "stick" && _ingredientB == "rock") || (_ingredientA == "rock" && _ingredientB == "stick"))
        {
            return "spear";
        }
        else if ((_ingredientA == "rope" && _ingredientB == "rock") || (_ingredientA == "rock" && _ingredientB == "rope"))
        {
            return "anchor";
        }
        else if ((_ingredientA == "stick" && _ingredientB == "rope") || (_ingredientA == "rope" && _ingredientB == "stick"))
        {
            return "torch";
        }
        return returnItem;
    }
}
