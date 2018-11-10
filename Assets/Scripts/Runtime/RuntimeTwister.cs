using UnityEngine;

namespace AnotherAutoRigger
{
    [System.Serializable]
    public class RuntimeTwister : YawPitchRollSetter
    {
        // settings
        public float multiplier = 5f;
        public int _blockMultiplier = 1;

        // private
        private bool isValid;

        void Awake()
        {
            // validate
            isValid = (poseReader == null) ? false : true;
        }

        void LateUpdate() {
            // only continue when twister is valid
            if (!isValid)
                return;

            // get twist value
            float rollValue = poseReader.GetYawPitchRoll().y;
            float twistValue = rollValue * multiplier * _blockMultiplier * 0.1f;

            // set twist value
            transform.localRotation = Quaternion.Euler(0, twistValue, 0);
        }
    }
}