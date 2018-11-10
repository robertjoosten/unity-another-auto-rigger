using UnityEngine;
using UnityEditor;
using AnotherAutoRigger;

namespace AnotherAutoRigger
{
    [CustomEditor(typeof(RuntimeHelperTranslate))]
    public class RuntimeHelperTranslateEditor : Editor
    {
        private enum AxisOptions { X=0, Y=1, Z=2 };
        private Vector2 translationExtremes;
        private Vector3 defaultPosition;
        private Vector3 offsetPosition;

        public override void OnInspectorGUI()
        {
            // get script
            RuntimeHelperTranslate script = (RuntimeHelperTranslate)target;

            // build pose reader
            EditorGUILayout.LabelField("Pose Reader", EditorStyles.boldLabel);
            script.poseReader = (YawPitchRoll)EditorGUILayout.ObjectField(
                "Pose Reader",
                script.poseReader,
                typeof(YawPitchRoll),
                true
            );

            // build pose reader axis
            AxisOptions axis = (AxisOptions)script._defaultAxis;
            script._defaultAxis = (int)(AxisOptions)EditorGUILayout.EnumPopup("Axis", axis);

            // build settings
            EditorGUILayout.LabelField("Settings", EditorStyles.boldLabel);

            // build multiplier
            script.multiplier = EditorGUILayout.FloatField("Multiplier", script.multiplier);

            // build reverse direction
            bool reverseDirection = (script._directionMultiplier == 1) ? true : false;
            reverseDirection = EditorGUILayout.Toggle("Reverse Direction", reverseDirection);
            script._directionMultiplier = (reverseDirection == true) ? 1 : -1;

            // build reverse offset
            bool reverseOffset = (script._blockMultiplier == 1) ? true : false;
            reverseOffset = EditorGUILayout.Toggle("Reverse Offset", reverseOffset);
            script._blockMultiplier = (reverseOffset == true) ? 1 : -1;

            // build curve
            EditorGUILayout.LabelField("Curve", EditorStyles.boldLabel);

            // build extremes
            translationExtremes.Set(script._negValue, script._posValue);
            translationExtremes = EditorGUILayout.Vector2Field("Translation Extremes", translationExtremes);
            script._negValue = translationExtremes.x;
            script._posValue = translationExtremes.y;

            // build placement
            EditorGUILayout.LabelField("Placement", EditorStyles.boldLabel);

            // build default position
            defaultPosition.Set(script._defaultPositionX, script._defaultPositionY, script._defaultPositionZ);
            defaultPosition = EditorGUILayout.Vector3Field("Default Position", defaultPosition);
            script._defaultPositionX = defaultPosition.x;
            script._defaultPositionY = defaultPosition.y;
            script._defaultPositionZ = defaultPosition.z;

            // build offset position
            offsetPosition.Set(script.offsetX, script.offsetY, script.offsetZ);
            offsetPosition = EditorGUILayout.Vector3Field("Default Position", offsetPosition);
            script.offsetX = offsetPosition.x;
            script.offsetY = offsetPosition.y;
            script.offsetZ = offsetPosition.z;
        }
    }
}