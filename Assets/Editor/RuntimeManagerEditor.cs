using UnityEngine;
using UnityEditor;

namespace AnotherAutoRigger
{
    [CustomEditor(typeof(RuntimeManager))]
    public class ObjectBuilderEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            // get script
            RuntimeManager script = (RuntimeManager)target;

            // create button
            if (GUILayout.Button("Build Runtime Skeleton"))
            {
                string filePath = EditorUtility.OpenFilePanel("Load skeleton preset", "", "skeletonPreset");
                script.BuildRuntimeSkeleton(filePath);
            }
        }
    }
}