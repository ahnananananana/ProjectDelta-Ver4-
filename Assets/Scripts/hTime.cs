using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class hTime
{
    private static float _deltaTime, _fixedTime;
    static hTime()
    {
        _deltaTime = Time.deltaTime;
        _fixedTime = Time.fixedDeltaTime;
    }

    public static float deltaTime { get => _deltaTime; set => _deltaTime = value; }
    public static float fixedDeltaTime { get => _fixedTime; set => _fixedTime = value; }
}
