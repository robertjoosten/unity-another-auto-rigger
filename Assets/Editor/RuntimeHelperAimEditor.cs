using UnityEngine;
using UnityEditor;
using AnotherAutoRigger;

[CustomEditor(typeof(RuntimeHelperAim))]
public class RuntimeHelperAimEditor : Editor
{ 
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        // get script
        RuntimeHelperAim script = (RuntimeHelperAim)target;

        // populate transforms
        if (script.targetTransform == null)
            script.targetTransform = script.GetComponentInGameObjectFromString<Transform>(script.target);
    }
}