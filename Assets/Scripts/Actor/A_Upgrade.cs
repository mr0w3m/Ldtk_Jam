using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class A_Upgrade : MonoBehaviour
{
    //a class where upgrade menu is managed, perhaps another for the ui but i think it's simple enough\
    // pretty similar to the inventory ui essentially i'm thinking

    [SerializeField] private List<UpgradeSlot> _upgradeSlots = new List<UpgradeSlot>();
    [SerializeField] private GameObject _upgradeSlotsParent;
    [SerializeField] private AudioClip _changeSelectionClip;
    [SerializeField] private AudioClip _selectUpgradeClip;

    private int _selectedInt = 0;


    public event Action SelectedUpgrade;

    private void OnSelectedUpgrade()
    {
        if (SelectedUpgrade != null)
        {
            SelectedUpgrade.Invoke();
        }
    }



    private void Start()
    {
        Actor.i.input.RBDown += TabRight;
        Actor.i.input.LBDown += TabLeft;

        CloseUI();
    }

    public void OpenUI()
    {
        _upgradeSlotsParent.SetActive(true);
        Actor.i.input.ADown += SelectUpgrade;
        Actor.i.inventoryUI.SetUIHideState(true);

        ClearInventorySelection();
        _selectedInt = 1;
        _upgradeSlots[_selectedInt].Select(true);
        AudioController.control.PlayClip(_changeSelectionClip);
    }

    public void CloseUI()
    {
        _upgradeSlotsParent.SetActive(false);
        Actor.i.input.ADown -= SelectUpgrade;
        Actor.i.inventoryUI.SetUIHideState(false);
    }

    private void SelectUpgrade()
    {
        Actor.i.input.ADown -= SelectUpgrade;
        //order of upgrades is the key here since we're not generating them i'll just hard code it for now
        if (_selectedInt == 0)
        {
            Actor.i.inventory.totalInventoryCount += 1;
            Debug.Log("Upgraded Inventory");
        }
        else if (_selectedInt == 1)
        {
            Actor.i.hunger.totalStartHunger += 50;
            Debug.Log("Upgraded MaxHunger");
        }
        else
        {
            Actor.i.health.maxHP += 1;
            Debug.Log("Upgraded max HP");
        }
        OnSelectedUpgrade();
        AudioController.control.PlayClip(_selectUpgradeClip);
    }

    private void TabRight()
    {
        ClearInventorySelection();

        if (_selectedInt >= _upgradeSlots.Count - 1)
        {
            _selectedInt = 0;
        }
        else
        {
            _selectedInt++;
        }
        _upgradeSlots[_selectedInt].Select(true);

        AudioController.control.PlayClip(_changeSelectionClip);
    }
    private void TabLeft()
    {
        ClearInventorySelection();

        if (_selectedInt <= 0)
        {
            _selectedInt = _upgradeSlots.Count - 1;
        }
        else
        {
            _selectedInt--;
        }
        _upgradeSlots[_selectedInt].Select(true);

        AudioController.control.PlayClip(_changeSelectionClip);
    }


    private void ClearInventorySelection()
    {
        _upgradeSlots.ForEach(s => s.Select(false));
    }
}
