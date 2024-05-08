using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour
{
    [SerializeField] private GameObject _go;
    [SerializeField] private GameObject _startPos;
    [SerializeField] private GameObject _endPos;
    [SerializeField] private AnimationCurve _curve;


    [SerializeField] private float _timeTo;


    public event Action MoveEnd;


    private void Start()
    {
        StartCameraMover();
    }

    private void StartCameraMover()
    {
        StartCoroutine(MoveRoutine());
    }

    private IEnumerator MoveRoutine()
    {
        float timer = _timeTo;

        while (timer > 0)
        {
            timer -= Time.deltaTime;
            _go.transform.position = Vector3.Lerp(_startPos.transform.position, _endPos.transform.position, _curve.Evaluate(Util.MapValue(timer, _timeTo, 0, 0, 1)));
            yield return 0f;
        }
        if (MoveEnd != null) { MoveEnd.Invoke(); }
    }
}