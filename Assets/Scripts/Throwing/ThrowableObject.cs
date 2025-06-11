using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowableObject : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rb;
    public Rigidbody2D rb
    {
        get { return _rb; }
    }

    public float throwForce;
    public bool _rotateInDirection = true;

    public virtual void Throw(Vector3 throwDirection)
    {

    }
}
