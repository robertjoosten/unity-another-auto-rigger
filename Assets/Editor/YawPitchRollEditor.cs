using UnityEngine;
using UnityEditor;
using AnotherAutoRigger;

[CustomEditor(typeof(YawPitchRoll))]
public class YawPitchRollEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        // get script
        YawPitchRoll script = (YawPitchRoll)target;

        // populate transforms
        if (script.originTransform == null)
            script.originTransform = script.GetComponentInGameObjectFromString<Transform>(script.origin);
        if (script.insertionTransform == null)
            script.insertionTransform = script.GetComponentInGameObjectFromString<Transform>(script.insertion);
    }
}