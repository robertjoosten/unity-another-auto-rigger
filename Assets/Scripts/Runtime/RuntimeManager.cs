using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AnotherAutoRigger;

namespace AnotherAutoRigger
{
    [System.Serializable]
    public class RuntimeManager : MonoBehaviour
    {
        private Component runtimeObject = new Component();

        public void ApplyRuntimeObjects(string path)
        {
            // load json data


            // loop joints
            foreach (var joint in GetComponentsInChildren<Transform>())
            {
                // get runtime type
                int runtimeType = GetRuntimeType(joint.name);
                
                // add runtime component
                switch (runtimeType)
                {
                    case 0:
                        // no runtime match found, skip
                        continue;
                    case 1:
                        runtimeObject = GetRuntimeObject<RuntimeTwister>(joint.gameObject);
                        break;
                    case 2:
                        runtimeObject = GetRuntimeObject<RuntimeHelperTranslate>(joint.gameObject);
                        break;
                    case 3:
                        runtimeObject = GetRuntimeObject<RuntimeHelperAim>(joint.gameObject);
                        break;
                }

            }
        }

        private int GetRuntimeType(String name)
        {
            // split name into list
            String[] elements = name.Split('_');
   
            // validate name list
            if (elements[elements.Length - 1].IsDigit() && elements[elements.Length - 2] == "jnt")
            {
                if (elements[elements.Length - 3] == "twister")
                    return 1;
                else if (elements[elements.Length - 3] == "helper")
                    return 2;
                else if (elements[elements.Length - 3] == "aim")
                    return 3;
            }
            return 0;
        }

        private T GetRuntimeObject<T>(GameObject joint) where T : Component
        {
            // see if component exists
            T existing = joint.GetComponent<T>();
            if (existing != null)
                return existing;

            // add component
            return joint.AddComponent<T>();
        }
    }
}
