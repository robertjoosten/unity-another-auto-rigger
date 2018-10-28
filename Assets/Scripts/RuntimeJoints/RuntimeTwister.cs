using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RuntimeTwister : MonoBehaviour {

    [Header("[Pose Reader]")]
    [Space(5)]
    public YawPitchRoll poseReader;

    [Header("[User Attributes]")]
    [Space(5)]
    public float multiplier = 0.5f;
	
    void Update() {
        // validate pose reader
        if (poseReader == null)
            return;

        // get twist value
        float rollValue = poseReader.GetYawPitchRoll().y;
        float twistValue = rollValue * multiplier;

        // set twist value
        transform.localRotation = Quaternion.Euler(0, twistValue, 0);
    }
}
