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
        Actor.i.death.Dead();
    }
}
