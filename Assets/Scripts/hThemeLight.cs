using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hThemeLight : MonoBehaviour
{
    private void Awake()
    {
        var light = GetComponent<Light>();
        light.color = hColorManager.current.curColor;
    }
}
