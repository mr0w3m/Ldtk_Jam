using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A_Movement : MonoBehaviour
{
    //[SerializeField] private A_AnimatorController _anim;
    //[SerializeField] private A_AnimatorController _weaponAnim;
    [SerializeField] private A_Collision _collision;
    [SerializeField] private A_Jump _jump;
    [SerializeField] private Rigidbody2D _rb2d;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _groundedDrag;

    private Direction _direction;

    private bool _movementDisabled = false;

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
    }

    private void CancelVelocity()
    {
        _rb2d.velocity = Vector2.zero;
    }

    private void Update()
    {
        SetMoveState(Actor.i.input.LSX);
    }


    private void FixedUpdate()
    {
        MoveLeftRight(Actor.i.input.LSX);

        if (_collision.Grounded && Mathf.Abs(Actor.i.input.LSX) <= 0.1f && !_jump.jumping)
        {
            _rb2d.velocity *= _groundedDrag;
        }
    }

    private void MoveLeftRight(float input)
    {
        if (_movementDisabled)
        {
            return;
        }
        if (Mathf.Abs(Actor.i.input.LSX) > 0.1f)
        {
            Vector2 direction = new Vector2(Actor.i.input.LSX, 0);
            _rb2d.velocity = new Vector2(direction.x * _moveSpeed, _rb2d.velocity.y);
        }
    }

    public void MoveDirection(Vector2 vector)
    {
        if (_movementDisabled)
        {
            return;
        }
        _rb2d.velocity = new Vector2(_rb2d.velocity.x, vector.y);
    }

    public void SetPosition(Vector2 pos)
    {
        _rb2d.isKinematic = true;
        _movementDisabled = true;

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