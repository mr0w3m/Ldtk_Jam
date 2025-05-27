using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;
using UnityEngine.UI;

public class ControlsIconUpdater : MonoBehaviour
{
    //icon data object with ref to each platform
    public ControlsIconDataObject _iconData;
    public SpriteRenderer _sprite;
    public Image _image;

    public bool _mainMenu = false;
    [SerializeField] private A_Input _input;

    void Update()
    {
        if (_mainMenu)
        {
            if (_input.MouseMode)
            {
                SetPC();
            }
            else
            {
                SetController();
            }
        }
        else
        {
            if (Actor.i != null)
            {
                if (Actor.i.input.MouseMode)
                {
                    SetPC();
                }
                else
                {
                    SetController();
                }
            }
        }
    }

    private void SetController()
    {
        if (_sprite != null)
        {
            _sprite.sprite = _iconData.GetControllerSprite();
        }
       
        if (_image != null)
        {
            _image.sprite = _iconData.GetControllerSprite();
        }
    }

    private void SetPC()
    {
        if (_sprite != null)
        {
            _sprite.sprite = _iconData.GetPCSprite();
        }

        if (_image != null)
        {
            _image.sprite = _iconData.GetPCSprite();
        }
    }
}
