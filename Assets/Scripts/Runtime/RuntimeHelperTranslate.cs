using UnityEngine;

namespace AnotherAutoRigger
{
    [System.Serializable]
    public class RuntimeHelperTranslate : YawPitchRollSetter
    {
        // settings
        public float multiplier = 1;
        public int _directionMultiplier = 1;
        public int _blockMultiplier = 1;

        // curve extremes
        public float _negValue = -1;
        public float _posValue = 1;

        // offset position
        public float offsetX;
        public float offsetY;
        public float offsetZ;

        // default position
        public int _defaultAxis;
        public float _defaultPositionX;
        public float _defaultPositionY;
        public float _defaultPositionZ;

        // debug
        public override Color DebugColor { get { return Color.yellow; } }

        // private
        private bool isValid;
        private Vector3 defaultPosition;
        private Vector3 startPosition;
        private AnimationCurve defaultCurve;
        private float defaultLength;

        void Awake()
        {
            // store valid state
            isValid = (poseReader == null) ? false : true;
        }

        void Start()
        {
            // only continue when helper is valid
            if (!isValid)
                return;

            // construct default values
            defaultPosition = new Vector3(-_defaultPositionX, _defaultPositionY, _defaultPositionZ);
            defaultLength = defaultPosition.magnitude;
            defaultCurve = new AnimationCurve(
                new Keyframe(180, 0),
                new Keyframe(170, _negValue * _directionMultiplier * _blockMultiplier),
                new Keyframe(120, _negValue * _directionMultiplier * _blockMultiplier),
                new Keyframe(-120, _posValue * _directionMultiplier * _blockMultiplier),
                new Keyframe(-170, _posValue * _directionMultiplier * _blockMultiplier),
                new Keyframe(-180, 0)
            );

            // construct start position
            Vector3 offsetPosition = new Vector3(-offsetX, offsetY, offsetZ) * _blockMultiplier;
            startPosition = defaultPosition + offsetPosition;
        }

        void LateUpdate()
        {
            // only continue when helper is valid
            if (!isValid)
                return;

            // extract twist value on prefered axis
            Vector3 yawPitchRoll = poseReader.GetYawPitchRoll();

            float twistValue = yawPitchRoll[_defaultAxis];
            float twistOffset = defaultCurve.Evaluate(twistValue) * defaultLength * multiplier;

            // set local position
            transform.localPosition = startPosition + new Vector3(0, twistOffset, 0);
        }
    }
}