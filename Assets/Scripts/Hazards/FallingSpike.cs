using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingSpike : MonoBehaviour
{
    [SerializeField] private Enemy_HP _hp;
    [SerializeField] private GameObject _breakFx;
    [SerializeField] private Rigidbody2D _rb2d;
    [SerializeField] private HitBoxCheck _fallHitBoxCheck;
    [SerializeField] private BoxCollider2D _spikeCollider;
    [SerializeField] private GameObject _checkforfallObj;
    [SerializeField] private LayerMask _spikeHitLayer;
    [SerializeField] private float _destroyTime;
    [SerializeField] private AudioClip _fallClip;
    [SerializeField] private AudioClip _hitGroundClip;

    [SerializeField] private bool _debug;

    private bool _falling = false;
    private bool _collided = false;
    private bool _destroy = false;

    private float _destroyTimer;


    private void Start()
    {
        _destroyTimer = _destroyTime;
        _fallHitBoxCheck.EnterCollider += Drop;
        _hp.Died += Drop;
    }

    private void Update()
    {
        if (_destroy)
        {
            if (_destroyTimer > 0)
            {
                _destroyTimer -= Time.deltaTime;
            }
            else
            {
                DestroyMe();
            }
        }

        if (_falling && !_collided)
        {
            Vector2 boxCenter = (Vector2)transform.position + _spikeCollider.offset;

            if (_debug)
            {
                Vector2 topLeft = new Vector2(boxCenter.x + (-_spikeCollider.size.x / 2), boxCenter.y + (_spikeCollider.size.y / 2));
                Vector2 topRight = new Vector2(boxCenter.x + (_spikeCollider.size.x / 2), boxCenter.y + (_spikeCollider.size.y / 2));
                Vector2 bottomLeft = new Vector2(boxCenter.x + (-_spikeCollider.size.x / 2), boxCenter.y + (-_spikeCollider.size.y / 2));
                Vector2 bottomRight = new Vector2(boxCenter.x + (_spikeCollider.size.x / 2), boxCenter.y + (-_spikeCollider.size.y / 2));


                Debug.DrawLine(topLeft, topRight, Color.red);
                Debug.DrawLine(topRight, bottomRight, Color.red);
                Debug.DrawLine(bottomRight, bottomLeft, Color.red);
                Debug.DrawLine(bottomLeft, topLeft, Color.red);
            }


            Collider2D[] hitColliders = Physics2D.OverlapBoxAll(boxCenter, _spikeCollider.size, 0, _spikeHitLayer);


            foreach (Collider2D hitColl in hitColliders)
            {
                if (hitColl != null)
                {
                    Actor a = hitColl.GetComponent<Actor>();
                    if (a != null)
                    {
                        HitPlayer();
                    }
                    else
                    {
                        a = hitColl.GetComponentInParent<Actor>();
                        if (a != null)
                        {
                            HitPlayer();
                        }
                        else
                        {
                            HitGround();
                        }
                    }
                }
            }
        }
    }


    private void Drop()
    {
        Instantiate(_breakFx, transform.position, Quaternion.identity);
        _fallHitBoxCheck.EnterCollider -= Drop;
        _hp.Died -= Drop;
        _checkforfallObj.SetActive(false);
        
        _falling = true;
        _destroy = true;

        _rb2d.isKinematic = false;

        AudioController.control.PlayClip(_fallClip);
    }

    private void HitGround()
    {
        _rb2d.velocity = Vector3.zero;
        _collided = true;

        AudioController.control.PlayClip(_hitGroundClip);
        //DestroyMe();
    }

    private void HitPlayer()
    {
        if (_collided)
        {
            return;
        }

        _collided = true;
        Debug.Log("HitPlayer");
        Actor.i.health.Hit(transform.position);
        //DestroyMe();
    }

    private void DestroyMe()
    {
        AudioController.control.PlayClip(_hitGroundClip);
        Instantiate(_breakFx, transform.position, Quaternion.identity);

        Destroy(this.gameObject);
    }
}
