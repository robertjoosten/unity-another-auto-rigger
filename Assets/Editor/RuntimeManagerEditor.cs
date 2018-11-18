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

            // set debug mode, the debug mode will draw spheres on each of 
            // the joint that are driven at runtime.
            script.debugDisplay = EditorGUILayout.ToggleLeft("Display Debug Gizmos", script.debugDisplay);
            script.SetDebugState();

            // redraw scene
            SceneView.RepaintAll();
        }
    }
}