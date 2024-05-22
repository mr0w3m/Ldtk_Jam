using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class A_CraftingUI : MonoBehaviour
{
    [SerializeField] private GenericItemDataList _itemDatabase;
    [SerializeField] private GameObject _craftingUIParent;
    [SerializeField] private GameObject _craftableParent;
    [SerializeField] private AudioClip _openCrafting, _closeCrafting;

    [SerializeField] private Image _craftingItemL, _craftingItemR, _potentialCraftedItem;

    public void SetUIState( bool state)
    {
        _craftingUIParent.SetActive(state);
        AudioController.control.PlayClip(state == true ? _openCrafting : _closeCrafting);
        _craftingItemL.sprite = null;
        _craftingItemL.enabled = false;
        _craftingItemR.sprite = null;
        _craftingItemR.enabled = false;
        _potentialCraftedItem.sprite = null;
        _craftingItemL.enabled = false;
        _craftableParent.SetActive(false);
    }

    public void SetIngredientItem(bool left, string id)
    {
        //find img w id
        if (left)
        {
            if(string.IsNullOrEmpty(id))
            {
                _craftingItemL.enabled = false;
            }
            else
            {
                _craftingItemL.sprite = _itemDatabase.ReturnItemData(id).sprite;
                _craftingItemL.enabled = true;
            }
            
        }
        else
        {
            if (string.IsNullOrEmpty(id))
            {
                _craftingItemR.enabled = false;
            }
            else
            {
                _craftingItemR.sprite = _itemDatabase.ReturnItemData(id).sprite;
                _craftingItemR.enabled = true;
            }
            
        }
    }

    public void SetCraftedItem(string id)
    {
        //find image for id
        if (string.IsNullOrEmpty(id))
        {
            _potentialCraftedItem.enabled = false;

        }
        else
        {
            _potentialCraftedItem.sprite = _itemDatabase.ReturnItemData(id).sprite;
            _potentialCraftedItem.enabled = true;
        }
    }

    public void ShowCraftable()
    {
        _craftableParent.SetActive(true);
    }
}
