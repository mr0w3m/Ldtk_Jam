using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A_Movement : MonoBehaviour
{
    [SerializeField] private A_Collision _collision;
    [SerializeField] private A_Jump _jump;
    [SerializeField] private Rigidbody2D _rb2d;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _groundedDrag;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private AudioClip _landClip;
    [SerializeField] private float _clamberTime;

    private float _clamberTimer;

    private Direction _direction;

    private bool _movementDisabled = false;
    private bool _disableDrag = false;
    public bool disableDrag
    {
        set { _disableDrag = value; }
    }

    private bool _checkPause = false;
    private Vector2 _cachedVelocity;

    public enum MoveState
    {
        idle,
        move
    }

    [SerializeField] private MoveState _moveState;
    public MoveState CurrentMoveState
    {
        get { return _moveState; }
    }

    public Direction Direction
    {
        get { return _direction; }
    }

    public bool PauseMovement
    {
        get { return _movementDisabled; }
        set 
        { 
            _movementDisabled = value;
            _rb2d.velocity = (value == true && _collision.Grounded) ? Vector2.zero : _rb2d.velocity;
        }
    }
    private bool _clambering;
    public bool clambering
    {
        get { return _clambering; }
    }

    public Rigidbody2D Rb2D
    {
        get { return _rb2d; }
    }

    public event Action FlipDirection;

    private void OnFlipDirection()
    {
        if (FlipDirection != null)
        {
            FlipDirection.Invoke();
        }
    }

    private void Start()
    {
        _collision.ReturnedToGround += CancelVelocity;
        Actor.i.input.ADown += CheckForPlatformBelow;
    }

    private void CancelVelocity()
    {
        _rb2d.velocity = Vector2.zero;
        AudioController.control.PlayClip(_landClip, UnityEngine.Random.Range(0.5f, 1), 0.1666f);
    }

    private void CheckForPlatformBelow()
    {
        if (_clambering)
        {
            return;
        }
        //if a is pressed while holding down, and there's a platform below, turn off the platformbelow\
        if (Actor.i.input.LSY < -0.9f)
        {
            RaycastHit2D hitInfo = Physics2D.Linecast(transform.position, (Vector2)transform.position + Vector2.down, _groundLayer);
            if (hitInfo.collider != null)
            {
                StepThroughPlatform stepThrough = hitInfo.collider.GetComponent<StepThroughPlatform>();
                if (stepThrough != null)
                {
                    stepThrough.StepThrough();
                }
            }
        }
    }

    private void Update()
    {
        if (Actor.i.paused)
        {
            if (!_checkPause)
            {
                _checkPause = true;
                _cachedVelocity = _rb2d.velocity;
                
                CancelVelocity();
                _rb2d.isKinematic = true;
            }
        }
        else
        {
            if (_checkPause)
            {
                _checkPause = false;
                _rb2d.isKinematic = false;
                _rb2d.velocity = _cachedVelocity;
            }
        }
        if (!Actor.i.paused || !(Actor.i.input.MouseMode && Actor.i.throwing.throwing))
        {
            SetMoveState(Actor.i.input.LSX);
        }
        else
        {
            Debug.Log("ReturningSetMoveState");
        }

        if (_clamberTimer > 0)
        {
            _clamberTimer -= Time.deltaTime;
        }
        else
        {
            if (_clambering)
            {
                _clambering = false;
                //finish clamber
                _movementDisabled = false;
            }
        }
    }


    private void FixedUpdate()
    {
        if (_movementDisabled)
        {
            return;
        }

        MoveLeftRight(Actor.i.input.LSX);

        if (_collision.Grounded && Mathf.Abs(Actor.i.input.LSX) <= 0.1f && !_jump.jumping && !_disableDrag)
        {
            _rb2d.velocity *= _groundedDrag;
        }

        if (!_collision.Grounded && !_clambering)
        {
            CheckForClamber();
        }
    }

    private void Clamber(Vector2 position)
    {
        //Debug.Log("StartClamber");
        //if we detect a stepthroughplatform,
        _clambering = true;
        _clamberTimer = _clamberTime;
        SetPosition(position);
        _movementDisabled = true;
    }

    private void CheckForClamber()
    {
        RaycastHit2D hitInfo = Physics2D.Linecast(transform.position, (Vector2)transform.position + Vector2.up, _groundLayer);
        if (hitInfo.collider != null)
        {
            StepThroughPlatform stepThrough = hitInfo.collider.GetComponent<StepThroughPlatform>();
            if (stepThrough != null)
            {
                Clamber(hitInfo.point + (Vector2.up * stepThrough.clamberOffset));
            }
        }
    }

    private void MoveLeftRight(float input)
    {
        if (_movementDisabled || Actor.i.paused || (Actor.i.input.MouseMode && Actor.i.throwing.throwing) || Actor.i.death.playerDead)
        {
            //Debug.Log("ReturningMoveLeftRight");
            return;
        }
        if (Mathf.Abs(Actor.i.input.LSX) > 0.1f)
        {
            Vector2 direction = new Vector2(Actor.i.input.LSX, _rb2d.velocity.y);
            _rb2d.velocity = new Vector2(direction.x * _moveSpeed, _rb2d.velocity.y);
        }
    }

    public void MoveUp(Vector2 vector)
    {
        if (_movementDisabled)
        {
            return;
        }
        _rb2d.velocity = new Vector2(_rb2d.velocity.x, vector.y);
    }

    //public void MoveInDirection(Vector2 direction)
    //{
    //    Debug.Log("MovingPlayer");
    //    _rb2d.velocity = new Vector2(_rb2d.velocity.x + direction.x, _rb2d.velocity.y + direction.y);
    //}


    public void SetPosition(Vector2 pos)
    {
        //Debug.Log("SettingPositionTo: " + pos.ToString());
        _rb2d.isKinematic = true;
        _movementDisabled = true;
        _rb2d.velocity = Vector2.zero;

        _rb2d.position = pos;

        _rb2d.isKinematic = false;
        _movementDisabled = false;
    }

    public void FaceDirection(float f)
    {
        SetFacingDirection(f);
    }

    private void SetMoveState(float f)
    {
        float value = Mathf.Abs(f);

        if (value > 0)
        {
            _moveState = MoveState.move;
            SetFacingDirection(f);
        }
        else
        {
            _moveState = MoveState.idle;
        }
    }

    private void SetFacingDirection(float f)
    {
        Direction lastDirection = _direction;

        _direction = f > 0 ? Direction.right : Direction.left;
        if (_direction != lastDirection)
        {
            OnFlipDirection();
        }
    }
}