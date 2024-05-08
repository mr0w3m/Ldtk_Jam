using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour
{
    public static Actor i;

    private void Awake()
    {
        if (i == null)
        {
            i = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public A_Inventory inventory;
    public A_Movement movement;
    public A_Hunger hunger;
    public A_Death death;
    public A_Input input;
    public A_Crafting crafting;
}
