using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class A_Inventory : MonoBehaviour
{
    [SerializeField] private A_InventoryUI _ui;

    public event Action InventoryUpdated;
    //reference to database where these items and their associated sprites can be referenced
    [SerializeField] private List<string> _inventoryItemStrings = new List<string>();

    private int _totalInventoryCount;
    public int totalInventoryCount
    {
        get { return _totalInventoryCount; }
        set 
        { 
            _totalInventoryCount = value;
            _ui.ReInitialize();
        }
    }

    public List<string> inventoryItemStrings
    {
        get
        {
            return _inventoryItemStrings;
        }
    }

    public event Action DataLoaded;

    private void OnDataLoaded()
    {
        if (DataLoaded != null)
        {
            DataLoaded.Invoke();
        }
    }

    private void OnInventoryUpdated()
    {
        if (InventoryUpdated != null)
        {
            InventoryUpdated.Invoke();
        }
    }

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        _totalInventoryCount = PlayerSaveManager.i.playerSaveData.totalItemSlots;
        _inventoryItemStrings = new List<string>(PlayerSaveManager.i.playerSaveData.items);
        OnDataLoaded();
        OnInventoryUpdated();
    }

    private void AddItem(string id)
    {
        _inventoryItemStrings.Add(id);
        OnInventoryUpdated();
    }

    public bool AddItemToInventory(string id)
    {
        if (_inventoryItemStrings.Count >= _totalInventoryCount)
        {
            Debug.Log("Max items picked up, drop one");
            return false;
        }
        else
        {
            //add item
            AddItem(id);
            return true;
        }
    }

    public void RemoveItemSelected()
    {
        _inventoryItemStrings.RemoveAt(_ui.selectedInt);
        OnInventoryUpdated();
    }

    public void RemoveItem(int index)
    {
        _inventoryItemStrings.RemoveAt(index);
        OnInventoryUpdated();
    }

    public void RemoveItem(string id)
    {
        if (_inventoryItemStrings.Contains(id))
        {
            _inventoryItemStrings.Remove(id);
            OnInventoryUpdated();
        }
    }

    public bool IsFull()
    {
        return _inventoryItemStrings.Count >= _totalInventoryCount;
    }

    public bool SelectedItemNotNull()
    {
        return _ui.SelectedSlotOccupied();
    }

    public string ReturnSelectedItem()
    {
        if (!_ui.SelectedSlotOccupied())
        {
            return "";
        }
        else
        {
            return _inventoryItemStrings[_ui.selectedInt];
        }
    }
}
