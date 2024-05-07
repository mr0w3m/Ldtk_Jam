using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearPlatform : MonoBehaviour
{
    [SerializeField] PlatformEffector2D _platformEffector;


    public void FlipPlatformDirection()
    {
        _platformEffector.rotationalOffset = 0;
    }
}
