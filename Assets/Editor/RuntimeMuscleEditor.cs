using UnityEngine;
using UnityEditor;
using System;

namespace AnotherAutoRigger
{
    [CustomEditor(typeof(RuntimeMuscle))]
    public class RuntimeMuscleEditor : Editor
    {
        // private foldout attributes
        private bool showDynamics = true;
        private bool showOrigin = false;
        private bool showInsertion = false;

        // private vector attributes
        private Vector3 originPosition;
        private Vector3 originPositionTangent;
        private Vector3 originUp;
        private Vector3 insertionPosition;
        private Vector3 insertionPositionTangent;
        private Vector3 insertionUp;

        public override void OnInspectorGUI()
        {
            // get script
            RuntimeMuscle script = (RuntimeMuscle)target;

            // populate transforms
            if (script.originTransform == null)
                script.originTransform = script.GetComponentInGameObjectFromString<Transform>(script.origin);
            if (script.insertionTransform == null)
                script.insertionTransform = script.GetComponentInGameObjectFromString<Transform>(script.insertion);

            // build inspector
            GUIStyle headerStyle = new GUIStyle(EditorStyles.foldout);
            headerStyle.fontStyle = FontStyle.Bold;

            // build settings
            EditorGUILayout.LabelField("Settings", EditorStyles.boldLabel);
            script.multiplier = EditorGUILayout.Slider("Multiplier", script.multiplier, 0, 10);
            script.slide = EditorGUILayout.Slider("Slide On Curve", script.slide, -10, 10);
            script.blend = EditorGUILayout.Slider("Blend Up Vector", script.blend, 0, 10);

            // build dynamics
            showDynamics = EditorGUILayout.Foldout(showDynamics, "Dynamics", headerStyle);
            if (showDynamics)
            {
                script.enableDynamics = EditorGUILayout.Toggle("Enable Dynamics", script.enableDynamics);
                script.stiffness = EditorGUILayout.FloatField("Stiffness", script.stiffness);
                script.mass = EditorGUILayout.FloatField("Mass", script.mass);
                script.damping = EditorGUILayout.FloatField("Damping", script.damping);
                script.gravity = EditorGUILayout.FloatField("Gravity", script.gravity);
            }   

            // build origin
            showOrigin = EditorGUILayout.Foldout(showOrigin, "Origin", headerStyle);
            if (showOrigin)
            {
                // build origin transform
                script.originTransform = (Transform)EditorGUILayout.ObjectField(
                    "Origin", 
                    script.originTransform, 
                    typeof(Transform), 
                    true
                );

                // build origin position
                originPosition.Set(script._originX, script._originY, script._originZ);
                originPosition = EditorGUILayout.Vector3Field("Origin Position", originPosition);
                script._originX = originPosition.x;
                script._originY = originPosition.y;
                script._originZ = originPosition.z;

                // build origin tangent position
                originPositionTangent.Set(script._originTangentX, script._originTangentY, script._originTangentZ);
                originPositionTangent = EditorGUILayout.Vector3Field("Origin Position ( Tangent )", originPositionTangent);
                script._originTangentX = originPositionTangent.x;
                script._originTangentY = originPositionTangent.y;
                script._originTangentZ = originPositionTangent.z;

                // build origin up
                originUp.Set(script._originUpX, script._originUpY, script._originUpZ);
                originUp = EditorGUILayout.Vector3Field("Origin Up", originUp);
                script._originUpX = originUp.x;
                script._originUpY = originUp.y;
                script._originUpZ = originUp.z;
            }

            // build insertion
            showInsertion = EditorGUILayout.Foldout(showInsertion, "Insertion", headerStyle);
            if (showInsertion)
            {
                // build insertion transform
                script.insertionTransform = (Transform)EditorGUILayout.ObjectField(
                    "Insertion", 
                    script.insertionTransform, 
                    typeof(Transform), 
                    true
                );

                // build origin position
                insertionPosition.Set(script._insertionX, script._insertionY, script._insertionZ);
                insertionPosition = EditorGUILayout.Vector3Field("Insertion Position", insertionPosition);
                script._insertionX = insertionPosition.x;
                script._insertionY = insertionPosition.y;
                script._insertionZ = insertionPosition.z;

                // build origin tangent position
                insertionPositionTangent.Set(script._insertionTangentX, script._insertionTangentY, script._insertionTangentZ);
                insertionPositionTangent = EditorGUILayout.Vector3Field("Insertion Position ( Tangent )", insertionPositionTangent);
                script._insertionTangentX = insertionPositionTangent.x;
                script._insertionTangentY = insertionPositionTangent.y;
                script._insertionTangentZ = insertionPositionTangent.z;

                // build origin up
                insertionUp.Set(script._insertionUpX, script._insertionUpY, script._insertionUpZ);
                insertionUp = EditorGUILayout.Vector3Field("Insertion Up", insertionUp);
                script._insertionUpX = originUp.x;
                script._insertionUpY = originUp.y;
                script._insertionUpZ = originUp.z;
            }
        }
    }
}