using UnityEngine;

public class SyncOVRToVuforia : MonoBehaviour
{
    public Transform vuforiaCamera;    // Drag your Vuforia ARCamera here
    public Transform ovrCameraRig;     // Drag your OVRCameraRig here

    private Vector3 initialPositionOffset;
    private Quaternion initialRotationOffset;

    void Start()
    {
        // Compute the initial relative offset at startup
        initialPositionOffset = ovrCameraRig.position - vuforiaCamera.position;
        initialRotationOffset = Quaternion.Inverse(vuforiaCamera.rotation) * ovrCameraRig.rotation;
    }

    void LateUpdate()
    {
        // Maintain the initial relative position and rotation
        ovrCameraRig.position = vuforiaCamera.position + vuforiaCamera.rotation * initialPositionOffset;
        ovrCameraRig.rotation = vuforiaCamera.rotation * initialRotationOffset;
    }
}
