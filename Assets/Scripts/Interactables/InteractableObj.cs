using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
    /// Interactable object:
    ///      An interface with interactive needs
/// </summary>
public interface InteractableObj
{
    void Interact();
    void Highlight(bool state);
}
