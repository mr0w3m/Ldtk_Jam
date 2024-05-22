using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A_AnimatorController : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    [SerializeField] private int _craftingInt;
    [SerializeField] private int _movingInt;
    [SerializeField] private int _jumpInt;
    [SerializeField] private int _idleInt;
    [SerializeField] private int _throwingInt;
    [SerializeField] private int _sleepInt;
    [SerializeField] private int _deadInt;

    void Update()
    {
        if (Actor.i.death.playerDead)
        {
            SetStateInt(_deadInt);
        }
        else if (Actor.i.sleeping)
        {
            SetStateInt(_sleepInt);
        }
        else if (Actor.i.crafting.crafting)
        {
            SetStateInt(_craftingInt);
        }
        else if (Actor.i.throwing.throwing)
        {
            SetStateInt(_throwingInt);
        }
        else if (Actor.i.jump.jumping)
        {
            SetStateInt(_jumpInt);
        }
        else if (Actor.i.movement.Rb2D.velocity.magnitude > 0.1f && Actor.i.collision.Grounded)
        {
            SetStateInt(_movingInt);
        }
        else if (Actor.i.movement.Rb2D.velocity.magnitude > 0.1f && !Actor.i.collision.Grounded)
        {
            SetStateInt(_jumpInt);
        }
        else
        {
            SetStateInt(_idleInt);
        }
    }

    private void SetStateInt(int value)
    {
        _animator.SetInteger("PlayerStateInt", value);
    }
}
