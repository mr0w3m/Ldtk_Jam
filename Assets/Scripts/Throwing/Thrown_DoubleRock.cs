using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thrown_DoubleRock : ThrowableObject
{
    [SerializeField] private ThrowableObject _throwRockPrefab;
    [SerializeField] private float _angleOffset = 5;

    public override void Throw(Vector3 throwDirection)
    {
        base.Throw(throwDirection);

        Debug.Log("(initial)ThrowDirection: " + throwDirection);

        ThrowableObject tObj = Instantiate(_throwRockPrefab, transform.position, Quaternion.identity);
        
        Vector3 newThrowDirection = new Vector3(throwDirection.x, throwDirection.y + 1, 0);
        newThrowDirection = newThrowDirection.normalized;
        Debug.Log("(new)ThrowDirection: " + newThrowDirection);

        tObj.rb.AddForce(newThrowDirection * tObj.throwForce, ForceMode2D.Impulse);
    }
}
