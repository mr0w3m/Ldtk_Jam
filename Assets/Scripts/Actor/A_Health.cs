using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class A_Health : MonoBehaviour
{

    private int _hp = 1;

    public event Action HealthChanged;

    private void OnHealthChanged()
    {
        if (HealthChanged != null)
        {
            HealthChanged.Invoke();
        }
    }

    public void Hit()
    {
        _hp -= 1;
        OnHealthChanged();
        CheckDeath();
    }

    private void CheckDeath()
    {
        if (_hp <= 0)
        {
            Actor.i.death.Dead();
        }
    }
}
