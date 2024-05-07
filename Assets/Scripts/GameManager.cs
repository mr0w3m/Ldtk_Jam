using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private SceneController _sceneController;

    private bool _checkForReload = false;
    private bool _reloading = false;

    private void Start()
    {
        Actor.i.death.playerDied += CheckForReload;
    }

    private void CheckForReload()
    {
        Actor.i.input.ADown += Reload;
        _checkForReload = true;
    }

    private void Reload()
    {
        if (!_checkForReload || _reloading)
        {
            Debug.Log("Don'treloadgame");
            return;
        }
        _sceneController.LoadScene("Cave");
        _reloading = true;
    }
}
