using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A_Jump : MonoBehaviour
{
    [SerializeField] private A_Input _input;
    [SerializeField] private A_Movement _movement;
    [SerializeField] private A_Collision _collision;

    [SerializeField] private Vector2 _jumpVector;
    [SerializeField] private float _continuousJumpForce;
    [SerializeField] private float _timeToFullJump;

    [SerializeField] private AudioClip _jumpClip;


    private float timer;
    private float jumpForce;

    private bool _jumping = false;
    public bool jumping
    {
        get { return _jumping; }
    }

    private bool _coyoteTime = false;
    private bool _coyoteTimeActivatedThisJump = false;

    private float _coyoteTimer;
    private float _coyoteTimerTime = 0.1f;

    private void Start()
    {
        _input.ADown += BeginJump;
        _input.AUp += EndJump;
    }

    private void FixedUpdate()
    {

        if (_jumping)
        {
            if (timer > 0)
            {
                timer -= Time.deltaTime;
                //jumpForce = (_continuousJumpForce * Util.MapValue(timer, _timeToFullJump, 0, 0, 1));
                _movement.MoveUp(_jumpVector * jumpForce);
            }
            else
            {
                //end jump
                EndJump();
            }
        }
        else
        {
            timer = _timeToFullJump;
            jumpForce = _continuousJumpForce;
        }

        if (!_collision.Grounded)
        {
            if (!_coyoteTimeActivatedThisJump)
            {
                _coyoteTimeActivatedThisJump = true;
                _coyoteTimer = _coyoteTimerTime;
                _coyoteTime = true;
            }
        }
        else
        {
            _coyoteTimeActivatedThisJump = false;
            _coyoteTime = false;
            _coyoteTimer = 0;
        }

        if (_coyoteTimer > 0)
        {
            _coyoteTimer -= Time.deltaTime;
        }
        else
        {
            if (_coyoteTime)
            {
                _coyoteTime = false;
            }
        }
    }

    private void BeginJump()
    {
        if (Actor.i.crafting.crafting || Actor.i.paused || Actor.i.input.LSY < -0.9f || Actor.i.death.playerDead)
        {
            return;
        }

        if (_collision.Grounded || _coyoteTime)
        {
            _jumping = true;
            _coyoteTime = false;
            AudioController.control.PlayClip(_jumpClip, Random.Range(0.85f, 2), 0.5f);
        }
    }

    private void EndJump()
    {
        _jumping = false;
    }
}