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

    [SerializeField] private AnimationCurve _lightDropOffCurve;
    [SerializeField] private AudioClip _starvingClip;

    [SerializeField] private Sprite _hungerAlertIcon;


    [SerializeField] private float _shakeTime = 0.2f;

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
                if (_hunger <= (15))
                {
                    A_CameraController.i.AddCameraShake(_shakeTime, 0.1f);
                    AudioController.control.PlayClip(_starvingClip, Random.Range(0.85f, 2), 0.5f);
                }
            }
        }

        _hungerText.text = _hunger.ToString();
        if (_hunger <= 0)
        {
            _death.Dead();
        }


        _hungerImg.color = new Color(1, 1, 1, Util.MapValue(_hunger, 0, 50, 1, 0));

        //evaluate the curve set, to change the intensity of the light from 0.7 to 0 based on the hunger of the player relative to their total hunger. (phew that's a mouthful)
        GlobalLightController.i.globalLightRef.intensity = _lightDropOffCurve.Evaluate(Util.MapValue(_hunger, 50, 0, 0.7f, 0));

        
    }

    public void EatFood(int amt)
    {
        if (amt == 0)
        {
            return;
        }

        _hunger += amt;
        if (_hunger > _totalStartHunger)
        {
            _hunger = _totalStartHunger;
        }


        string alertString = amt.ToString();
        if (amt > 0)
        {
            alertString = ("+" + alertString);
        }

        Actor.i.alerts.Alert(alertString, _hungerAlertIcon);
    }
}
