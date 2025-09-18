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

    public A_Interaction interaction;
    public A_Inventory inventory;
    public A_InventoryUI inventoryUI;
    public A_Movement movement;
    public A_Hunger hunger;
    public A_Death death;
    public A_Input input;
    public A_Crafting crafting;
    public A_Health health;
    public A_Throwing throwing;
    public A_Jump jump;
    public A_Collision collision;
    public Actor_Save save;
    public TimedFader_CanvasGroup fader;
    public HUD_Conversation conversation;
    public A_Upgrade upgrade;
    public A_ReviveTotem reviveTotem;
    public A_Lunchbox lunchbox;
    public A_Alerts alerts;


    public Transform playerCenterT;

    public bool paused = false;
    public bool sleeping = false;

}
