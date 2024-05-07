using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class A_InventoryUI : MonoBehaviour
{
    [SerializeField] private A_Throwing _throwing;
    [SerializeField] private A_Inventory _inventory;
    [SerializeField] private A_Input _input;
    [SerializeField] private InventorySlot _invSlotPrefab;
    [SerializeField] private Transform _invSlotParent;
    [SerializeField] private GenericItemDataList _itemDatabase;

    private List<InventorySlot> _inventorySlots = new List<InventorySlot>();
    private int _selectedInt = 0;
    public int selectedInt
    {
        get { return _selectedInt; }
    }


    public Action inventoryItemSelected;

    private void OnInventoryItemSelected()
    {
        if (inventoryItemSelected != null)
        {
            inventoryItemSelected.Invoke();
        }
    }


    private void Start()
    {
        _inventory.InventoryUpdated += UpdateUI;
        _input.RBDown += TabRight;
        _input.LBDown += TabLeft;

        for (int i = 0; i < _inventory.totalInventoryCount; i++)
        {
            InventorySlot slot = Instantiate(_invSlotPrefab, _invSlotParent);
            slot.Init(null);
            _inventorySlots.Add(slot);
        }

        _inventorySlots[0].Select(true);
    }

    private void UpdateUI()
    {
        for (int i = 0; i < _inventory.totalInventoryCount; i++)
        {
            if (_inventory.inventoryItemStrings.Count >= (i+1))
            {
                _inventorySlots[i].Init(_itemDatabase.ReturnItemData(_inventory.inventoryItemStrings[i]));
            }
            else
            {
                _inventorySlots[i].Init(null);
            }
        }
        _inventorySlots[_selectedInt].Select(true);
    }

    private void TabRight()
    {
        if (_throwing.throwing)
        {
            return;
        }
        ClearInventorySelection();

        if (_selectedInt >= _inventory.totalInventoryCount -1)
        {
            _selectedInt = 0;
        }
        else
        {
            _selectedInt++;
        }
        _inventorySlots[_selectedInt].Select(true);

        OnInventoryItemSelected();
    }
    private void TabLeft()
    {
        if (_throwing.throwing)
        {
            return;
        }
        ClearInventorySelection();

        if (_selectedInt <= 0)
        {
            _selectedInt = _inventory.totalInventoryCount - 1;
        }
        else
        {
            _selectedInt--;
        }
        _inventorySlots[_selectedInt].Select(true);
        OnInventoryItemSelected();
    }
    

    private void ClearInventorySelection()
    {
        _inventorySlots.ForEach(s => s.Select(false));
    }


    public bool SelectedSlotOccupied()
    {
        return _inventorySlots[_selectedInt].occupied;
    }

    public void RefreshSelectSlot()
    {
        _inventorySlots[_selectedInt].Select(true);
    }
}
