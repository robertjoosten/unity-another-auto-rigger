using UnityEngine;
using UnityEditor;
using AnotherAutoRigger;

[CustomEditor(typeof(YawPitchRoll))]
public class YawPitchRollEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        // populate transforms
        YawPitchRoll script = (YawPitchRoll)target;
        script.originTransform = script.GetComponentInGameObjectFromString<Transform>(script.origin);
        script.insertionTransform = script.GetComponentInGameObjectFromString<Transform>(script.insertion);
    }
}