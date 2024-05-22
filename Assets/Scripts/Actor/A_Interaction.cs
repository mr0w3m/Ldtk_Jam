using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A_Interaction : MonoBehaviour
{
    [SerializeField] private A_Input _input;
    [SerializeField] private A_Collision _collision;
    [SerializeField] private A_Inventory _inventory;
    [SerializeField] private LayerMask _interactableLayer;
    [SerializeField] private A_Crafting _crafting;
    [SerializeField] private GameObject _button;

    InteractableObj tempInteractableObj = null;

    private bool _colliding;


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

        if (_colliding)
        {
            tempInteractableObj = hitCollider.GetComponent<InteractableObj>();
            if (tempInteractableObj != null)
            {
                _button.SetActive(true);
            }
        }
        else
        {
            tempInteractableObj = null;
            _button.SetActive(false);
        }
    }


    
    

    private void TryInteract()
    {
        if (_crafting.crafting && !_colliding)
        {
            return;
        }


        if (tempInteractableObj != null)
        {
            tempInteractableObj.Interact();
        }
        else
        {

        }
    }
}
