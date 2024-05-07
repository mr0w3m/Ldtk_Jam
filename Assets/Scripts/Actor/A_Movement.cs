using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A_Movement : MonoBehaviour
{
    //[SerializeField] private A_AnimatorController _anim;
    //[SerializeField] private A_AnimatorController _weaponAnim;
    [SerializeField] private Rigidbody2D _rb2d;
    [SerializeField] private float _turnAroundTime;

    private bool _pauseMovement;
    private Direction _direction;
    private IEnumerator _moveToPosRoutine;

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
        get { return _pauseMovement; }
        set { _pauseMovement = value; }
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

    public void MoveToPos(Vector2 pos, float time, AnimationCurve curve, Action callback = null)
    {
        if (_moveToPosRoutine != null)
        {
            StopCoroutine(_moveToPosRoutine);
            _moveToPosRoutine = null;
        }
        _moveToPosRoutine = MoveToPosRoutine(pos, time, curve, callback);
        StartCoroutine(_moveToPosRoutine);
    }

    private IEnumerator MoveToPosRoutine(Vector2 pos, float time, AnimationCurve curve, Action callback)
    {
        float timer = time;
        Vector2 prevPos = _rb2d.transform.position;

        while (timer > 0)
        {
            timer -= Time.fixedDeltaTime;
            _rb2d.transform.position = Vector2.Lerp(prevPos, pos, curve.Evaluate(Util.MapValue(timer, time, 0, 0, 1)));

            yield return 0f;
        }

        if (callback != null)
        {
            callback.Invoke();
        }
    }

    public void Throw(Vector2 direction)
    {
        // if (_pauseMovement)
        // {
        //     return;
        // }
        _rb2d.AddForce(direction, ForceMode2D.Impulse);
    }

    public void MoveDirection(Vector2 vector)
    {
        if (_movementDisabled)
        {
            return;
        }
        SetMoveState(vector.x);
        if (_pauseMovement)
        {
            return;
        }

        _rb2d.AddForce(vector, ForceMode2D.Impulse);
        
        SetAnim();
    }

    public void SetPosition(Vector2 pos)
    {
        _rb2d.isKinematic = true;
        _pauseMovement = true;

        _rb2d.position = pos;

        _rb2d.isKinematic = false;
        _pauseMovement =false;
    }

    public void FaceDirection(float f)
    {
        SetFacingDirection(f);
    }

    private void SetMoveState(float f)
    {
        if (_movementDisabled)
        {
            return;
        }
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
            TurnAround();
        }
    }

    private void SetAnim()
    {
        //_anim.SetBool("Moving", _moveState == MoveState.move);
        //if (_weaponAnim != null)
        //{
        //    _weaponAnim.SetBool("Moving", _moveState == MoveState.move);
        //}
    }

    private void TurnAround()
    {
        if (_pauseMovement)
        {
            return;
        }
        if (_turnAroundTime <= 0)
        {
            return;
        }
        //_anim.SetTrigger("TurnAround");
        _pauseMovement = true;
        StartCoroutine(Util.WaitAndCallRoutine(_turnAroundTime, () => _pauseMovement = false));
    }
}