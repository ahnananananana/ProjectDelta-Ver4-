using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class hCavasScaler : MonoBehaviour
{
    private CanvasScaler _canvasScaler;
    private void Awake()
    {
        _canvasScaler = GetComponent<CanvasScaler>();/*
#if UNITY_EDITOR
        var resol = new Vector2(1920, 1920);
#else
        var ratio = Screen.currentResolution.height / (float)Screen.currentResolution.width;
        var resol = new Vector2(1080 * ratio, 1080);
#endif*/
        var ratio = Screen.currentResolution.height / (float)Screen.currentResolution.width;
        var resol = new Vector2(1080, 1080 * ratio);

        _canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        _canvasScaler.referenceResolution = resol;
        _canvasScaler.matchWidthOrHeight = .5f;
    }
}
