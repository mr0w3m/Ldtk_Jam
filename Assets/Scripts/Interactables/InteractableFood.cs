using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableFood : MonoBehaviour, InteractableObj
{
    public int hungerHealed = 33;
    public int hpHealed = 1;

    [SerializeField] private AudioClip _eatClip;
    [SerializeField] private AudioClip _dmgClip;

    [SerializeField] private SpriteRenderer _sr;
    [SerializeField] private Material _highlightMat;
    [SerializeField] private Material _normalMat;

    [SerializeField] private GameObject _parentToDestroy;

    private GameObject _ObjToDestroy;

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

    public void Interact()
    {
        AudioController.control.PlayClip(_eatClip, Random.Range(0.7f, 1.3f), 0.5f);
        Actor.i.hunger.EatFood(hungerHealed);
        Actor.i.health.AddHP(hpHealed);

        
        if (hungerHealed < 0 || hpHealed < 0)
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
