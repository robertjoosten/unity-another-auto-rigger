using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

namespace AnotherAutoRigger
{
    [System.Serializable]
    public class RuntimeManager : MonoBehaviour
    {
        public bool debugDisplay = false;

        private string namespacePrefix;
        private string typePrefix = "AnotherAutoRigger.";
        private SimpleJSON.JSONNode runtimeDataDict;
        private List<string> runtimeDataKeys = new List<string>();
        private List<string> runtimeObjectNames = new List<string>
        { 
            "RuntimeTwister",
            "RuntimeHelperTranslate",
            "RuntimeHelperAim",
            "RuntimeMuscle"
        };

        // -------------------------------------------------------------------------

        private SimpleJSON.JSONNode GetRuntimeDataFromFilePath(string filePath)
        {
            // load json data
            string skeletonDataString = File.ReadAllText(filePath);
            var skeletonDataDict = JSON.Parse(skeletonDataString);
            return skeletonDataDict["runtime"];
        }

        // -------------------------------------------------------------------------

        private GameObject GetGameObject(string name)
        {
            // get object
            string path = namespacePrefix + name;
            GameObject obj = GameObject.Find(path);

            // validate object
            if (obj == null)
            {
                Debug.LogWarning(path + " doesn't exist!");
            }

            return obj;
        }

        private object GetRuntimeObject(GameObject runtimeJoint, Type runtimeObjectString)
        {
            // find component
            var existing = runtimeJoint.GetComponent(runtimeObjectString);
            if (existing != null)
                return existing;

            // add component
            return runtimeJoint.AddComponent(runtimeObjectString);
        }

        // -------------------------------------------------------------------------

        private void ProcessRuntimeObjects()
        {
            // loop runtime object variables
            foreach (string runtimeObjectName in runtimeObjectNames)
            {
                // skip if runtime object is not part of data provided
                if (!runtimeDataKeys.Contains(runtimeObjectName))
                    continue;

                // get runtime data
                var runtimeData = runtimeDataDict[runtimeObjectName];

                // get runtime object type
                string runtimeObjectString = typePrefix + runtimeObjectName;
                Type runtimeObjectType = Type.GetType(runtimeObjectString);

                // loop joints
                List<string> runtimeJoints = runtimeDataDict[runtimeObjectName].GetKeys();
                foreach (string runtimeJointName in runtimeJoints)
                {
                    // get joint object
                    GameObject runtimeJoint = GetGameObject(runtimeJointName);

                    // validate joint object
                    if (runtimeJoint == null)
                        continue;

                    // apply runtime object
                    var runtimeObject = GetRuntimeObject(
                        runtimeJoint,
                        runtimeObjectType
                    );

                    // apply runtime data
                    var runtimeObjectData = runtimeData[runtimeJointName].ToString();
                    JsonUtility.FromJsonOverwrite(
                        runtimeObjectData, 
                        runtimeObject
                    );
                }
            }
        }

        private void ProcessYawPitchRollObjects()
        {
            // validate jaw pitch roll 
            if (!runtimeDataKeys.Contains("YawPitchRoll"))
                return;

            // get runtime data
            var yawPitchRollData = runtimeDataDict["YawPitchRoll"];

            // get runtime object type
            string yawPitchRollObjectString = typePrefix + "YawPitchRoll";
            Type yawPitchRollObjectType = Type.GetType(yawPitchRollObjectString);

            // loop joints
            List<string> yawPitchRollJoints = runtimeDataDict["YawPitchRoll"].GetKeys();
            foreach (string yawPitchJointName in yawPitchRollJoints)
            {
                // get joint object
                GameObject yawPitchRollJoint = GetGameObject(yawPitchJointName);

                // validate joint object
                if (yawPitchRollJoint == null)
                    continue;

                // apply runtime object
                var yawPitchRollObject = GetRuntimeObject(
                    yawPitchRollJoint,
                    yawPitchRollObjectType
                ) as YawPitchRoll;

                // apply runtime data
                var yawPitchRollObjectData = yawPitchRollData[yawPitchJointName];
                var yawPitchRollInitializeObjectData = yawPitchRollObjectData["initialize"].ToString();
                JsonUtility.FromJsonOverwrite(
                    yawPitchRollInitializeObjectData,
                    yawPitchRollObject
                );

                // loop connections
                foreach (string connectedJointName in yawPitchRollObjectData["connected"].Values)
                {
                    // get joint object
                    GameObject connectedJoint = GetGameObject(connectedJointName);

                    // validate joint object
                    if (connectedJoint == null)
                        continue;

                    // loop runtime object names
                    foreach (string runtimeObjectName in runtimeObjectNames)
                    {
                        // get runtime object type
                        string runtimeObjectString = typePrefix + runtimeObjectName;
                        Type runtimeObjectType = Type.GetType(runtimeObjectString);

                        // get runtime object on joint
                        var runtimeObject = connectedJoint.GetComponent(runtimeObjectType) as YawPitchRollSetter;
                        if (runtimeObject == null)
                            continue;

                        // set pose reader
                        runtimeObject.poseReader = yawPitchRollObject;
                    }
                }
            }
        }

        // -------------------------------------------------------------------------

        public void BuildRuntimeSkeleton(string filePath)
        {
            // get runtime data
            runtimeDataDict = GetRuntimeDataFromFilePath(filePath);
            runtimeDataKeys = runtimeDataDict.GetKeys();

            // get namespace
            namespacePrefix = gameObject.GetMayaStyleNamespace();

            // process runtime objects
            ProcessRuntimeObjects();

            // process yaw pitch roll objects
            ProcessYawPitchRollObjects();

            // log completion
            Debug.Log("Skeleton preset succesfully applied");
        }

        // -------------------------------------------------------------------------

        public void SetDebugState()
        {
            // loop runtime object variables
            foreach (string runtimeObjectName in runtimeObjectNames)
            {
                // get runtime object type
                string runtimeObjectString = typePrefix + runtimeObjectName;
                Type runtimeObjectType = Type.GetType(runtimeObjectString);

                // get component in children
                foreach (DebugSetter component in gameObject.GetComponentsInChildren(runtimeObjectType))
                {
                    component.debugDisplay = debugDisplay;
                }
            }
        }
    }
}
