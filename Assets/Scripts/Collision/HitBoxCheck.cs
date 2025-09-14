using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBoxCheck : MonoBehaviour
{
    public bool debug;
    [SerializeField] private BoxCollider2D _bColl;
    [SerializeField] private float _boxColliderRotation = 0;
    [SerializeField] private CircleCollider2D _cColl;

    public bool colliding;

    public LayerMask _targetLayer;

    public bool disabled = false;


    //This should change to a HitInfo Class that we can pass through when collision happens.
    public event Action EnterCollider;
    private void OnCollided()
    {
        if (EnterCollider != null)
        {
            EnterCollider.Invoke();
        }
        if (debug)
        {
            Debug.Log("OnCollided");
        }
    }

    public event Action<CollisionInfo> EnterCollider_Info;
    private void OnEnterCollided(CollisionInfo info)
    {
        if (EnterCollider_Info != null)
        {
            EnterCollider_Info.Invoke(info);
        }
        if (debug)
        {
            Debug.Log("OnEnterCollided");
        }
    }

    public event Action LeftCollider;
    private void OnLeftCollider()
    {
        if (LeftCollider != null)
        {
            LeftCollider.Invoke();
        }
        if (debug)
        {
            Debug.Log("OnLeftCollider");
        }
    }

    public event Action<CollisionInfo> InCollider;
    private void OnInCollider(CollisionInfo info)
    {
        if (InCollider != null)
        {
            InCollider.Invoke(info);
        }
        if (debug)
        {
            Debug.Log("OnInCollider");
        }
    }

    private void FixedUpdate()
    {
        if (disabled)
        {
            return;
        }
        CheckRoutine();
    }

    private void CheckRoutine()
    {
        Collider2D hitObj;
        if (_bColl != null)
        {
            hitObj = Physics2D.OverlapBox(((Vector2)_bColl.transform.position + _bColl.offset), _bColl.size, _boxColliderRotation, _targetLayer);
        }
        else
        {
            hitObj = Physics2D.OverlapCircle(((Vector2)_cColl.transform.position + _cColl.offset), _cColl.radius, _targetLayer);
        }

        if (hitObj != null)
        {
            Vector2 currentPos = _bColl != null ? ((Vector2)_bColl.transform.position + _bColl.offset) : ((Vector2)_cColl.transform.position + _cColl.offset);
            CollisionInfo info = new CollisionInfo(hitObj, currentPos);

            if (!colliding)
            {
                colliding = true;
                OnEnterCollided(info);
                OnCollided();
            }

            OnInCollider(info);
        }
        else
        {
            if (colliding)
            {
                OnLeftCollider();
                colliding = false;
            }
        }

        if (debug)
        {
            Debug.Log("Checking on : " + this.gameObject.name);
        }

    }
}