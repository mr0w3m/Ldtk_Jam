using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionInfo
{
    public Collider2D col;
    public Vector2 pos;

    public CollisionInfo(Collider2D collider)
    {
        this.col = collider;
        this.pos = Vector2.zero;
    }

    public CollisionInfo(Collider2D collider, Vector2 pos)
    {
        this.col = collider;
        this.pos = pos;
    }
}
