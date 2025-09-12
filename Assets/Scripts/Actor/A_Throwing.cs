using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using static UnityEngine.GraphicsBuffer;

public class A_Throwing : MonoBehaviour
{
    //throwing
    //set the player sprite to throw
    //enable throw helper ui
    //directionally show 
    [SerializeField] private A_Inventory _inventory;
    [SerializeField] private A_Input _input;
    [SerializeField] private A_Movement _movement;
    [SerializeField] private GenericItemDataList _itemDatabase;
    [SerializeField] private A_Crafting _crafting;

    [SerializeField] private SpriteRenderer _playerSpriteRenderer;//lazy,removelater when animating
    [SerializeField] private Sprite _throwingSprite, _idleSprite;

    [SerializeField] private Transform _throwingIndicator;
    [SerializeField] private Transform _targetSpawnThrownLocation;
    [SerializeField] private AudioClip _throwClip;


    [SerializeField] private LayerMask _groundLayer;

    [SerializeField] private float _postThrowTime = 1;

    public int angleOffset = 275;

    private bool _throwing = false;

    public bool throwing
    {
        get { return _throwing; }
    }

    private bool _postThrow = false;

    private Vector3 _throwDirection;

    private float _postThrowTimer = 0;

    private void Start()
    {
        _input.BDown += StartThrowing;
        _input.BUp += EndThrowing;
    }

    private void Update()
    {
        if (_throwing)
        {
            if (_input.MouseMode)
            {
                _throwDirection = new Vector3(Util.MapValue(Input.mousePosition.x, 0, Screen.width, -1, 1), Util.MapValue(Input.mousePosition.y, 0, Screen.height, -1, 1));
            }
            else
            {
                if (Mathf.Abs(_input.LSX) > 0.01f || Mathf.Abs(_input.LSY) > 0.01f)
                {
                    //there is input on the left stick so take it
                    _throwDirection = new Vector3(_input.LSX, _input.LSY, 0);
                }
                else
                {
                    //No input on the left stick, let's set a constant based on the direction they are looking
                    _throwDirection = new Vector3((_movement.Direction == Direction.left ? -1:1), 0.5f, 0);
                }
            }


            _throwDirection = _throwDirection.normalized;
            float angleDir = Mathf.Atan2(_throwDirection.y, _throwDirection.x) * Mathf.Rad2Deg;
            angleDir += angleOffset;

            _throwingIndicator.eulerAngles = new Vector3(_throwingIndicator.eulerAngles.x, _throwingIndicator.eulerAngles.y, angleDir);
        }

        if(_postThrowTimer > 0)
        {
            if (_postThrow)
            {
                _postThrowTimer -= Time.deltaTime;
            }
            else
            {
                _postThrow = true;
                _movement.PauseMovement = true;
            }
        }
        else
        {
            if (_postThrow)
            {
                _postThrow = false;
                _movement.PauseMovement = false;
            }
        }
    }

    public void DropItem(string id)
    {
        //throw item with 0 force
        Debug.Log("Dropping: " + id);
        ThrowableObject tObj = Instantiate(_itemDatabase.ReturnItemData(id)._throwablePrefab, _targetSpawnThrownLocation.position, Quaternion.identity);
        
        tObj.rb.AddForce(Random.insideUnitCircle * tObj.throwForce * 0.3f, ForceMode2D.Impulse);
        tObj.Throw(_throwDirection);

        AudioController.control.PlayClip(_throwClip);

        _inventory.RemoveItemSelected();
    }

    private void StartThrowing()
    {
        if (_crafting.crafting || Actor.i.paused || Actor.i.death.playerDead)
        {
            return;
        }
        if (!_inventory.SelectedItemNotNull())
        {
            return;
        }

        _throwingIndicator.gameObject.SetActive(true);
        _throwing = true;
        _movement.PauseMovement = true;
        _playerSpriteRenderer.sprite = _throwingSprite;
    }

    private void EndThrowing()
    {
        RaycastHit2D hitInfo = Physics2D.Linecast(_throwingIndicator.position, ((Vector2)_targetSpawnThrownLocation.position), _groundLayer);
        Debug.DrawLine(_throwingIndicator.position, _targetSpawnThrownLocation.position, Color.green, 10f);
        if (hitInfo.collider != null)
        {
            _throwingIndicator.gameObject.SetActive(false);
            _throwing = false;
            _movement.PauseMovement = false;
            _playerSpriteRenderer.sprite = _idleSprite;
            return;
        }

        if (!_throwing)
        {
            _throwingIndicator.gameObject.SetActive(false);
            _throwing = false;
            _movement.PauseMovement = false;
            _playerSpriteRenderer.sprite = _idleSprite;
            return;
        }

        //check current selected item
        string id = _inventory.ReturnSelectedItem();

        ThrowableObject tObj = Instantiate(_itemDatabase.ReturnItemData(id)._throwablePrefab, _targetSpawnThrownLocation.position, Quaternion.identity);
        if (tObj._rotateInDirection)
        {
            _throwDirection = _throwDirection.normalized;
            float angleDir = Mathf.Atan2(_throwDirection.y, _throwDirection.x) * Mathf.Rad2Deg;
            //angleDir += angleOffset;

            tObj.transform.eulerAngles = new Vector3(_throwingIndicator.eulerAngles.x, _throwingIndicator.eulerAngles.y, angleDir);
        }
        

        tObj.rb.AddForce(_throwDirection * tObj.throwForce, ForceMode2D.Impulse);
        tObj.Throw(_throwDirection);

        AudioController.control.PlayClip(_throwClip);

        _inventory.RemoveItemSelected();
        _throwingIndicator.gameObject.SetActive(false);
        _throwing = false;
        _movement.PauseMovement = false;
        _playerSpriteRenderer.sprite = _idleSprite;

        _postThrowTimer = _postThrowTime;
    }
}
