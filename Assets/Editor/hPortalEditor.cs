using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(hPortal))]
public class hPortalEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        hPortal portal = (hPortal)target;

        //portal.SetColor(portal._color);
    }
}
