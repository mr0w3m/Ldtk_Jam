using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Snake : MonoBehaviour
{
    [SerializeField] private Enemy_HP _hp;
    [SerializeField] private Animator _animator;
    [SerializeField] private HitBoxCheck _playerCheck;
    [SerializeField] private GameObject _deadFxObj;
    [SerializeField] private Vector2 _timeBetweenChangeDirectionRange;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private LayerMask _wallLayer;
    [SerializeField] private List<GameObject> _objectsToFlip;
    [SerializeField] private float _timeToAttack;
    [SerializeField] private Transform _centerT;
    [SerializeField] private GameObject _fireFx;
    [SerializeField] private AudioClip _moveClip;
    [SerializeField] private AudioClip _attackTriggerClip;
    [SerializeField] private AudioClip _onAttackClip;

    private Vector2 _targetPos;
    private Vector2 _forwardPosition;

    private float _directionChangeTimer;

    private float _attackTimer;
    [SerializeField] private float _attackCooldownTimeAfterAttack = 3;

    private float _aggroTimer;
    private float _aggroTime = 10;

    private float _fireDmgTimer;
    private float _fireDmgTime = 1f;

    private Direction _currentDirection;
    private bool _wallDetected = false;
    private bool _stopMovement = false;
    private bool _onFire = false;

    private Vector2 _wallCheckLeftPosition;
    private Vector2 _wallCheckRightPosition;

    [SerializeField] private float _wallCheckRadius;
    [SerializeField] private float _wallCheckDepth;
    [SerializeField] private float _wallCheckStartDistanceMultiplier;

    public GameObject _startForwardCastObj;
    public GameObject _endForwardCastObj;

    [SerializeField] private float _pitCheckRadius;
    [SerializeField] private float _pitCheckDepth;
    [SerializeField] private float _pitCheckStartDistanceMultiplier;

    public GameObject _startDownwardCastObj;
    public GameObject _endDownwardCastObj;

    private float _randomfloat;
    private bool _closeToPlayer;

    private void Start()
    {
        _hp.Died += Dead;
        _hp.HitEvent += Aggro;
        _playerCheck.EnterCollider += TriggerSnakeAttack;
        _wallCheckLeftPosition = (Vector2)_centerT.position + Vector2.left;
        _wallCheckRightPosition = (Vector2)_centerT.position + Vector2.right;

        _currentDirection = Direction.right;

        if (Random.value > 0.5f)
        {
            //keep it right
        }
        else
        {
            FlipDirection();
        }


        _randomfloat = Random.Range(0, 10000);
    }

    private void Update()
    {
        if (_attackTimer > 0)
        {
            _attackTimer -= Time.deltaTime;
            _stopMovement = true;
        }
        else
        {
            _stopMovement = false;
        }

        if (_aggroTimer > 0)
        {
            _aggroTimer -= Time.deltaTime;
        }

        if (_directionChangeTimer > 0 && !_stopMovement)
        {
            _directionChangeTimer -= Time.deltaTime;
        }
        else if (_directionChangeTimer < 0)
        {
            FlipDirection();
            _directionChangeTimer = Random.Range(_timeBetweenChangeDirectionRange.x, _timeBetweenChangeDirectionRange.y);
        }

        CheckForWall();
        CheckForPit();
        
        if (!_stopMovement)
        {

            MovementBehavior();
            if (_wallDetected)
            {
                FlipDirection();
            }
        }

        SetAnimation();

        if (_onFire)
        {
            if (_fireDmgTimer > 0)
            {
                _fireDmgTimer -= Time.deltaTime;
            }
            else
            {
                _hp.Hit(1, this.gameObject, true);
                _fireDmgTimer = _fireDmgTime;
            }
        }

        //if we're movign and are close to player
        if (_moveClip != null)
        {
            if (!_stopMovement && Vector2.Distance((Vector2)transform.position, (Vector2)Actor.i.playerCenterT.position) < 5)
            {
                if (!_closeToPlayer)
                {
                    _closeToPlayer = true;

                    AudioController.control.PlayLoopingAudio(_moveClip, _moveClip.length, false, "enemyMoveClip" + _randomfloat, 1);
                }
            }
            else if (_closeToPlayer)
            {
                _closeToPlayer = false;
                AudioController.control.StopLoopingAudio("enemyMoveClip" + _randomfloat);
            }
        }
        


        //debug obj positions to help debug casts
        //if (_currentDirection == Direction.right) {
        //wall check
        //    _startForwardCastObj.transform.position = _wallCheckRightPosition;
        //    _endForwardCastObj.transform.position = _wallCheckRightPosition + (Vector2.right * _wallCheckDepth); }
        //else {
        //    _startForwardCastObj.transform.position = _wallCheckLeftPosition;
        //    _endForwardCastObj.transform.position = _wallCheckLeftPosition + (Vector2.left * _wallCheckDepth); }

        //Vector2 _pitCheckForwardPosition = (_currentDirection == Direction.right ? (Vector2)_centerT.position + (Vector2.right * _pitCheckStartDistanceMultiplier) : (Vector2)_centerT.position + (Vector2.left * _pitCheckStartDistanceMultiplier));
        //pit check
        //_startDownwardCastObj.transform.position = _pitCheckForwardPosition;
        //_endDownwardCastObj.transform.position = _pitCheckForwardPosition + (Vector2.down * _pitCheckDepth);
    }

    private void Aggro(GameObject hitGo)
    {
        _aggroTimer = _aggroTime;

        if (hitGo.GetComponent<Fire>() != null)
        {
            CatchOnFire();
        }
    }

    private void CatchOnFire()
    {
        GameObject fireFXObj = Instantiate(_fireFx, _centerT.position, Quaternion.identity);
        fireFXObj.transform.SetParent(this.transform);
        _onFire = true;
    }


    private void SetAnimation()
    {
        _animator.SetBool("Moving", !_stopMovement);
    }

    private void TriggerSnakeAttack()
    {
        if (_attackTimer > 0)
        {
            return;
        }
        _animator.SetTrigger("Attack");
        _playerCheck.EnterCollider -= TriggerSnakeAttack;
        _attackTimer = _attackCooldownTimeAfterAttack + _timeToAttack;
        AudioController.control.PlayClip(_attackTriggerClip);
        StartCoroutine(AttackRoutine());
    }

    private IEnumerator AttackRoutine()
    {
        yield return new WaitForSeconds(_timeToAttack);
        if (_onAttackClip != null )
        {
            AudioController.control.PlayClip(_onAttackClip);
        }
        if (_playerCheck.colliding)
        {
            HitPlayer();
        }
        yield return new WaitForSeconds(_attackCooldownTimeAfterAttack);

        if (_playerCheck.colliding)
        {
            TriggerSnakeAttack();
        }
        else
        {
            _playerCheck.EnterCollider += TriggerSnakeAttack;
        }
    }

    //runs in Update
    private void MovementBehavior()
    {
        _forwardPosition = _currentDirection == Direction.right ? (Vector2)transform.position + Vector2.right : (Vector2)transform.position + Vector2.left;


        if (_aggroTimer > 0)
        {
            Vector2 playerDirectionV2 = Actor.i.playerCenterT.position - transform.position;
            _forwardPosition = playerDirectionV2.x > 0 ? (Vector2)transform.position + Vector2.right : (Vector2)transform.position + Vector2.left;
            Direction playerDirection = playerDirectionV2.x > 0 ? Direction.right : Direction.left;
            if (_currentDirection != playerDirection)
            {
                FlipDirection();
            }
        }

        _targetPos = Vector2.MoveTowards((Vector2)transform.position, _forwardPosition, _moveSpeed * Time.deltaTime);

        if (!_wallDetected)
        {
            transform.position = _targetPos;
        }
    }

    //runs in Update
    private void CheckForWall()
    {

        _wallCheckRightPosition = (Vector2)_centerT.position + (Vector2.right * _wallCheckStartDistanceMultiplier);
        _wallCheckLeftPosition = (Vector2)_centerT.position + (Vector2.left * _wallCheckStartDistanceMultiplier);
        RaycastHit2D hitInfoRight = Physics2D.CircleCast(_wallCheckRightPosition, _wallCheckRadius, Vector2.right, _wallCheckDepth, _wallLayer);
        RaycastHit2D hitInfoLeft = Physics2D.CircleCast(_wallCheckLeftPosition, _wallCheckRadius, Vector2.left, _wallCheckDepth, _wallLayer);

        if (hitInfoRight.collider != null && _currentDirection == Direction.right)
        {
            _wallDetected = true;
            _aggroTimer = 0;
        }
        else if (hitInfoLeft.collider != null && _currentDirection == Direction.left)
        {
            _wallDetected = true;
            _aggroTimer = 0;
        }
    }

    //runs in update
    public virtual void CheckForPit()
    {
        Vector2 _pitCheckForwardPosition = (_currentDirection == Direction.right ? (Vector2)_centerT.position + (Vector2.right * _pitCheckStartDistanceMultiplier) : (Vector2)_centerT.position + (Vector2.left * _pitCheckStartDistanceMultiplier));

        RaycastHit2D hitInfo = Physics2D.CircleCast(_pitCheckForwardPosition, _pitCheckRadius, Vector2.down, _pitCheckDepth, _wallLayer);
        if (hitInfo.collider == null)
        {
            //NoGround
            //change direction
            FlipDirection();
            _aggroTimer = 0;
        }
    }

    private void FlipDirection()
    {
        _currentDirection = _currentDirection == Direction.right ? Direction.left : Direction.right;
        _objectsToFlip.ForEach(o => o.transform.localScale = new Vector2(-o.transform.localScale.x, o.transform.localScale.y));
        _wallDetected = false;
    }

    private void HitPlayer()
    {
        Actor.i.health.Hit(transform.position);
    }

    private void Dead()
    {
        _hp.Died -= Dead;
        Instantiate(_deadFxObj, _centerT.position, Quaternion.identity);
        Destroy(this.gameObject);
    }

    private void OnDestroy()
    {
        AudioController.control.StopLoopingAudio("enemyMoveClip" + _randomfloat);
    }
}
