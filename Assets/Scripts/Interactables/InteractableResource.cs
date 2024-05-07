using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableResource : MonoBehaviour, InteractableObj
{
    
    [SerializeField] private string _id;
    [SerializeField] private bool _destroyOnInteract = false;
    [SerializeField] private GameObject _parentToDestroy;

    public GameObject parentToDestroy
    {
        set { _parentToDestroy = value; }
    }

    public event Action ResourceAcquired;

    public void Interact()
    {
        Debug.Log("InteractedWith:" + this.gameObject.name);
        
        if (Actor.i.inventory.AddItemToInventory(_id))
        {
            if (_destroyOnInteract)
            {
                if (ResourceAcquired != null)
                {
                    ResourceAcquired.Invoke();
                }
                if (_parentToDestroy != null)
                {
                    Destroy(_parentToDestroy);
                }
                else
                {
                    Destroy(this.gameObject);
                }

            }
        }        
    }
}
