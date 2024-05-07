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
    [SerializeField] private AnimationCurve _jumpCurve;

    private IEnumerator _jumpRoutine;

    private float timer;
    private float jumpForce;

    private bool _jumping = false;

    private void Start()
    {
        _input.ADown += BeginJump;
        _input.AUp += EndJump;
    }

    private void Update()
    {

        if (_jumping)
        {
            if (timer > 0)
            {
                timer -= Time.deltaTime;
                jumpForce = (_continuousJumpForce * _jumpCurve.Evaluate(Util.MapValue(timer, _timeToFullJump, 0, 0, 1))) * Time.deltaTime;
                _movement.MoveDirection(_jumpVector * jumpForce);
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
    }

    private void BeginJump()
    {
        Debug.Log("Jump!");
        if (_collision.Grounded)
        {
            _jumping = true;
            //_jumpRoutine = JumpRoutine();
            //StartCoroutine(_jumpRoutine);
        }
    }

    private void BeginJumpIgnoreGround()
    {
        _jumpRoutine = JumpRoutine();
        StartCoroutine(_jumpRoutine);
    }

    private void EndJump()
    {
        _jumping = false;
        if (_jumpRoutine != null)
        {
            
            //StopCoroutine(_jumpRoutine);
            //_jumpRoutine = null;
        }
    }

    private IEnumerator JumpRoutine()
    {
        float timer = _timeToFullJump;
        float jumpForce = _continuousJumpForce;

        while (timer > 0)
        {
            timer -= Time.deltaTime;
            jumpForce = (_continuousJumpForce * _jumpCurve.Evaluate(Util.MapValue(timer, _timeToFullJump, 0, 0, 1))) * Time.deltaTime;
            _movement.MoveDirection(_jumpVector * jumpForce);

            yield return 0f;
        }
    }
}