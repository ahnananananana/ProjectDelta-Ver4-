using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class hAudioController
{
    private AudioSource _audioSource;
    private bool _isPause;
    public bool isPlaying => _audioSource.isPlaying;

    public bool isPause { get => _isPause; set => _isPause = value; }

    public hAudioController(GameObject gameObject)
    {
        _audioSource = gameObject.GetComponent<AudioSource>();
        if(_audioSource == null)
            _audioSource = gameObject.AddComponent<AudioSource>();
        _audioSource.outputAudioMixerGroup = hDatabase.current.audioMixerGroup;
    }

    public void Play(AudioClip clip = null, float pitch = 1, bool loop = false)
    {
        if(clip != null)
            _audioSource.clip = clip;
        _audioSource.pitch = pitch;
        _audioSource.loop = loop;
        _audioSource.Play();
    }

    public void Stop()
    {
        _audioSource.Stop();
        _audioSource.clip = null;
    }

    public void Pause()
    {
        if (!_audioSource.isPlaying) 
            return;
        _isPause = true;
        _audioSource.Pause();
    } 
    public void UnPause() 
    {
        if (!_isPause)
            return;
        _isPause = false;
        _audioSource.UnPause();
    } 
}
