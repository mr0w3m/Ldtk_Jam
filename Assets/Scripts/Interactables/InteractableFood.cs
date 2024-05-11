using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableFood : MonoBehaviour, InteractableObj
{
    public int hungerHealed = 33;
    public int hpHealed = 1;

    public void Interact()
    {
        Actor.i.hunger.EatFood(hungerHealed);
        Actor.i.health.AddHP(hpHealed);

        Destroy(this.gameObject);
    }
}
