using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Enemy_HP : MonoBehaviour
{
    [SerializeField] private float _invulTime = 0.1f;
    [SerializeField] private int _maxHP;
    [SerializeField] private SpriteRenderer _sprite;
    [SerializeField] private Material _litMaterial;
    [SerializeField] private Material _flashMaterial;

    [SerializeField] private AudioClip _deadClip;

    private float _invulTimer = 0;
    private int _hp = 1;
    private bool _dead = false;
    private bool _invulnerable = false;

    private float _flashTimer;
    private float _timeToFlash;
    private bool _flashBool = false;

    private Material _material;


    public event Action<GameObject> HitEvent;
    public event Action Died;

    private void OnHit(GameObject go)
    {
        if (HitEvent != null)
        {
            HitEvent.Invoke(go);
        }
    }

    private void OnDied()
    {
        if (Died != null)
        {
            Died.Invoke();
        }
    }

    private void Start()
    {
        _hp = _maxHP;

        _material = _sprite.material;
    }

    private void Update()
    {

        if (_invulTimer > 0)
        {
            _invulTimer -= Time.deltaTime;
            FlashSprite();
        }
        else if (_invulnerable)
        {
            _invulnerable = false;
            _material.SetFloat("_FlashAmount", 0);
            _sprite.material = _litMaterial;
            Debug.Log("FlashEnd");
        }
    }

    public void Hit(int amt, GameObject go, bool ignoreInvul = false)
    {
        if (_invulTimer > 0 && !ignoreInvul)
        {
            Debug.Log("Invul");
            return;
        }
        Debug.Log("HitEnemy");
        _hp -= amt;
        
        CheckDeath();
        TriggerInvulnerable();
        if (!_dead)
        {
            OnHit(go);
        }
    }

    private void TriggerInvulnerable()
    {
        _sprite.material = _flashMaterial;
        _material = _sprite.material;
        _invulTimer = _invulTime;
        _invulnerable = true;
        
    }

    private void CheckDeath()
    {
        if (_hp <= 0 && !_dead)
        {
            AudioController.control.PlayClip(_deadClip);
            OnDied();
        }
    }

    private void FlashSprite()
    {
        if (_flashTimer <= 0)
        {
            _flashTimer = Util.MapValue(_invulTimer, _invulTime, 0, _timeToFlash, 0.001f);
            _flashBool = !_flashBool;
            _material.SetFloat("_FlashAmount", _flashBool == true ? 0 : 1);
        }
        else
        {
            _flashTimer -= Time.deltaTime;
        }
    }
}
