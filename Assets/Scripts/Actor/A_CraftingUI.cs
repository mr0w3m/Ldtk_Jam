using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class A_CraftingUI : MonoBehaviour
{
    [SerializeField] private GenericItemDataList _itemDatabase;
    [SerializeField] private GameObject _craftingUIParent;
    [SerializeField] private GameObject _craftableParent;

    [SerializeField] private Image _craftingItemL, _craftingItemR, _potentialCraftedItem;

    public void SetUIState( bool state)
    {
        _craftingUIParent.SetActive(state);
        _craftingItemL.sprite = null;
        _craftingItemR.sprite = null;
        _potentialCraftedItem.sprite = null;
        _craftableParent.SetActive(false);
    }

    public void SetIngredientItem(bool left, string id)
    {
        //find img w id
        if (left)
        {
            if(string.IsNullOrEmpty(id))
            {
                _craftingItemL.sprite = null;
            }
            else
            {
                _craftingItemL.sprite = _itemDatabase.ReturnItemData(id).sprite;
            }
            
        }
        else
        {
            if (string.IsNullOrEmpty(id))
            {
                _craftingItemR.sprite = null;
            }
            else
            {
                _craftingItemR.sprite = _itemDatabase.ReturnItemData(id).sprite;
            }
            
        }
    }

    public void SetCraftedItem(string id)
    {
        //find image for id
        if (string.IsNullOrEmpty(id))
        {
            _potentialCraftedItem.sprite = null;
        }
        else
        {
            _potentialCraftedItem.sprite = _itemDatabase.ReturnItemData(id).sprite;
        }
    }

    public void ShowCraftable()
    {
        _craftableParent.SetActive(true);
    }
}
