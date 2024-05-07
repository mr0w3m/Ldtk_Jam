using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A_FacingDirection : MonoBehaviour
{
    [SerializeField] private A_Movement _movement;
    [SerializeField] private List<GameObject> _objectsToFlip;

    private void Start()
    {
        _movement.FlipDirection += FlipDirection;
    }

    private void FlipDirection()
    {
        _objectsToFlip.ForEach(o => o.transform.localScale = new Vector2(-o.transform.localScale.x, o.transform.localScale.y));
    }
}