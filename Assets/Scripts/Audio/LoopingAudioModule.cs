using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopingAudioModule : MonoBehaviour
{
    private AudioController _audioController;
    private float _waitTime;
    private AudioClip _clip;
    private bool _random;
    private float _volume;

    public string id;


    public void Init(AudioClip clip, AudioController ac, float timeBetweenLoop, bool randomizePitch, string id, float volume)
    {
        this._clip = clip;
        _audioController = ac;
        _waitTime = timeBetweenLoop;
        _random = randomizePitch;
        this.id = id;
        this._volume = volume;
        Play();
    }

    public void TerminateModule()
    {
        Destroy(this.gameObject);
    }

    public void ChangeClip(AudioClip newClip)
    {
        _clip = newClip;
    }

    private void Play()
    {
        Loop();
    }

    private void Loop()
    {
        float pitch = _random ? Random.Range(0.9f, 1.05f) : 1f;
        _audioController.PlayClip(_clip, pitch, _volume);
        StartCoroutine(Util.WaitAndCallRoutine(_waitTime, Loop));
    }
}