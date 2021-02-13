using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class hBGMController : MonoBehaviour
{
    private static hBGMController _instance;
    public static hBGMController current
    { 
    get
        {
            if (_instance == null)
                _instance = FindObjectOfType<hBGMController>();
            if (_instance == null)
            {
                var prefab = Resources.Load("GameObjects/BGMController/BGMController") as hBGMController;
                _instance = Instantiate(prefab);
                _instance.name = "BGMController";
                DontDestroyOnLoad(_instance);
            }
            return _instance;
        }
    }

    private hAudioController _audioController;
    [SerializeField]
    private AudioClip _bgmClip;
    private AudioMixerGroup mixer;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if(_instance == this)
        {
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            DestroyImmediate(gameObject);
            return;
        }

        mixer = hDatabase.current.audioMixerGroup;

        if (_audioController == null)
            _audioController = new hAudioController(gameObject);
        //Play();
    }

    public void Play()
    {
        if(mixer == null)
            mixer = hDatabase.current.audioMixerGroup;
        mixer.audioMixer.SetFloat("Master", 0f);
        if (_audioController == null)
            _audioController = new hAudioController(gameObject);
        if (_audioController.isPause || _audioController.isPlaying) 
            return;
        _audioController.Play(_bgmClip, 1, true);
    }

    public void Stop()
    {
        if (_audioController == null)
            _audioController = new hAudioController(gameObject);
        _audioController.Stop();
        if (mixer == null)
            mixer = hDatabase.current.audioMixerGroup;
        mixer.audioMixer.SetFloat("Master", -80f);
    }
    public void Pause() => _audioController.Pause();
    public void UnPause() => _audioController.UnPause();

}
