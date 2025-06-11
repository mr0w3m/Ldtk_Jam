using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableResource : MonoBehaviour, InteractableObj
{
    
    [SerializeField] private string _id;
    [SerializeField] private bool _destroyOnInteract = false;
    [SerializeField] private GameObject _parentToDestroy;
    [SerializeField] private AudioClip _interactClip;

    [SerializeField] private SpriteRenderer _sr;
    [SerializeField] private Material _highlightMat;
    [SerializeField] private Material _normalMat;


    public GameObject parentToDestroy
    {
        set { _parentToDestroy = value; }
    }

    public event Action ResourceAcquired;

    public void Interact()
    {
        if (Actor.i.inventory.AddItemToInventory(_id))
        {
            AudioController.control.PlayClip(_interactClip, UnityEngine.Random.Range(0.5f, 1));
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

    public void Highlight(bool state)
    {
        if (_sr != null)
        {
            _sr.material = state ? _highlightMat : _normalMat;
        }
    }
}
