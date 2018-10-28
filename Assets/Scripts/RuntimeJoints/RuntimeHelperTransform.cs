using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RuntimeTransformHelper : MonoBehaviour {

    [Header("[Pose Reader]")]
    [Space(5)]
    public YawPitchRoll poseReader;

    [Header("[User Attributes]")]
    [Space(5)]
    public float multiplier = 1;
    public int _reverseDirection = 1;
    public float _minValue = -1;
    public float _maxValue = 1;

    [Header("[Default Attributes]")]
    [Space(5)]
    public int _defaultAxis;
    public float _defaultPositionX;
    public float _defaultPositionY;
    public float _defaultPositionZ;

    private Vector3 defaultPosition;
    private AnimationCurve defaultCurve; 
    private float defaultLength;

	void Start () {
        // construct default values
        defaultPosition = new Vector3(_defaultPositionX, _defaultPositionY, _defaultPositionZ);
        defaultLength = defaultPosition.magnitude;
        defaultCurve = new AnimationCurve(
            new Keyframe(-180, 0), 
            new Keyframe(-170, _minValue * defaultLength * _reverseDirection),
            new Keyframe(-120, _minValue * defaultLength * _reverseDirection),
            new Keyframe(120, _maxValue * defaultLength * _reverseDirection),
            new Keyframe(170, _maxValue * defaultLength * _reverseDirection),
            new Keyframe(180, 0)
        );
    }
	
	void Update () {
        // validate pose reader
        if (poseReader == null)
            return;

        // extract twist value on prefered axis
        Vector3 yawPitchRoll = poseReader.GetYawPitchRoll();

        float twistValue = yawPitchRoll[_defaultAxis];
        float twistOffset = defaultCurve.Evaluate(twistValue) * multiplier;

        // set local position
        transform.localPosition = defaultPosition + new Vector3(0, twistOffset, 0);
    }
}
