using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerSaveData
{
    public int hp;
    public int totalHp;
    public int hunger;
    public int totalHunger;
    public int totalItemSlots;
    public List<string> items;

    public PlayerSaveData()
    {
        this.hp = 2;
        this.totalHp = 2;
        this.hunger = 50;
        this.totalHunger = 50;
        this.totalItemSlots = 2;
        this.items = new List<string>();
    }
    
    public PlayerSaveData(PlayerSaveData data)
    {
        this.hp = data.hp;
        this.totalHp = data.totalHp;
        this.hunger = data.hunger;
        this.totalHunger= data.totalHunger;
        this.totalItemSlots= data.totalItemSlots;
        this.items = new List<string>(data.items);
    }

    public PlayerSaveData(int hp, int totalHp, int hunger, int totalHunger, int totalItemSlots, List<string> items)
    {
        this.hp = hp;
        this.totalHp = totalHp;
        this.hunger = hunger;
        this.totalHunger = totalHunger;
        this.totalItemSlots = totalItemSlots;
        this.items = new List<string>(items);
    }

}