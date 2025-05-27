using System;
using System.Collections;
using System.Collections.Generic;
using Rewired;
using UnityEngine;

public class A_Input : MonoBehaviour
{
    [Header("Sensitivity")]
    [SerializeField] private float _leftStick;
    [SerializeField] private float _rightStick;

    private bool _aDown;
    private bool _bDown;
    private bool _xDown;
    private bool _yDown;
    private bool _rbDown;
    private bool _lbDown;
    private bool _selectDown;
    private bool _startDown;

    private bool _aUp;
    private bool _bUp;
    private bool _xUp;
    private bool _yUp;
    private bool _rbUp;
    private bool _lbUp;
    private bool _selectUp;
    private bool _startUp;

    private float _lsX;
    private float _lsY;

    private float _rsX;
    private float _rsY;

    public float _prevLsX = 0;
    public float _prevLsY = 0;
    public float _prevRsX = 0;
    public float _prevRsY = 0;

    private float _rt;
    private float _lt;

    public event Action ADown;
    public event Action BDown;
    public event Action XDown;
    public event Action YDown;
    public event Action RBDown;
    public event Action LBDown;
    public event Action SelectDown;
    public event Action StartDown;

    public event Action AUp;
    public event Action BUp;
    public event Action XUp;
    public event Action YUp;
    public event Action RBUp;
    public event Action LBUp;
    public event Action SelectUp; 
    public event Action StartUp;

    public event Action RSLostInput;
    public event Action LSLostInput;

    /// <summary>
    /// Invokes once the left stick has been fully pressed up.  Resets when stick goes back to 0
    /// </summary>
    public event Action LSPressedUp;

    public bool MouseMode = true;

    private bool _disableLS = false;

    public float LSX
    {
        get { return _lsX; }
    }

    public float LSY
    {
        get { return _lsY; }
    }

    public float RSX
    {
        get { return _rsX; }
    }

    public float RSY
    {
        get { return _rsY; }
    }

    public float RT
    {
        get { return _rt; }
    }

    public float LT
    {
        get { return _lt; }
    }

    private Rewired.Player _rePlayer;

    private void OnADown()
    {
        if (ADown != null)
        {
            ADown.Invoke();
        }
    }
    private void OnBDown()
    {
        if (BDown != null)
        {
            BDown.Invoke();
        }
    }
    private void OnXDown()
    {
        if (XDown != null)
        {
            XDown.Invoke();
        }
    }
    private void OnYDown()
    {
        if (YDown != null)
        {
            YDown.Invoke();
        }
    }
    private void OnRBDown()
    {
        if (RBDown != null)
        {
            RBDown.Invoke();
        }
    }
    private void OnLBDown()
    {
        if (LBDown != null)
        {
            LBDown.Invoke();
        }
    }
    private void OnSelectDown()
    {
        if (SelectDown != null)
        {
            SelectDown.Invoke();
        }
    }
    private void OnStartDown()
    {
        if (StartDown != null)
        {
            StartDown.Invoke();
        }
    }

    private void OnAUp()
    {
        if (AUp != null)
        {
            AUp.Invoke();
        }
    }
    private void OnBUp()
    {
        if (BUp != null)
        {
            BUp.Invoke();
        }
    }
    private void OnXUp()
    {
        if (XUp != null)
        {
            XUp.Invoke();
        }
    }
    private void OnYUp()
    {
        if (YUp != null)
        {
            YUp.Invoke();
        }
    }
    private void OnRBUp()
    {
        if (RBUp != null)
        {
            RBUp.Invoke();
        }
    }
    private void OnLBUp()
    {
        if (LBUp != null)
        {
            LBUp.Invoke();
        }
    }
    private void OnSelectUp()
    {
        if (SelectUp != null)
        {
            SelectUp.Invoke();
        }
    }
    private void OnStartUp()
    {
        if (StartUp != null)
        {
            StartUp.Invoke();
        }
    }

    private void OnRSLostInput()
    {
        if (RSLostInput != null)
        {
            RSLostInput.Invoke();
        }
    }

    private void OnLSLostInput()
    {
        if (LSLostInput != null)
        {
            LSLostInput.Invoke();
        }
    }

    private void OnLSPressedUp()
    {
        if (LSPressedUp != null)
        {
            LSPressedUp.Invoke();
        }
    }

    private void Awake()
    {
        InitRePlayer();
        StartCoroutine(InputLoop());
    }

    private void InitRePlayer()
    {
        _rePlayer = ReInput.players.GetPlayer(0);
    }

    private void Update()
    {
        if (MouseMode)
        {
            //check for controller
            ControllerCheck();
        }
        else
        {
            //check for mouse
            MouseCheck();
        }

        if (Actor.i != null)
        {
            if (MouseMode && Actor.i.throwing.throwing)
            {
                _disableLS = true;
            }
            else
            {
                _disableLS = false;
            }
        }
    }

    private void ControllerCheck()
    {
        if (_rePlayer == null)
        {
            return;
        }
        if (_rePlayer.controllers.GetLastActiveController().type == ControllerType.Joystick)
        {
            MouseMode = false;
        }
    }

    private void MouseCheck()
    {
        if (Mathf.Abs(Input.GetAxisRaw("Mouse X")) > 0.001f || Mathf.Abs(Input.GetAxisRaw("Mouse Y")) > 0.001f)
        {
            MouseMode = true;
        }
    }

    private IEnumerator InputLoop()
    {
        while (true)
        {
            ReadButtons();
            ReadTriggers();
            ReadSticks();
            TriggerEvents();
            yield return 0f;
        }
    }

    private void ReadButtons()
    {
        _aDown = _rePlayer.GetButtonDown("AButton");
        _bDown = _rePlayer.GetButtonDown("BButton");
        _xDown = _rePlayer.GetButtonDown("XButton");
        _yDown = _rePlayer.GetButtonDown("YButton");
        _rbDown = _rePlayer.GetButtonDown("RightBumper");
        _lbDown = _rePlayer.GetButtonDown("LeftBumper");
        _selectDown = _rePlayer.GetButtonDown("Select");
        _startDown = _rePlayer.GetButtonDown("Start");

        _aUp = _rePlayer.GetButtonUp("AButton");
        _bUp = _rePlayer.GetButtonUp("BButton");
        _xUp = _rePlayer.GetButtonUp("XButton");
        _yUp = _rePlayer.GetButtonUp("YButton");
        _rbUp = _rePlayer.GetButtonUp("RightBumper");
        _lbUp = _rePlayer.GetButtonUp("LeftBumper");
        _selectUp = _rePlayer.GetButtonUp("Select");
        _startUp = _rePlayer.GetButtonUp("Start");
    }

    private void ReadTriggers()
    {
        _rt = _rePlayer.GetAxisRaw("RightTrigger");
        _lt = _rePlayer.GetAxisRaw("LeftTrigger");
    }

    private void ReadSticks()
    {
        if (!_disableLS)
        {
            _lsX = _rePlayer.GetAxisRaw("LSX");
            _lsY = _rePlayer.GetAxisRaw("LSY");
        }
        _rsX = _rePlayer.GetAxisRaw("RSX");
        _rsY = _rePlayer.GetAxisRaw("RSY");

        if (Mathf.Abs(_lsX) < _leftStick)
        {
            _lsX = 0;
        }

        if (Mathf.Abs(_lsY) < _leftStick)
        {
            _lsY = 0;
        }

        if (Mathf.Abs(_rsX) < _rightStick)
        {
            _rsX = 0;
        }

        if (Mathf.Abs(_rsY) < _rightStick)
        {
            _rsY = 0;
        }

        //fire off an event when we lose input
        if (_lsX == 0 && _lsY == 0 && Mathf.Abs(_lsX) < Mathf.Abs(_prevLsX) && Mathf.Abs(_lsY) < Mathf.Abs(_prevLsY))
        {
            OnLSLostInput();
        }

        if (_rsX == 0 && _rsY == 0 && Mathf.Abs(_rsX) < Mathf.Abs(_prevRsX) && Mathf.Abs(_rsY) < Mathf.Abs(_prevRsY))
        {
            OnRSLostInput();
        }

        if (_lsY >= 0.75f && _prevLsY < 0.75f)
        {
            OnLSPressedUp();
        }

        _prevLsX = _lsX;
        _prevLsY = _lsY;
        _prevRsX = _rsX;
        _prevRsY = _rsY;
    }

    private void TriggerEvents()
    {
        if (_aDown) { OnADown(); }
        if (_bDown) { OnBDown(); }
        if (_xDown) { OnXDown(); }
        if (_yDown) { OnYDown(); }
        if (_rbDown) { OnRBDown(); }
        if (_lbDown) { OnLBDown(); }
        if (_selectDown) { OnSelectDown(); }
        if (_startDown) { OnStartDown(); }

        if (_aUp) { OnAUp(); }
        if (_bUp) { OnBUp(); }
        if (_xUp) { OnXUp(); }
        if (_yUp) { OnYUp(); }
        if (_rbUp) { OnRBUp(); }
        if (_lbUp) { OnLBUp(); }
        if (_selectUp) { OnSelectUp(); }
        if (_startUp) { OnStartUp(); }
    }
}