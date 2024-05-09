using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;

public class PerlinMover : MonoBehaviour
{
    public enum ObjectType
    {
        ui,
        gameobject
    }

    [SerializeField] private GameObject _moveGameObject;
    [SerializeField] private RectTransform _moveUIObject;
    [SerializeField] private float _perlinSpeed;
    [SerializeField] private Vector2 _perlinStrength;

    [SerializeField] private float _movetowardsSpeed;

    public ObjectType typeToMove;


    private float _timeOffset = 0;

    private void Update()
    {
        _timeOffset += Time.deltaTime;

        if (typeToMove == ObjectType.ui)
        {
            _moveUIObject.anchoredPosition = new Vector2(Mathf.Clamp(Mathf.PerlinNoise(_timeOffset, _timeOffset), 0, 1) * _perlinStrength.x, Mathf.Clamp(Mathf.PerlinNoise(_timeOffset, _timeOffset), 0, 1) * _perlinStrength.y);
        }
        else if (typeToMove == ObjectType.gameobject)
        {
            _moveGameObject.transform.position = Vector2.MoveTowards(_moveGameObject.transform.position, new Vector2(Mathf.Clamp(Mathf.PerlinNoise(_timeOffset, _timeOffset), 0, 1) * _perlinStrength.x, Mathf.Clamp(Mathf.PerlinNoise(_timeOffset, _timeOffset), 0, 1) * _perlinStrength.y), _movetowardsSpeed * Time.deltaTime);
        }
    }
}
