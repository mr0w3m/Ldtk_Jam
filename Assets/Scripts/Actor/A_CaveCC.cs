using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A_CaveCC : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rb2d;

    private bool _movementDisabled = false;


    public void MoveDirection(Vector2 vector)
    {
        if (_movementDisabled)
        {
            return;
        }

        _rb2d.AddForce(vector, ForceMode2D.Impulse);
    }
}
