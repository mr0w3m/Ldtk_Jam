using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{


    [SerializeField] private SceneController _sceneController;
    [SerializeField] private GameObject _startmenuUI;
    [SerializeField] private string _startingLevel;

    private bool _checkForReload = false;
    private bool _reloading = false;

    private bool _menu = false;

    private void Start()
    {
        Actor.i.death.playerDied += CheckForReload;
        Actor.i.input.SelectDown += ToggleMenu;
        Actor.i.input.ADown += Restart;
        Actor.i.input.BDown += MainMenu;
    }

    private void Restart()
    {
        if (_menu)
        {
            _sceneController.LoadScene("Cave");
        }
    }

    private void MainMenu()
    {
        if (_menu)
        {
            _sceneController.LoadScene("Title");
        }
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
            return;
        }
        _sceneController.LoadScene(_startingLevel);
        _reloading = true;
    }

    private void ToggleMenu()
    {
        if (_menu)
        {
            _menu = false;
            _startmenuUI.SetActive(false);
            Actor.i.paused = false;
        }
        else
        {
            _menu = true;
            _startmenuUI.SetActive(true);
            Actor.i.paused = true;
        }
    }
}
