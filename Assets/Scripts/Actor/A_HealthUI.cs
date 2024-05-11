using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class A_HealthUI : MonoBehaviour
{
    [SerializeField] private HPUISlot _hpChunk;
    [SerializeField] private GameObject _chunkParent;

    private List<HPUISlot> _hpSlots = new List<HPUISlot>();

    private float _timer;
    private float _timeToJiggleBar = 0.33f;
    private Vector3 _targetScale = new Vector3(2, 2, 1);

    private void Start()
    {
        Actor.i.health.HealthChanged += UpdateHP;

        for(int i = 0; i < Actor.i.health.maxHP; i++)
        {
            HPUISlot slot = Instantiate(_hpChunk, _chunkParent.transform);
            _hpSlots.Add(slot);
        }
    }

    private void Update()
    {
        if (_timer > 0)
        {
            _timer -= Time.deltaTime;
            
        }
        else
        {
            _timer = 0;
        }
        transform.localScale = Vector3.Lerp(transform.localScale, _targetScale, Util.MapValue(_timer, _timeToJiggleBar, 0, 0, 1));
    }

    private void UpdateHP()
    {
        _timer = _timeToJiggleBar;
        int currentHp = Actor.i.health.hp;
        Debug.Log("currentHP: " + currentHp);
        for (int i = 0; i < _hpSlots.Count; i++)
        {
            Debug.Log("slot: " + i + " <= currentHp - 1: " + (currentHp - 1));
            Debug.Log(i <= currentHp - 1);
            if (i <= currentHp - 1)
            {
                _hpSlots[i].SetHeartState(true);
            }
            else
            {
                _hpSlots[i].SetHeartState(false);
            }
        }
    }
}
