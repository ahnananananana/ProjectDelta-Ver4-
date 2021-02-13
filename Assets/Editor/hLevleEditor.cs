using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(hLevel))]
public class hLevleEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        hLevel level = (hLevel)target;

        if(GUILayout.Button("Serialize"))
        {
            level.Save();
        }
    }
}
