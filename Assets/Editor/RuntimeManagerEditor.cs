using UnityEngine;
using System.Collections;
using UnityEditor;
using AnotherAutoRigger;

[CustomEditor(typeof(RuntimeManager))]
public class ObjectBuilderEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        RuntimeManager script = (RuntimeManager)target;
        if (GUILayout.Button("Apply Runtime Objects"))
        {
            string path = EditorUtility.OpenFilePanel("Load skeleton preset", "", "skeletonPreset");
            script.ApplyRuntimeObjects(path);
        }
    }
}