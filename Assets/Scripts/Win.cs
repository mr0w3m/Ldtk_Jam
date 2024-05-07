using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Win : MonoBehaviour
{

    [SerializeField] private HitBoxCheck _hitboxCheck;
    [SerializeField] private GameObject _winObj;


    private void Start()
    {
        _winObj.SetActive(false);
        _hitboxCheck.EnterCollider += WinGame;
    }

    private void WinGame()
    {
        _winObj.SetActive(true);
    }
}
