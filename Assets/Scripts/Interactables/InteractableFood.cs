using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableFood : MonoBehaviour, InteractableObj
{
    [SerializeField] private FoodItemDataObject _foodData;

    [SerializeField] private AudioClip _eatClip;
    [SerializeField] private AudioClip _dmgClip;
    [SerializeField] private AudioClip _pickupClip;

    [SerializeField] private SpriteRenderer _sr;
    [SerializeField] private Material _highlightMat;
    [SerializeField] private Material _normalMat;

    [SerializeField] private GameObject _parentToDestroy;

    private GameObject _ObjToDestroy;

    public FoodItemDataObject foodData
    {
        get
        {
            return _foodData;
        }
    }

    private void Start()
    {
        if (_parentToDestroy != null)
        {
            _ObjToDestroy = _parentToDestroy;
        }
        else
        {
            _parentToDestroy = this.gameObject;
        }
    }

    public void PickUp()
    {
        AudioController.control.PlayClip(_pickupClip);
        Destroy(_parentToDestroy);
    }

    public void Interact()
    {
        AudioController.control.PlayClip(_eatClip, Random.Range(1.3f, 1.6f), 0.5f);
        Actor.i.hunger.EatFood(foodData.hungerHealed);
        Actor.i.health.AddHP(foodData.healthHealed);

        
        if (foodData.hungerHealed < 0 || foodData.healthHealed < 0)
        {
            AudioController.control.PlayClip(_dmgClip);
        }

        Destroy(_parentToDestroy);
    }


    public void Highlight(bool state)
    {
        if (_sr != null)
        {
            _sr.material = state ? _highlightMat : _normalMat;
        }
    }
}
