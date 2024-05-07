using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimplerAnimation : MonoBehaviour
{
    public enum MotionType
    {
        Relative,
        Absolute
    }

    public enum LoopMode
    {
        OneShot,
        ResetTime,
        Loop
    }

    [SerializeField] private bool _playOnStart;
    //[SerializeField] private bool _loop;
    [SerializeField] private LoopMode _loopMode;
    [SerializeField] private bool _randomizeStartTime;
    [SerializeField, Range(0,1)] private float _currentTime;
    [SerializeField] private bool _applyPhysics;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private Transform _animationObject;
    [SerializeField] private Space _space;
    [SerializeField] private MotionType _motionType;
    [SerializeField] private bool _animatePosition;
    [SerializeField] private Vector3 _startingPosition;
    [SerializeField] private Vector3 _endingPosition;
    [SerializeField] private bool _animateRotation;
    [SerializeField] private Vector3 _startingRotation;
    [SerializeField] private Vector3 _endingRotation;
    [SerializeField] private bool _animateScale;
    [SerializeField] private Vector3 _startingScale = Vector3.one;
    [SerializeField] private Vector3 _endingScale = Vector3.one;
    [SerializeField] private float _animationLength = 1f;
    [SerializeField] private AnimationCurve _easingCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    private float _previousCurrentTime;
    public bool _forwards { get; private set; }
    private Vector3 _adjustedEndingPosition;
    private Vector3 _adjustedEndingRotation;
    private Vector3 _adjustedEndingScale;
    private float _timeScale = 1f;
    private bool _initializedSpace;

    public bool playing { get; private set; }
    public float animationLength => _animationLength;
    public float currentNormalizedTime => _currentTime;
    public float currentRealTime => _currentTime * _animationLength;
    public LoopMode loopMode => _loopMode;

    private void Reset()
    {
        _playOnStart = true;
        _loopMode = LoopMode.Loop;
        _animationObject = transform;
    }

    private void Start()
    {
        InitializeSpace();
        AlignToTimer();

        if(_randomizeStartTime)
        {
            _currentTime = Random.value;
        }

        if (_playOnStart)
        {
            Play(false);
        }
    }

    private void InitializeSpace()
    {
        if(!_initializedSpace)
        {
            _initializedSpace = true;

            if (_motionType == MotionType.Relative)
            {
                if (_space == Space.Self)
                {
                    _startingPosition = _animationObject.localPosition;
                    _startingRotation = _animationObject.localRotation.eulerAngles;
                }
                else if (_space == Space.World)
                {
                    _startingPosition = _animationObject.position;
                    _startingRotation = _animationObject.rotation.eulerAngles;
                }
            }

        }
    }

    private void Update()
    {
        if (playing)
        {
            if (_forwards)
            {
                AlignToTimer();

                if (_currentTime < 2f)
                {
                    _currentTime += (Time.deltaTime / _animationLength) * _timeScale;
                }

                if (_currentTime >= 1f)
                {
                    if(_loopMode == LoopMode.Loop)
                    {
                        _currentTime = 0f;
                    }
                    else if(_loopMode == LoopMode.ResetTime)
                    {
                        _currentTime = 0f;
                        playing = false;
                    }
                    else if (_loopMode == LoopMode.OneShot)
                    {
                        _currentTime = 1f;
                        playing = false;
                    }

                    AlignToTimer();
                }
            }
            else
            {
                AlignToTimer();

                if (_currentTime > -1f)
                {
                    _currentTime -= (Time.deltaTime / _animationLength) * _timeScale;
                }

                if (_currentTime <= 0)
                {
                    if (_loopMode == LoopMode.Loop)
                    {
                        _currentTime = 1f;
                    }
                    else if (_loopMode == LoopMode.ResetTime)
                    {
                        _currentTime = 1f;
                        playing = false;
                    }
                    else if (_loopMode == LoopMode.OneShot)
                    {
                        _currentTime = 0f;
                        playing = false;
                    }

                    AlignToTimer();
                }
            }
        }
        else
        {
            if (_currentTime != _previousCurrentTime)
            {
                AlignToTimer();
            }

            _previousCurrentTime = _currentTime;
        }
    }

    public void SetCurrentNormalizedTime(float newTime)
    {
        _currentTime = newTime;
    }

    public void SetCurrentRealTime(float newTime)
    {
        _currentTime = newTime / _animationLength;
    }

    public void SetNewLength(float newLength)
    {
        _animationLength = newLength;
    }

    public void SetTimeScale(float newTimeScale)
    {
        _timeScale = newTimeScale;
    }

    private void AlignToTimer()
    {
        float curvedTimer = _easingCurve.Evaluate(_currentTime);

        if(_motionType == MotionType.Relative)
        {
            _adjustedEndingPosition = _startingPosition + _endingPosition;
            _adjustedEndingRotation = _startingRotation + _endingRotation;
            _adjustedEndingScale = _endingScale;
        }
        else if(_motionType == MotionType.Absolute)
        {
            _adjustedEndingPosition = _endingPosition;
            _adjustedEndingRotation = _endingRotation;
            _adjustedEndingScale = _endingScale;
        }

        if (_space == Space.Self)
        {
            if (_animatePosition)
            {
                if (_applyPhysics)
                {
                    Vector3 newLocation = Vector3.LerpUnclamped(_startingPosition, _adjustedEndingPosition, curvedTimer);

                    if (_animationObject.parent != null)
                    {
                        newLocation = _animationObject.parent.TransformPoint(newLocation);
                    }

                    _rigidbody.MovePosition(newLocation);
                }
                else
                {
                    _animationObject.localPosition = Vector3.LerpUnclamped(_startingPosition, _adjustedEndingPosition, curvedTimer);
                }
            }

            if (_animateRotation)
            {
                if (_applyPhysics)
                {
                    Quaternion newRotation = Quaternion.SlerpUnclamped(Quaternion.Euler(_startingRotation), Quaternion.Euler(_adjustedEndingRotation), curvedTimer);

                    if(_animationObject.parent != null)
                    {
                        newRotation = _animationObject.parent.rotation * newRotation;
                    }

                    _rigidbody.MoveRotation(newRotation);
                }
                else
                {
                    _animationObject.localRotation = Quaternion.SlerpUnclamped(Quaternion.Euler(_startingRotation), Quaternion.Euler(_adjustedEndingRotation), curvedTimer);
                }
            }
        }
        else if(_space == Space.World)
        {
            if (_animatePosition)
            {
                if (_applyPhysics)
                {
                    _rigidbody.MovePosition(Vector3.LerpUnclamped(_startingPosition, _adjustedEndingPosition, curvedTimer));
                }
                else
                {
                    _animationObject.position = Vector3.LerpUnclamped(_startingPosition, _adjustedEndingPosition, curvedTimer);
                }
            }

            if (_animateRotation)
            {
                if (_applyPhysics)
                {
                    _rigidbody.MoveRotation(Quaternion.SlerpUnclamped(Quaternion.Euler(_startingRotation), Quaternion.Euler(_adjustedEndingRotation), curvedTimer));
                }
                else
                {
                    _animationObject.rotation = Quaternion.SlerpUnclamped(Quaternion.Euler(_startingRotation), Quaternion.Euler(_adjustedEndingRotation), curvedTimer);
                }
            }
        }

        if (_animateScale)
        {
            _animationObject.localScale = Vector3.LerpUnclamped(_startingScale, _adjustedEndingScale, curvedTimer);
        }
    }

    [ContextMenu("Play")]
    public void Play()
    {
        Play(true);
    }

    public void Play(bool resetTime)
    {
        InitializeSpace();
        _forwards = true;
        playing = true;

        if (resetTime)
        {
            _currentTime = 0;
            AlignToTimer();
        }
    }

    public void Pause()
    {
        playing = false;
    }

    [ContextMenu("Stop")]
    public void Stop()
    {
        Stop(true);
    }

    public void Stop(bool resetTime)
    {
        playing = false;

        if (resetTime)
        {
            _currentTime = 0;
            AlignToTimer();
        }
    }

    [ContextMenu("Play Reverse")]
    public void PlayReverse()
    {
        PlayReverse(true);
    }

    public void PlayReverse(bool resetTime)
    {
        InitializeSpace();
        _forwards = false;
        playing = true;

        if (resetTime)
        {
            _currentTime = 1f;
            AlignToTimer();
        }
    }
}
