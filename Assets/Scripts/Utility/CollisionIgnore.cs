using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionIgnore : MonoBehaviour
{
    public Collider2D thisCollider;
    public List<Collider2D> otherColliders = new List<Collider2D>();

    private void Start()
    {
        foreach (Collider2D c in otherColliders)
        {
            Physics2D.IgnoreCollision(thisCollider, c, true);
        }
    }
}