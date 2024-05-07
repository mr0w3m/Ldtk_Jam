using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
    [SerializeField] private GameObject _leader;
    [SerializeField] private Vector2 _offset;

    [SerializeField] private float _maxDistance;
    [SerializeField] private Vector2 _maxDistanceDelta;

    [SerializeField] private AnimationCurve _curve;

    private Vector2 _additiveOffset = Vector2.zero;

    public void Init(GameObject go)
    {
        _leader = go;
    }

    private void FixedUpdate()
    {
        if (_leader == null)
        {
            return;
        }

        Vector3 current = transform.position;
        Vector3 moveToPos = new Vector3((_leader.transform.position.x + _offset.x + _additiveOffset.x), (_leader.transform.position.y + _offset.y + _additiveOffset.y), -10);
        Vector2 distance = (Vector2) moveToPos - (Vector2) transform.position;
        transform.position = Vector3.MoveTowards(current, moveToPos, Util.MapValue(Mathf.Abs(distance.magnitude), 0, _maxDistance, _maxDistanceDelta.x, _maxDistanceDelta.y));
    }

    public void AddOffset(Vector2 offset)
    {
        _additiveOffset = offset;
    }
}