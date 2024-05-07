using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LDPlayer : MonoBehaviour
{
    [SerializeField] private A_Input _input;
    [SerializeField] private A_Movement _movement;
    [SerializeField] private A_Collision _collision;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _inAirMoveSpeed;

    [SerializeField] private float _maxMovementLength;


    private float _targetMoveSpeed;

    private void Start()
    {
        _input.LSLostInput += EndMove;
        _collision.LeftGround += InAirStart;
        _collision.ReturnedToGround += InAirEnd;

        _targetMoveSpeed = _moveSpeed;
    }

    private void Update()
    {
        Move(_input.LSX);
    }

    private void InAirStart()
    {
        _targetMoveSpeed = _inAirMoveSpeed;
    }
    private void InAirEnd()
    {
        _targetMoveSpeed = _moveSpeed;
    }

    public void Move(float f)
    {
        Vector2 direction = ((Vector2.one) * (f > 0 ? 1 : -1)) * _targetMoveSpeed * Time.deltaTime;
        direction = new Vector2(direction.x, 0);
        direction = f == 0 ? Vector2.zero : direction;

        //direction = direction.normalized;
        _movement.MoveDirection(direction);

        _movement.Rb2D.velocity = Vector2.ClampMagnitude(_movement.Rb2D.velocity, _maxMovementLength);
    }
    public void EndMove()
    {
        //Debug.Log("END MOVE");
        //_movement.MoveDirection(Vector2.zero);
        _targetMoveSpeed = _moveSpeed;
    }
}
