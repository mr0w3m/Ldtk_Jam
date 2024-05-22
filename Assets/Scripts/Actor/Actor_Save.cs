using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor_Save : MonoBehaviour
{

    private PlayerSaveData _playerSaveData;

    public event Action saveLoadedEvent;
    private void OnSaveLoadedEvent()
    {
        if (saveLoadedEvent != null)
        {
            Debug.Log("****PlayerSaveLoaded");
            saveLoadedEvent.Invoke();
        }
    }

    //All the subscribe events happen on start so load after all of that occurs in update after a small timer

    bool saveLoaded = false;
    private float timer = 0.666f;

    private void LateUpdate()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
        else if (!saveLoaded)
        {
            saveLoaded = true;
            Load();
        }
    }



    public PlayerSaveData GetPlayerSave()
    {
        return _playerSaveData;
    }

    public void CallSave()
    {
        Save();
    }

    private void Save()
    {
        string saveString = JsonUtility.ToJson(_playerSaveData);
        PlayerPrefs.SetString("PlayerSaveData", saveString);
    }

    private void Load()
    {
        string version = PlayerPrefs.GetString("Version", "0");

        if (version == Application.version)
        {
            string saveString = PlayerPrefs.GetString("PlayerSaveData", "");
            Debug.Log("ActorSave| " + saveString);
            if (!string.IsNullOrEmpty(saveString))
            {
                _playerSaveData = JsonUtility.FromJson<PlayerSaveData>(saveString);
                Debug.Log("LoadedThePlayerSaveData from file");
            }
            else
            {
                _playerSaveData = new PlayerSaveData();
            }
        }
        else
        {
            _playerSaveData = new PlayerSaveData();
        }
        OnSaveLoadedEvent();
        Debug.Log("SaveLoadedEvent");
    }
}