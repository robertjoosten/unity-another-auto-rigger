using UnityEngine;
using UnityEditor;
using AnotherAutoRigger;

[CustomEditor(typeof(RuntimeHelperAim))]
public class RuntimeHelperAimEditor : Editor
{ 
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        // populate transforms
        RuntimeHelperAim script = (RuntimeHelperAim)target;
        script.targetTransform = script.GetComponentInGameObjectFromString<Transform>(script.target);
    }
}