using UnityEditor;
using UnityEngine;

// ----------------------------------------------------------------------------
// Author: Alexandre Brull - Pandaroo
// https://pandaroo.be
// ----------------------------------------------------------------------------

namespace Pandaroo.autoruletile
{
    [CustomEditor(typeof(AutoRuleTile))]
    [CanEditMultipleObjects]
    public class ObjectBuilderEditor : Editor
    {
#if UNITY_EDITOR
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        AutoRuleTile myScript = (AutoRuleTile)target;
        if (GUILayout.Button("Build Rule Tile"))
        {
            myScript.OverrideRuleTile();
        }
    }
#endif
    }

}



