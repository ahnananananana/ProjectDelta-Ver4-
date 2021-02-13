using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hGround : hLevelObject
{
    private void Awake()
    {
        gameObject.layer = LayerMask.NameToLayer("Ground");
        OnAwake();
    }

    protected virtual void OnAwake() { }
}
