using UnityEngine;
using UnityEditor;
using AnotherAutoRigger;

[CustomEditor(typeof(RuntimeManager))]
public class ObjectBuilderEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

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