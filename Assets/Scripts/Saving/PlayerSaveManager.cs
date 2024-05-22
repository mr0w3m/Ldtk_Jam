using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSaveManager : MonoBehaviour
{
    public static PlayerSaveManager i;

    private bool _initialized = false;

    private void Awake()
    {
        if (i == null)
        {
            i = this;
            _playerSaveData = new PlayerSaveData();
            _initialized = true;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        if (!_initialized)
        {
            return;
        }
        this.gameObject.transform.SetParent(null);
        Debug.Log("DontDestroyOnLoad");
        DontDestroyOnLoad(this.gameObject);
    }

    private PlayerSaveData _playerSaveData;

    public PlayerSaveData playerSaveData
    {
        get { return _playerSaveData; }
        set { _playerSaveData = new PlayerSaveData(value); }
    }
}
