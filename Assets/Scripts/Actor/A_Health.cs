using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class A_Health : MonoBehaviour
{
    [SerializeField] private float _invulTime = 2f;

    [SerializeField] private int _maxHP = 3;

    public int maxHP
    {
        get
        {
            return _maxHP;
        }
    }



    private float _invulTimer = 0;
    private int _hp = 1;

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
        if (_invulTimer > 0)
        {
            return;
        }
        _hp -= 1;
        OnHealthChanged();
        CheckDeath();
        TriggerInvulnerable();
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
    }

    private void CheckDeath()
    {
        if (_hp <= 0)
        {
            Actor.i.death.Dead();
        }
    }
}
