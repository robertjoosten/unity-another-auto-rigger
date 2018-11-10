using UnityEngine;
using UnityEditor;

namespace AnotherAutoRigger
{
    [CustomEditor(typeof(YawPitchRoll))]
    public class YawPitchRollEditor : Editor
    {
        private Vector3 parentOffset;

        public override void OnInspectorGUI()
        {
            // get script
            YawPitchRoll script = (YawPitchRoll)target;

            // populate transforms
            if (script.originTransform == null)
                script.originTransform = script.GetComponentInGameObjectFromString<Transform>(script.origin);
            if (script.insertionTransform == null)
                script.insertionTransform = script.GetComponentInGameObjectFromString<Transform>(script.insertion);

            // build settings
            EditorGUILayout.LabelField("Settings", EditorStyles.boldLabel);

            // build mapping
            script.mapping = (YawPitchRoll.MappingOptions)EditorGUILayout.EnumPopup("Mapping", script.mapping);

            // build parent euler offset
            parentOffset.Set(script._parentOffsetX, script._parentOffsetY, script._parentOffsetZ);
            parentOffset = EditorGUILayout.Vector3Field("Origin Euler Offset", parentOffset);
            script._parentOffsetX = parentOffset.x;
            script._parentOffsetY = parentOffset.y;
            script._parentOffsetZ = parentOffset.z;

            // build transforms
            EditorGUILayout.LabelField("Transforms", EditorStyles.boldLabel);
            script.originTransform = (Transform)EditorGUILayout.ObjectField(
                "Origin",
                script.originTransform,
                typeof(Transform),
                true
            );
            script.insertionTransform = (Transform)EditorGUILayout.ObjectField(
                "Insertion",
                script.insertionTransform,
                typeof(Transform),
                true
            );
        }
    }
}