using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        if (control == null)
            control = this;
        else if (control != null)
            Destroy(gameObject);
    }

    public void PlayClip(AudioClip clip, float pitch = 1, float volume = 1)
    {
        AudioSource tempSource = _audioSources.FirstOrDefault(a => a.isPlaying == false);
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
        LoopingAudioModule module = _loopingAudios.FirstOrDefault(l => l.id == id);
        if (module != null)
        {
            _loopingAudios.Remove(module);
            module.TerminateModule();
        }
    }

    public void ChangeLoopingAudioClip(AudioClip clip, string id)
    {
        LoopingAudioModule module = _loopingAudios.FirstOrDefault(l => l.id == id);
        module.ChangeClip(clip);
    }
}