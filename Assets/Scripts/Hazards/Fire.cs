using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    [SerializeField] private AudioClip _fireLoop;
    [SerializeField] private bool _alwaysPlay = false;

    private bool _soundLooping = false;

    private float _randomfloat;

    void Start()
    {
        _randomfloat = Random.Range(0, 10000);
    }

    // Update is called once per frame
    void Update()
    {
        if (_alwaysPlay)
        {
            if (!_soundLooping)
            {
                _soundLooping = true;
                AudioController.control.PlayLoopingAudio(_fireLoop, _fireLoop.length, false, "fireLoop" + _randomfloat, 0.666f);
            }
            return;
        }

        if (Actor.i != null)
        {
            if (Vector2.Distance((Vector2)transform.position, (Vector2)Actor.i.playerCenterT.position) < 8)
            {
                //start loop
                if (!_soundLooping)
                {
                    _soundLooping = true;
                    AudioController.control.PlayLoopingAudio(_fireLoop, _fireLoop.length, false, "fireLoop" + _randomfloat, 0.666f);
                }
            }
            else if (_soundLooping)
            {
                //end loop
                _soundLooping = false;
                AudioController.control.StopLoopingAudio("fireLoop" + _randomfloat);
            }
        }
    }

    private void OnDestroy()
    {
        if (_soundLooping) 
        {
            _soundLooping = false;
            AudioController.control.StopLoopingAudio("fireLoop" + _randomfloat);
        }
    }
}
