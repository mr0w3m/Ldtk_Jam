using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScreen : MonoBehaviour
{
    [SerializeField] private A_Input _input;
    [SerializeField] private SceneController _sceneController;
    [SerializeField] private Mover _cameraMover;
    [SerializeField] private string _sceneToLoad;
    [SerializeField] private AudioClip _sFX_startGame;


    void Start()
    {
        _cameraMover.MoveEnd += ResetTitleScreen;
        _input.ADown += StartGame;
        _input.BDown += CloseGame;
    }

    private void ResetTitleScreen()
    {
        _sceneController.LoadScene("Title");
    }

    private void StartGame()
    {
        AudioController.control.PlayClip(_sFX_startGame);
        _input.ADown -= StartGame;
        _sceneController.LoadScene(_sceneToLoad);
    }

    private void CloseGame()
    {
        Application.Quit();
    }
}
