using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableFood : MonoBehaviour, InteractableObj
{
    public void Interact()
    {
        Actor.i.hunger.EatFood(33);

        Destroy(this.gameObject);
    }
}
