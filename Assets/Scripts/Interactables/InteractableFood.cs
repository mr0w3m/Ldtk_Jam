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


    public void Interact()
    {
        Actor.i.hunger.EatFood(hungerHealed);
        Actor.i.health.AddHP(hpHealed);

        
        if (hungerHealed < 0 || hpHealed < 0)
        {
            AudioController.control.PlayClip(_dmgClip);
        }
        else
        {
            AudioController.control.PlayClip(_eatClip);
        }

        Destroy(this.gameObject);
    }

    public void Highlight(bool state)
    {
        if (_sr != null)
        {
            _sr.material = state ? _highlightMat : _normalMat;
        }
    }
}
