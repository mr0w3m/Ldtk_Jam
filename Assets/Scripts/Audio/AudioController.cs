using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    public static AudioController control;
    [SerializeField] private AudioSource _sourceModule;
    [SerializeField] private List<AudioSource> _audioSources;
    [SerializeField] private GameObject _audioSourceHolder;

    [SerializeField] private List<LoopingAudioModule> _loopingAudios;
    [SerializeField] private LoopingAudioModule _loopingAudioPrefab;
    [SerializeField] private GameObject _loopingModuleHolder;

    void Start()
    {
        DontDestroyOnLoad(gameObject);
        if (control == null)
            control = this;
        else if (control != null)
            Destroy(gameObject);
    }

    public void PlayClip(AudioClip clip, float pitch = 1, float volume = 1)
    {
        AudioSource tempSource = null;
        foreach (AudioSource source in _audioSources)
        {
            if (source.isPlaying == false)
            {
                tempSource = source;
                break;
            }
        }

        if (tempSource == null)
        {
            tempSource = AddNewSource();
        }
        tempSource.volume = volume;
        tempSource.clip = clip;
        tempSource.pitch = pitch;
        tempSource.Play();
    }

    private AudioSource AddNewSource()
    {
        AudioSource tempSource = Instantiate(_sourceModule, _audioSourceHolder.transform) as AudioSource;
        _audioSources.Add(tempSource);
        return tempSource;
    }

    public void PlayLoopingAudio(AudioClip clip, float waitTimeBetween, bool randomizePitch, string id, float volume)
    {
        LoopingAudioModule tempModule = Instantiate(_loopingAudioPrefab, _loopingModuleHolder.transform) as LoopingAudioModule;
        tempModule.Init(clip, this, waitTimeBetween, randomizePitch, id, volume);
        _loopingAudios.Add(tempModule);
    }

    public void StopLoopingAudio(string id)
    {
        LoopingAudioModule module = null;

        foreach (LoopingAudioModule tempModule in _loopingAudios)
        {
            if (tempModule.id == id)
            {
                module = tempModule;
            }
        }

        if (module != null)
        {
            _loopingAudios.Remove(module);
            module.TerminateModule();
        }
    }

    public void ChangeLoopingAudioClip(AudioClip clip, string id)
    {
        LoopingAudioModule module = null;

        foreach (LoopingAudioModule tempModule in _loopingAudios)
        {
            if (tempModule.id == id) 
            {
                module = tempModule;
            }
        }
        if (module != null)
        {
            module.ChangeClip(clip);
        }
    }

    public void CleanUp()
    {
        foreach(AudioSource source in _audioSources) 
        {
            if (source != null) 
            {
                source.Stop();
            }
        }

        foreach (LoopingAudioModule tempModule in _loopingAudios)
        {
            if (tempModule != null)
            {
                tempModule.TerminateModule();
                Destroy(tempModule.gameObject);
            }
        }
        _loopingAudios.Clear();
    }

    public bool IsTrackPlaying(string id)
    {
        foreach(LoopingAudioModule tempModule in _loopingAudios)
        {
            if (tempModule != null)
            {
                if (tempModule.id == id)
                {
                    return true;
                }
            }
        }
        return false;
    }
}