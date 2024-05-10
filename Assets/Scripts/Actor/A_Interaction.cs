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

    private bool _interactableDetected = false;
    InteractableObj tempInteractableObj = null;
    Collider2D hitCollider = null;


    private void Start()
    {
        _input.XDown += TryInteract;
    }

    private void Update()
    {
        ShowInteractor();
    }

    
    private void ShowInteractor()
    {
        hitCollider = Physics2D.OverlapBox(new Vector3
            (_collision.GetTrigger.transform.position.x + _collision.GetTrigger.offset.x,
            _collision.GetTrigger.transform.position.y + _collision.GetTrigger.offset.y,
            _collision.GetTrigger.transform.position.z),
            _collision.GetTrigger.size, 0, _interactableLayer);

        if (hitCollider != null)
        {
            tempInteractableObj = hitCollider.GetComponent<InteractableObj>();
        }
        else
        {
            tempInteractableObj = null;
        }

        if (tempInteractableObj != null && !_interactableDetected)
        {
            _interactableDetected = true;
        }
        else if (tempInteractableObj == null && _interactableDetected == true)
        {
            _interactableDetected = false;
        }

        _button.SetActive(_interactableDetected);
    }

    private void TryInteract()
    {
        Debug.Log("Try interact");
        if (_crafting.crafting)
        {
            return;
        }

        InteractableObj tempObj = Select();
        if (tempObj != null)
        {
            Debug.Log("tempObjectFound");
            tempObj.Interact();
        }
    }

    private InteractableObj Select()
    {
        InteractableObj tempInteractableObj = null;
        BoxCollider2D coll = _collision.GetTrigger;
        //Collider2D[] hitColliders = Physics2D.OverlapBoxAll((coll.transform.position + new Vector3(coll.size.x, coll.size.y, 0)), coll.size, 0, _interactableLayer);
        Collider2D hitCollider = Physics2D.OverlapBox(new Vector3 (coll.transform.position.x+coll.offset.x, coll.transform.position.y + coll.offset.y, coll.transform.position.z), coll.size, 0, _interactableLayer);
        if (hitCollider != null)
        {

            tempInteractableObj = hitCollider.GetComponent<InteractableObj>();
        }
        return tempInteractableObj;
    }
}
