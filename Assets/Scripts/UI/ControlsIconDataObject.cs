using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ControlsIcon_DataObject")]
public class ControlsIconDataObject : ScriptableObject
{
    public Sprite controllerSprite;
    public Sprite pcSprite;

    public Sprite GetControllerSprite()
    {
        return controllerSprite;
    }

    public Sprite GetPCSprite()
    {
        return pcSprite;
    }
}
