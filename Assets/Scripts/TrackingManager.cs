using UnityEngine;
using UnityEngine.XR;

public class DisablePhysics : MonoBehaviour
{
    void Start()
    {
        Physics.gravity = Vector3.zero;

        //(hopefully) stops head position from affecting camera position
        if (Camera.main != null)
        {
            XRDevice.DisableAutoXRCameraTracking(Camera.main, true);
            Debug.Log("✅ XR tracking disabled for main camera.");
        }
        else
        {
           Debug.LogWarning("⚠️ Camera.main not found!");
        }
    }
}
