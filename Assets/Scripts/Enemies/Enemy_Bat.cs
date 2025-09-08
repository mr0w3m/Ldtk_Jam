using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy_Bat : MonoBehaviour
{
    [SerializeField] private Enemy_HP _hp;
    [SerializeField] private HitBoxCheck _awakenBatHitBoxCheck;

    [SerializeField] private float _moveSpeed;

    [SerializeField] private GameObject _deadFxObj;
    [SerializeField] private GameObject _sleepParticlesObj;
    [SerializeField] private BoxCollider2D _hitPlayerColl;
    [SerializeField] private Animator _animator;
    [SerializeField] private LayerMask _playerLayer;
    [SerializeField] private LayerMask _wallLayer;

    [SerializeField] private AudioClip _announceClip;
    [SerializeField] private AudioClip _wingFlap;


    [SerializeField] private GameObject _debugTargetObject;
    [SerializeField] private GameObject _debugForwardObject;

    [SerializeField] private GameObject _deathFood;

    private bool _awake = false;
    private bool _wallDetected = false;
    private Vector2 _targetPos;

    private float _wiggleWallBatOffset = 3;
    private float _wiggleSpeed = 1;
    private float _timeOffsetForSin = 0;

    private float _hitPlayerTimer;
    private float _timeBetweenHitPlayer = 2;

    private float _randomfloat;
    private bool _closeToPlayer;

    void Start()
    {
        _awakenBatHitBoxCheck.EnterCollider += Wake;
        _hp.Died += Dead;

        _randomfloat = Random.Range(0, 10000);
    }

    private void Update()
    {
        if (_awake)
        {
            if (_hitPlayerTimer > 0)
            {
                _hitPlayerTimer -= Time.deltaTime;
            }

            if (Vector2.Distance((Vector2)transform.position, (Vector2)Actor.i.playerCenterT.position) < 10)
            {
                if (!_closeToPlayer)
                {
                    _closeToPlayer = true;

                    AudioController.control.PlayLoopingAudio(_wingFlap, _wingFlap.length, false, "wingFlap" + _randomfloat, 1);
                }
            }
            else if (_closeToPlayer)
            {
                _closeToPlayer = false;
                AudioController.control.StopLoopingAudio("wingFlap" + _randomfloat);
            }

            //move towards the player
            //if we hit a wall, move up and down based on a sinwave of our current position
            _targetPos = Vector2.MoveTowards(transform.position, Actor.i.playerCenterT.position, _moveSpeed * Time.deltaTime);


            //_debugTargetObject.transform.position = Vector2.MoveTowards(_targetPos, Actor.i.playerCenterT.position, _moveSpeed * Time.deltaTime);

            //what direction is the player in?
            Vector2 playerDirection = Actor.i.playerCenterT.position - transform.position;
            if (Mathf.Abs(playerDirection.x) > Mathf.Abs(playerDirection.y))
            {
                Vector2 forwardPosition;
                forwardPosition = playerDirection.x > 0 ? (Vector2)transform.position + Vector2.right : (Vector2)transform.position + Vector2.left;
                //_debugForwardObject.transform.position = forwardPosition;

                RaycastHit2D hitInfo = Physics2D.Linecast(transform.position, forwardPosition, _wallLayer);
                _wallDetected = hitInfo.collider != null ? true : false;


                if (_wallDetected)
                {
                    _timeOffsetForSin += (_wiggleSpeed * Time.deltaTime);
                    //if we detect a wall in front, move up and down via a sin wave to get 'unstuck', and multiply the height by how high or low the bat should try to go
                    Vector2 newTargetPosition = new Vector2(forwardPosition.x, forwardPosition.y + (_wiggleWallBatOffset * Mathf.Sin(_timeOffsetForSin)));
                    _targetPos = Vector2.MoveTowards(transform.position, newTargetPosition, _moveSpeed * Time.deltaTime);
                    //_targetPos = new Vector2(forwardPosition.x, forwardPosition.y + (_wiggleWallBatOffset * Mathf.Sin(_timeOffsetForSin)));
                }
            }

            transform.position = _targetPos;


            Vector2 boxCenter = (Vector2)transform.position + _hitPlayerColl.offset;

            Collider2D[] hitColliders = Physics2D.OverlapBoxAll(boxCenter, _hitPlayerColl.size, 0, _playerLayer);

            foreach (Collider2D hitColl in hitColliders)
            {
                if (hitColl != null)
                {
                    Actor a = hitColl.GetComponent<Actor>();
                    if (a != null && _hitPlayerTimer <= 0)
                    {
                        HitPlayer();
                    }
                }
            }
        }
    }

    private void HitPlayer()
    {
        Actor.i.health.Hit(transform.position);
        _hitPlayerTimer = _timeBetweenHitPlayer;
    }

    private void Wake()
    {
        //start moving
        _awakenBatHitBoxCheck.gameObject.SetActive(false);
        _sleepParticlesObj.SetActive(false);
        _awake = true;
        _animator.SetBool("Fly", true);
        AudioController.control.PlayClip(_announceClip);
    }

    private void Dead()
    {
        _hp.Died -= Dead;
        Instantiate(_deadFxObj, transform.position,Quaternion.identity);
        AudioController.control.StopLoopingAudio("wingFlap" + _randomfloat);

        Instantiate(_deathFood, transform).transform.SetParent(null);


        Destroy(this.gameObject);
    }

    private void OnDestroy()
    {
        AudioController.control.StopLoopingAudio("wingFlap" + _randomfloat);
    }
}
