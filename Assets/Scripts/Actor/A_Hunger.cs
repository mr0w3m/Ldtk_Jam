using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class A_Hunger : MonoBehaviour
{

    [SerializeField] private int _hunger;
    [SerializeField] private float _timeToLoseHunger;
    [SerializeField] private int _totalStartHunger = 100;


    [SerializeField] private TextMeshProUGUI _hungerText;
    [SerializeField] private A_Death _death;

    [SerializeField] private Image _hungerImg;
    [SerializeField] private GameObject _shakeObj;


    private float _timer;


    private void Start()
    {
        _timer = _timeToLoseHunger;
        _hunger = _totalStartHunger;
    }

    private void Update()
    {
        if (_death.playerDead)
        {
            return;
        }
        if (_hunger > 0)
        {
            if (_timer > 0)
            {
                _timer -= Time.deltaTime;
            }
            else
            {
                _hunger -= 1;
                _timer = _timeToLoseHunger;
            }
        }

        _hungerText.text = _hunger.ToString();
        if (_hunger <= 0)
        {
            _death.Dead();
        }


        _hungerImg.color = new Color(1, 1, 1, Util.MapValue(_hunger, 0, 100, 1, 0));
    }

    public void EatFood(int amt)
    {
        _hunger += amt;
        if (_hunger > 100)
        {
            _hunger = 100;
        }
    }
}
