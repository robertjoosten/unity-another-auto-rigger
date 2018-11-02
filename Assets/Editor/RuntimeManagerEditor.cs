using UnityEngine;
using UnityEditor;
using AnotherAutoRigger;

[CustomEditor(typeof(RuntimeManager))]
public class ObjectBuilderEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        RuntimeManager script = (RuntimeManager)target;
        if (GUILayout.Button("Build Runtime Skeleton"))
        {
            string filePath = EditorUtility.OpenFilePanel("Load skeleton preset", "", "skeletonPreset");
            script.BuildRuntimeSkeleton(filePath);
        }
    }
}