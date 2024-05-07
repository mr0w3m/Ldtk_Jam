using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.LowLevel;

/// <summary>
/// An easy way to define a spline, and have a line renderer render along it
/// </summary>
public class SplineRenderer : MonoBehaviour
{
    public static int _rateLimitCounter;

    [SerializeField] private int _resolution = 10;
    [SerializeField] private LineRenderer _lineRenderer;
    [SerializeField] private Transform _pointA;
    [SerializeField] private Transform _handleA;
    [SerializeField] private Transform _pointB;
    [SerializeField] private Transform _handleB;
    private Vector3[] _positions;

    private bool render = true;

    public void SetPointA(Transform newPointA)
    {
        _pointA = newPointA;
    }

    public void SetPointB(Transform newPointB)
    {
        _pointB = newPointB;
    }

    public void SetHandleA(Transform newHandleA)
    {
        _handleA = newHandleA;
    }

    public void SetHandleB(Transform newHandleB)
    {
        _handleB = newHandleB;
    }

    private void Start()
    {
        InitializePoints();
    }

    public void InitializePoints()
    {
        _positions = new Vector3[_resolution];
        _lineRenderer.positionCount = _resolution;

        if (_lineRenderer == null || _pointA == null || _handleA == null || _handleB == null || _pointB == null || !render)
        {
            enabled = false;
        }
        else
        {
            BezierCurve.GetBezierCurve(_pointA.position, _handleA.position, _handleB.position, _pointB.position, _positions);
            _lineRenderer.SetPositions(_positions);
        }
    }

    private void LateUpdate()
    {
            Vector3 newPointAPosition = _pointA.position;
            Vector3 newHandleAPosition = _handleA.position;
            Vector3 newPointBPosition = _pointB.position;
            Vector3 newHandleBPosition = _handleB.position;

            BezierCurve.GetBezierCurve(newPointAPosition, newHandleAPosition, newHandleBPosition, newPointBPosition, _positions);
            _lineRenderer.SetPositions(_positions);
    }
}
