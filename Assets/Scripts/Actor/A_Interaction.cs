using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.UI;

public class A_Interaction : MonoBehaviour
{
    [SerializeField] private A_Input _input;
    [SerializeField] private A_Collision _collision;
    [SerializeField] private A_Inventory _inventory;
    [SerializeField] private LayerMask _interactableLayer;
    [SerializeField] private A_Crafting _crafting;
    [SerializeField] private GameObject _button;

    InteractableObj prevTempInteractableObj = null;
    InteractableObj tempInteractableObj = null;

    private bool _colliding;
    private GameObject _collidedObject;


    private void Start()
    {
        _input.XDown += TryInteract;
    }

    private void Update()
    {
        CheckRoutine();
    }

    private void CheckRoutine()
    {
        Collider2D hitCollider = Physics2D.OverlapBox(new Vector3
            (_collision.GetTrigger.transform.position.x + _collision.GetTrigger.offset.x,
            _collision.GetTrigger.transform.position.y + _collision.GetTrigger.offset.y,
            _collision.GetTrigger.transform.position.z),
            _collision.GetTrigger.size, 0, _interactableLayer);


        if (hitCollider != null)
        {
            if (!_colliding)
            {
                _colliding = true;
            }
        }
        else
        {
            if (_colliding)
            {
                _colliding = false;
            }
        }

        if (hitCollider != null)
        {
            tempInteractableObj = hitCollider.GetComponent<InteractableObj>();
            if (tempInteractableObj != null)
            {
                //set collided gameobject so we can get component on it for later lunchbox system
                _collidedObject = hitCollider.gameObject;

                if (tempInteractableObj != prevTempInteractableObj)
                {
                    if (prevTempInteractableObj != null)
                    {
                        prevTempInteractableObj.Highlight(false);
                    }
                    prevTempInteractableObj = tempInteractableObj;
                }
                _button.SetActive(true);
                tempInteractableObj.Highlight(true);
            }
        }
        else
        {
            if (tempInteractableObj != null)
            {
                tempInteractableObj.Highlight(false);
            }
            tempInteractableObj = null;
            _button.SetActive(false);
        }

        //problem is in the same frame we'll remain colliding and switch the subject. WE need a way to detect if we actually did it
        //we could generate ids for each interactable... on start and check between the two
        //I wonder if unity can compare two 
        /*
        if (_colliding)
        {
            tempInteractableObj = hitCollider.GetComponent<InteractableObj>();
            if (tempInteractableObj != null)
            {
                _button.SetActive(true);
                tempInteractableObj.Highlight(true);
            }
        }
        else
        {
            if (tempInteractableObj != null)
            {
                tempInteractableObj.Highlight(false);
            }
            tempInteractableObj = null;
            _button.SetActive(false);
        }
        */
    }

    public InteractableObj ReturnInteraction()
    {
        if (_crafting.crafting && !_colliding)
        {
            return null;
        }

        return tempInteractableObj;
    }

    
    

    private void TryInteract()
    {
        if (_crafting.crafting && !_colliding)
        {
            return;
        }


        if (tempInteractableObj != null)
        {
            InteractableFood foodItem = _collidedObject.GetComponent<InteractableFood>();
            if (foodItem != null && Actor.i.lunchbox.holdingLunchbox && !Actor.i.lunchbox.foodItemHeld)
            {
                Actor.i.lunchbox.AddFoodItem(foodItem.foodData.itemID);
                foodItem.PickUp();
                //don't interact with it/return.
                return;
            }

            tempInteractableObj.Interact();
        }
        else
        {

        }
    }
}
