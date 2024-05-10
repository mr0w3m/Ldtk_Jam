using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Bat : MonoBehaviour
{
    [SerializeField] private HitBoxCheck _playerHitBoxCheck;

    private bool _awake = false;

    void Start()
    {
        _playerHitBoxCheck.EnterCollider += Wake;
    }

    private void Update()
    {
        if (_awake)
        {
            //move towards the player
            //if we hit a wall, move up and down based on a sinwave of our current position
        }
    }


    private void Wake()
    {
        //start moving
        _awake = true;
    }
}
