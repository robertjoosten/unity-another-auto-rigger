using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AnotherAutoRigger;

namespace AnotherAutoRigger
{
    [System.Serializable]
    [ExecuteInEditMode]
    public class RuntimeMuscle : MonoBehaviour
    {
        [HideInInspector] public string origin;
        [HideInInspector] public string insertion;
        [Header("[Transforms]")]
        [Space(5)]
        public Transform originTransform;
        public Transform insertionTransform;

        [Header("[Multiplier Attributes]")]
        [Space(5)]
        public float multiplier = 5f;
        public float blend = 5f;
        public float slide = 0f;

        [Header("[Origin Attributes]")]
        [Space(5)]
        public float _originX;
        public float _originY;
        public float _originZ;

        [Header("[Origin Tangent Attributes]")]
        [Space(5)]
        public float _originTangentX;
        public float _originTangentY;
        public float _originTangentZ;

        [Header("[Origin Up Attributes]")]
        [Space(5)]
        public float _originUpX;
        public float _originUpY;
        public float _originUpZ;

        [Header("[Insertion Attributes]")]
        [Space(5)]
        public float _insertionX;
        public float _insertionY;
        public float _insertionZ;

        [Header("[Insertion Tangent Attributes]")]
        [Space(5)]
        public float _insertionTangentX;
        public float _insertionTangentY;
        public float _insertionTangentZ;

        [Header("[Insertion Up Attributes]")]
        [Space(5)]
        public float _insertionUpX;
        public float _insertionUpY;
        public float _insertionUpZ;

        private bool isValid;
        private float defaultLength;
 
        private Vector3 localOrigin;
        private Vector3 localOriginTangent;
        private Vector3 localOriginUp;
        private Vector3 localInsertion;
        private Vector3 localInsertionTangent;
        private Vector3 localInsertionUp;

        private Vector3 worldOrigin;
        private Vector3 worldOriginTangent;
        private Vector3 worldOriginUp;
        private Vector3 worldInsertion;
        private Vector3 worldInsertionTangent;
        private Vector3 worldInsertionUp;

        private Vector3 localOriginTangentVector;
        private Vector3 localInsertionTangentVector;
        private Quaternion localOriginTangentOffset;
        private Quaternion localInsertionTangentOffset;

        private void InitializeLocalVectors()
        {
            // set local positions
            localOrigin = new Vector3(-_originX, _originY, _originZ);
            localInsertion = new Vector3(-_insertionX, _insertionY, _insertionZ);
            localOriginTangent = new Vector3(-_originTangentX, _originTangentY, _originTangentZ);
            localInsertionTangent = new Vector3(-_insertionTangentX, _insertionTangentY, _insertionTangentZ);

            // set local directions
            localOriginUp = new Vector3(-_originUpX, _originUpY, _originUpZ);
            localInsertionUp = new Vector3(-_insertionUpX, _insertionUpY, _insertionUpZ);
        }

        // -------------------------------------------------------------------------

        private void SetWorldCurvePositions()
        {
            // set world positions
            worldOrigin = originTransform.TransformPoint(localOrigin);
            worldOriginTangent = originTransform.TransformPoint(localOriginTangent);
            worldInsertion = insertionTransform.TransformPoint(localInsertion);
            worldInsertionTangent = insertionTransform.TransformPoint(localInsertionTangent);
        }

        private void SetWorldCurveDirections()
        {
            // set world positions
            worldOriginUp = originTransform.TransformVector(localOriginUp);
            worldInsertionUp = insertionTransform.TransformVector(localInsertionUp);
        }

        // -------------------------------------------------------------------------

        private Vector3 CalculateTangentVector(Vector3 source, Vector3 target)
        {
            // get the length between the source and the target and create a vector
            // with on Z axis translation using that length.
            Vector3 offset = target - source;
            return new Vector3(0, 0, offset.magnitude);
        }

        private Quaternion CalculateTangentRotationOffset(
            Vector3 source, Vector3 target1, Vector3 target2, Vector3 up)
        {
            // calculate the initial rotation offset between the provided source
            // and both targets.
            Vector3 targetOffset1 = target1 - source;
            Vector3 targetOffset2 = target2 - source;

            Quaternion targetRotation1 = Quaternion.LookRotation(targetOffset1, up);
            Quaternion targetRotation2 = Quaternion.LookRotation(targetOffset2, up);

            return Quaternion.Inverse(targetRotation1) * targetRotation2;
        }

        private Vector3 CalculateTangentPosition(
            Vector3 source, Vector3 target, Vector3 up, 
            Vector3 posOffset, Quaternion rotationOffset, 
            float scale, float rotation
        )
        {
            // get all rotations
            Quaternion rotationSource = Quaternion.LookRotation(target - source, up);
            Quaternion rotationStretch = Quaternion.Euler(rotation, 0, 0);
            Quaternion rotationAdded = rotationSource * rotationOffset * rotationStretch;

            // create matrix
            Matrix4x4 rotationMatrix = Matrix4x4.Rotate(rotationAdded);

            // calculate position
            Vector3 posOffsetScaled = posOffset * scale;
            return rotationMatrix.MultiplyPoint(posOffsetScaled) + source;
        }

        // -------------------------------------------------------------------------

        private float GetLength()
        {
            // get length between end points
            return (worldOrigin - worldInsertion).magnitude;
        }

        private float GetSlideValue()
        {
            // the slide value coming from maya is between -10 and 10, for unity
            // this value needs to be remapped between 0 and 1.
            return Mathf.Clamp01((slide * 0.05f) + 0.5f);
        }

        // -------------------------------------------------------------------------

        private Vector3 GetTransformPosition()
        {
            // get bezier point
            float t = GetSlideValue();
            float oneMinusT = 1f - t;
            return
                oneMinusT * oneMinusT * oneMinusT * worldOrigin +
                3f * oneMinusT * oneMinusT * t * worldOriginTangent +
                3f * oneMinusT * t * t * worldInsertionTangent +
                t * t * t * worldInsertion;
        }

        public Vector3 GetTransformForwardVector()
        {
            // get bezier forward vector
            float t = GetSlideValue();
            float oneMinusT = 1f - t;
            return
                3f * oneMinusT * oneMinusT * (worldOriginTangent - worldOrigin) +
                6f * oneMinusT * t * (worldInsertionTangent - worldOriginTangent) +
                3f * t * t * (worldInsertion - worldInsertionTangent);
        }

        // -------------------------------------------------------------------------

        public bool ValidateMuscle()
        {
            // validate settings, if the tangents or the ups are set to zero means
            // that the muscle position cannot be calculated. It also check if the
            // transforms needed are applied.
            if (localOriginTangent == Vector3.zero || localInsertionTangent == Vector3.zero ||
                    localOriginUp == Vector3.zero || localInsertionUp == Vector3.zero ||
                    originTransform == null || insertionTransform == null)
                return false;
            return true;
        }

        // -------------------------------------------------------------------------

        void Awake()
        {
            // populate transforms
            if (originTransform == null)
                originTransform = this.GetComponentInGameObjectFromString<Transform>(origin);
            if (insertionTransform == null)
                insertionTransform = this.GetComponentInGameObjectFromString<Transform>(insertion);

            // initial local positions and directions
            InitializeLocalVectors();

            // validate settings
            isValid = ValidateMuscle();
        }

        void Start()
        {
            // only continue when muscle is valid
            if (!isValid)
                return;

            // set world positions and directions
            SetWorldCurvePositions();
            SetWorldCurveDirections();

            // get length
            defaultLength = GetLength();

            // get tangent vectors
            localOriginTangentVector = CalculateTangentVector(worldOrigin, worldOriginTangent);
            localInsertionTangentVector = CalculateTangentVector(worldInsertion, worldInsertionTangent);

            // get tangent offset rotation
            localOriginTangentOffset = CalculateTangentRotationOffset(
                worldOrigin, 
                worldInsertion,
                worldOriginTangent, 
                worldOriginUp
            );
            localInsertionTangentOffset = CalculateTangentRotationOffset(
                worldInsertion,
                worldOrigin,
                worldInsertionTangent,
                worldInsertionUp
            );
        }

        void LateUpdate()
        {
            // only continue when muscle is valid
            if (!isValid)
                return;

            // set world positions and directions
            SetWorldCurvePositions();
            SetWorldCurveDirections();

            // get stretch factor
            float length = GetLength();
            float factor = (length / defaultLength) - 1;

            // get stretch scale factor
            float stretchScale = (factor * 0.25f) + 1;

            // get stretch rotation factor
            float stretchRotation = factor * multiplier;
            float stretchRotationClamped = (stretchRotation < -75) ? -75 : (stretchRotation > 0) ? 0 : stretchRotation;

            // get tangent positions
            worldOriginTangent = CalculateTangentPosition(
                worldOrigin,
                worldInsertion,
                worldOriginUp,
                localOriginTangentVector,
                localOriginTangentOffset,
                stretchScale,
                stretchRotationClamped
            );
            worldInsertionTangent = CalculateTangentPosition(
                worldInsertion,
                worldOrigin,
                worldInsertionUp,
                localInsertionTangentVector,
                localInsertionTangentOffset,
                stretchScale,
                stretchRotationClamped
            );

            // get joint position
            Vector3 jointWorldPos = GetTransformPosition();

            // get joint rotation
            Vector3 jointWorldForward = GetTransformForwardVector();
            Vector3 jointWorldUp = Vector3.Lerp(worldOriginUp, worldInsertionUp, blend * 0.1f);

            // set joint values
            transform.position = jointWorldPos;
            transform.rotation = Quaternion.LookRotation(jointWorldForward, jointWorldUp);
        }
    }
}