using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A_Death : MonoBehaviour
{
    [SerializeField] private GameObject _yourdeadParent;
    [SerializeField] private SpriteRenderer _playerSprite;
    [SerializeField] private Sprite _deadSprite;
    [SerializeField] private Sprite _revivedSprite;
    [SerializeField] private AudioClip _dieClip;
    [SerializeField] private AudioClip _reviveClip;

    public bool playerDead = false;


    public event Action playerDied;

    public void Revive()
    {
        _yourdeadParent.SetActive(false);
        _playerSprite.sprite = _revivedSprite;
        playerDead = false;
        Actor.i.movement.PauseMovement = false;
        AudioController.control.PlayClip(_reviveClip);
        Actor.i.health.FullHeal();
    }
    
    public void Dead()
    {
        _yourdeadParent.SetActive(true);
        _playerSprite.sprite = _deadSprite;
        playerDead = true;
        Actor.i.movement.PauseMovement = true;
        AudioController.control.PlayClip(_dieClip);

        if (playerDied != null)
        {
            playerDied.Invoke();
        }
    }
}
