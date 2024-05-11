using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Enemy_HP : MonoBehaviour
{
    [SerializeField] private float _invulTime = 0.1f;
    [SerializeField] private int _maxHP;


    private float _invulTimer = 0;
    private int _hp = 1;
    private bool _dead = false;

    public event Action Died;

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
    }

    private void Update()
    {
        if (_invulTimer > 0)
        {
            _invulTimer -= Time.deltaTime;
        }
    }

    public void Hit()
    {
        Debug.Log("HitEnemy");
        if (_invulTimer > 0)
        {
            return;
        }
        _hp -= 1;
        OnDied();
        CheckDeath();
        TriggerInvulnerable();
    }

    private void TriggerInvulnerable()
    {
        _invulTimer = _invulTime;
    }

    private void CheckDeath()
    {
        if (_hp <= 0 && !_dead)
        {
            OnDied();
        }
    }
}
