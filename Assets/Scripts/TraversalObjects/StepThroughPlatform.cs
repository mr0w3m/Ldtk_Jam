using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepThroughPlatform : MonoBehaviour
{
    [SerializeField] private Collider2D _platformToDisable;

    private float _timer;
    private float _timeToReenable = 1;
    private bool _disabled = false;

    void Update()
    {
        if (_timer > 0)
        {
            _timer -= Time.deltaTime;
        }
        else if (_disabled)
        {
            _disabled = false;
            _platformToDisable.enabled = true;
        }
    }


    public void StepThrough()
    {
        if (_disabled)
        {
            return;
        }
        _platformToDisable.enabled = false;
        _timer = _timeToReenable;
        _disabled = true;
    }

}
