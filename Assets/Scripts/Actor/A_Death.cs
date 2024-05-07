using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A_Death : MonoBehaviour
{
    [SerializeField] private GameObject _yourdeadParent;
    [SerializeField] private SpriteRenderer _playerSprite;
    [SerializeField] private Sprite _deadSprite;

    public bool playerDead = false;


    public event Action playerDied;


    public void Dead()
    {
        _yourdeadParent.SetActive(true);
        _playerSprite.sprite = _deadSprite;
        playerDead = true;
        Actor.i.movement.PauseMovement = true;

        if (playerDied != null)
        {
            playerDied.Invoke();
        }
    }
}
