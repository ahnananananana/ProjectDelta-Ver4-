using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class hAdManager : MonoBehaviour//, IUnityAdsListener
{
    private static hAdManager instance;
    public static hAdManager current
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<hAdManager>();
            }
            if (instance == null)
            {
                GameObject gob = new GameObject
                {
                    name = "AdManager"
                };
                instance = gob.AddComponent<hAdManager>();
                DontDestroyOnLoad(gob);
            }
            return instance;
        }
    }

    private int _adStack;
    private int _maxStack;
    private DelVoid _del;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            DestroyImmediate(gameObject);
            return;
        }

#if UNITY_EDITOR
        Advertisement.Initialize("3693287", true);
        _maxStack = PlayerPrefs.GetInt("AdMaxStack", 10);
#else
        Advertisement.Initialize("3693287", false);
        _maxStack = PlayerPrefs.GetInt("AdMaxStack", 10);
#endif
        _adStack = PlayerPrefs.GetInt("AdStack", 0);
    }

    public int clearTime { get => _adStack; set => _adStack = value; }

    public void TryShow(DelVoid del = null)
    {
#if UNITY_EDITOR
        del?.Invoke();
        return;
#endif
        float curTime = Time.time;
        ++_adStack;
        PlayerPrefs.SetInt("AdStack", _adStack);

        if (Advertisement.IsReady() && _adStack >= _maxStack)
        {
            _del = del;

            _adStack = 0;
            PlayerPrefs.SetInt("AdStack", _adStack);
            hBGMController.current.Pause();
            var options = new ShowOptions { resultCallback = HandleShowResult };
            Advertisement.Show("video", options);
            return;
        }

        del?.Invoke();
    }

    private void HandleShowResult(ShowResult result)
    {
        hBGMController.current.UnPause();
        switch (result)
        {
            case ShowResult.Finished:
                break;
            case ShowResult.Skipped:
                break;
            case ShowResult.Failed:
                break;
        }
        _del?.Invoke();
        _del = null;
    }

    public void OnUnityAdsDidError(string message)
    {
        hBGMController.current.UnPause();
        _del?.Invoke();
        _del = null;
    }

    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        hBGMController.current.UnPause();
        _del?.Invoke();
        _del = null;
    }

    public void OnUnityAdsDidStart(string placementId)
    {
    }

    public void OnUnityAdsReady(string placementId)
    {
    }
}