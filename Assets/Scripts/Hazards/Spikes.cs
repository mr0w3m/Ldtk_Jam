using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour
{

    [SerializeField] private HitBoxCheck _hitboxcheck;

    private void Start()
    {
        _hitboxcheck.EnterCollider += HitPlayer;
    }

    private void HitPlayer()
    {
        //Actor.i.health.Hit(transform.position); eh i don't like it you can get stuck
        Actor.i.health.Death();
    }
}
