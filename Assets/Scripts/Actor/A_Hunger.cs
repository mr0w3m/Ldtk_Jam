using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class A_Hunger : MonoBehaviour
{

    [SerializeField] private int _hunger;
    public int hunger
    {
        get { return _hunger; }
    }
    [SerializeField] private float _timeToLoseHunger;
    [SerializeField] private int _totalStartHunger;
    public int totalStartHunger
    {
        get { return _totalStartHunger; }
        set
        {
            _totalStartHunger = value;
        }
    }


    [SerializeField] private TextMeshProUGUI _hungerText;
    [SerializeField] private A_Death _death;

    [SerializeField] private Image _hungerImg;
    [SerializeField] private GameObject _shakeObj;

    private bool _initialized = false;
    private float _timer;


    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        PlayerSaveData tempSave = PlayerSaveManager.i.playerSaveData;
        _timer = _timeToLoseHunger;
        _totalStartHunger = tempSave.totalHunger;
        _hunger = tempSave.hunger;
        _initialized = true;
    }

    private void Update()
    {
        if (!_initialized)
        {
            return;
        }

        if (_death.playerDead || Actor.i.paused)
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


        _hungerImg.color = new Color(1, 1, 1, Util.MapValue(_hunger, 0, _totalStartHunger, 1, 0));
    }

    public void EatFood(int amt)
    {
        _hunger += amt;
        if (_hunger > _totalStartHunger)
        {
            _hunger = _totalStartHunger;
        }
    }
}
