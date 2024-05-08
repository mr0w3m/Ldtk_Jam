using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScreen : MonoBehaviour
{
    [SerializeField] private A_Input _input;
    [SerializeField] private SceneController _sceneController;
    [SerializeField] private Mover _cameraMover;


    void Start()
    {
        _input.ADown += StartGame;
        _cameraMover.MoveEnd += ResetTitleScreen;
    }

    private void ResetTitleScreen()
    {
        _sceneController.LoadScene("Title");
    }

    private void StartGame()
    {
        _input.ADown -= StartGame;
        _sceneController.LoadScene("Cave");
    }
}
