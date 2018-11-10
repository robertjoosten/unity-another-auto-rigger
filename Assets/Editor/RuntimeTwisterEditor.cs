using UnityEditor;

namespace AnotherAutoRigger
{
    [CustomEditor(typeof(RuntimeTwister))]
    public class RuntimeTwisterEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            // get script
            RuntimeTwister script = (RuntimeTwister)target;

            // build pose reader
            EditorGUILayout.LabelField("Pose Reader", EditorStyles.boldLabel);
            script.poseReader = (YawPitchRoll)EditorGUILayout.ObjectField(
                "Pose Reader",
                script.poseReader,
                typeof(YawPitchRoll),
                true
            );

            // build settings
            EditorGUILayout.LabelField("Settings", EditorStyles.boldLabel);

            // build multiplier
            script.multiplier = EditorGUILayout.Slider("Multiplier", script.multiplier, 0, 10);

            // build reverse twist
            bool reverse = (script._blockMultiplier == 1) ? true : false;
            reverse = EditorGUILayout.Toggle("Reverse Rotation", reverse);
            script._blockMultiplier = (reverse == true) ? 1 : -1;
        }
    }
}