using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AnotherAutoRigger;

namespace AnotherAutoRigger
{
    [System.Serializable]
    [ExecuteInEditMode]
    public class RuntimeHelperAim : MonoBehaviour
    {
        [HideInInspector]public string target;

        [Header("[Transforms]")]
        [Space(5)]
        public Transform targetTransform;

        [Header("[Multiplier Attributes]")]
        [Space(5)]
        public int _blockMultiplier = 1;

        private Vector3 xDirection = new Vector3(1, 0, 0);

        void Awake()
        {
            targetTransform = this.GetComponentInGameObjectFromString<Transform>(target);
        }

        void LateUpdate()
        {
            // validate target
            if (targetTransform == null)
                return;

            // get parent x vector
            Transform parentTransform = transform.parent;
            Matrix4x4 parentMatrix = Matrix4x4.TRS(parentTransform.position, parentTransform.rotation, parentTransform.localScale);
            Vector3 xVectorParent = parentMatrix.MultiplyVector(xDirection);

            // get y vector
            Vector3 yVector = (targetTransform.position - transform.position).normalized * _blockMultiplier;

            // get z vector
            Vector3 zVector = Vector3.Cross(xVectorParent, yVector);

            // set rotation
            transform.rotation = Quaternion.LookRotation(zVector, yVector);
        }
    }
}
