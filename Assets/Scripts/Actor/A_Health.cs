using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class A_Health : MonoBehaviour
{
    [SerializeField] private A_HealthUI _healthUI;
    [SerializeField] private GameObject _invulSprite;
    [SerializeField] private float _invulTime = 1f;
    [SerializeField] private SpriteRenderer _playerSprite;
    [SerializeField] private Material _litMaterial;
    [SerializeField] private Material _flashMaterial;

    [SerializeField] private int _maxHP = 3;

    public int maxHP
    {
        get
        {
            return _maxHP;
        }
        set
        {
            _maxHP = value;
            _healthUI.ReInitialize();
        }
    }

    [SerializeField] private AudioClip _getHitClip;

    private bool _dataLoaded = false;
    private bool _invulnerable = false;
    private bool _flashBool = false;
    private float _flashTimer = 0;
    private float _timeToFlash = 0.1f;
    private float _invulTimer = 0;
    private int _hp = 1;

    private Material _material;

    public int hp
    {
        get { return _hp; }
    }

    public event Action HealthChanged;

    private void OnHealthChanged()
    {
        if (HealthChanged != null)
        {
            HealthChanged.Invoke();
        }
    }


    public event Action HealthLost;

    private void OnHealthLost()
    {
        if (HealthLost != null)
        {
            HealthLost.Invoke();
        }
    }

    public event Action DataLoaded;

    private void OnDataLoaded()
    {
        if (DataLoaded != null)
        {
            DataLoaded.Invoke();
        }
    }

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        PlayerSaveData tempSave = PlayerSaveManager.i.playerSaveData;
        _hp = tempSave.hp;
        _maxHP = tempSave.totalHp;
        _dataLoaded = true;
        OnDataLoaded();
    }

    private void Update()
    {
        if (!_dataLoaded)
        {
            return;
        }
        if (_invulTimer > 0)
        {
            _invulTimer -= Time.deltaTime;
            FlashSprite();
        }
        else if (_invulnerable)
        {
            _invulnerable = false;
            _material.SetFloat("_FlashAmount", 0);
            _playerSprite.material = _litMaterial;
            //_invulSprite.SetActive(false);
        }
    }

    public void Hit(Vector2 posOfDamage)
    {
        if (_invulTimer > 0)
        {
            return;
        }
        _hp -= 1;
        OnHealthChanged();
        CheckDeath();
        
        if (_hp > 0)
        {
            OnHealthLost();
            TriggerInvulnerable();
            AudioController.control.PlayClip(_getHitClip);
            A_CameraController.i.AddCameraShake(0.2f, 0.1f);
        }
    }

    public void Death()
    {
        _hp = 0;
        OnHealthChanged();
        CheckDeath();
    }

    public void AddHP(int amt)
    {
        _hp += amt;
        if (_hp > _maxHP)
        {
            _hp = _maxHP;
        }

        OnHealthChanged();
        TriggerInvulnerable();
    }

    private void TriggerInvulnerable()
    {
        _invulTimer = _invulTime;
        _playerSprite.material = _flashMaterial;
        _material = _playerSprite.material;
        _invulnerable = true;
    }

    private void CheckDeath()
    {
        if (_hp <= 0)
        {
            Actor.i.death.Dead();
        }
    }

    //called in update
    private void FlashSprite()
    {
        if (_flashTimer <= 0)
        {
            _flashTimer = Util.MapValue(_invulTimer, _invulTime, 0, _timeToFlash, 0.001f);
            _flashBool = !_flashBool;
            //_invulSprite.SetActive(_flashBool);
            _material.SetFloat("_FlashAmount", _flashBool == true ? 0 : 1);
        }
        else
        {
            _flashTimer -= Time.deltaTime;
        }
    }
}
