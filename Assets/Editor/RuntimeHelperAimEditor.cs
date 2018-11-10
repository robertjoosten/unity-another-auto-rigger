using UnityEngine;
using UnityEditor;
using AnotherAutoRigger;

namespace AnotherAutoRigger
{
    [CustomEditor(typeof(RuntimeHelperAim))]
    public class RuntimeHelperAimEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            // get script
            RuntimeHelperAim script = (RuntimeHelperAim)target;

            // populate transforms
            if (script.targetTransform == null)
                script.targetTransform = script.GetComponentInGameObjectFromString<Transform>(script.target);

            // build settings
            EditorGUILayout.LabelField("Settings", EditorStyles.boldLabel);

            // build target
            script.targetTransform = (Transform)EditorGUILayout.ObjectField(
                "Target",
                script.targetTransform,
                typeof(Transform),
                true
            );

            // build reverse rotation
            bool reverse = (script._blockMultiplier == 1) ? true : false;
            reverse = EditorGUILayout.Toggle("Reverse Direction", reverse);
            script._blockMultiplier = (reverse == true) ? 1 : -1;
        }
    }
}