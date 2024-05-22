using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class A_HealthUI : MonoBehaviour
{
    [SerializeField] private A_Health _health;
    [SerializeField] private HPUISlot _hpChunk;
    [SerializeField] private GameObject _chunkParent;

    private List<HPUISlot> _hpSlots = new List<HPUISlot>();
    private bool _loaded = false;

    private float _timer;
    private float _timeToJiggleBar = 0.33f;
    private Vector3 _targetScale = new Vector3(2, 2, 1);

    private void Awake()
    {
        _health.DataLoaded += Initialize;
    }

    private void Initialize()
    {
        Actor.i.health.HealthChanged += UpdateHP;

        for (int i = 0; i < Actor.i.health.maxHP; i++)
        {
            HPUISlot slot = Instantiate(_hpChunk, _chunkParent.transform);
            _hpSlots.Add(slot);
        }
    }
    public void ReInitialize()
    {
        foreach (HPUISlot slot in _hpSlots)
        {
            Destroy(slot.gameObject);
        }
        _hpSlots.Clear();

        for (int i = 0; i < Actor.i.health.maxHP; i++)
        {
            HPUISlot slot = Instantiate(_hpChunk, _chunkParent.transform);
            _hpSlots.Add(slot);
        }
    }

    private void Update()
    {
        //if (_timer > 0)
        //{
            //_timer -= Time.deltaTime;
            
        //}
        //else
        //{
            //_timer = 0;
        //}
        //transform.localScale = Vector3.Lerp(transform.localScale, _targetScale, Util.MapValue(_timer, _timeToJiggleBar, 0, 0, 1));
    }

    private void UpdateHP()
    {
        //_timer = _timeToJiggleBar;
        int currentHp = Actor.i.health.hp;

        for (int i = 0; i < _hpSlots.Count; i++)
        {
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
