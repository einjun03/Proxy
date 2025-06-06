using UnityEngine;
using System.Net.Sockets;
using System.Net;
using System.Text;
using UnityEngine.XR;
using System.Collections.Generic;

public class TestHeadRotation : MonoBehaviour
{
    [Header("Networking")]
    public string raspberryPiIP = "192.168.0.7";
    public int headPort = 8888;
    public int movePort = 8890;

    //private Quaternion initialRotation;

    private UdpClient headClient;
    private UdpClient moveClient;

    void Start()
    {
        headClient = new UdpClient();
        moveClient = new UdpClient();

        //InputDevice head = InputDevices.GetDeviceAtXRNode(XRNode.Head);
        //if (head.TryGetFeatureValue(CommonUsages.deviceRotation, out initialRotation))
        //{
        //Debug.Log("Initial head rotation captured.");
        //}
        // else
        //{
        //Debug.LogWarning("Could not get initial head rotation.");
        //}
    }

    void Update()
    {
        InputDevice head = InputDevices.GetDeviceAtXRNode(XRNode.Head);
        if (head.TryGetFeatureValue(CommonUsages.deviceRotation, out Quaternion currentRot))
        {
            Quaternion relativeRotation = currentRot; //Quaternion.Inverse(initialRotation) * currentRot;
            Vector3 euler = relativeRotation.eulerAngles;

            float pitch = -NormalizeAngle(euler.x); // Up/down
            //mirror poses
            float yaw = -NormalizeAngle(euler.y);   // Left/right

            Debug.Log($"Pitch: {pitch:F2}°, Yaw: {yaw:F2}°");
            

            string movementMsg = $"{pitch},{yaw}";
            byte[] moveData = Encoding.UTF8.GetBytes(movementMsg);
            headClient.Send(moveData, moveData.Length, raspberryPiIP, headPort);
        }

        InputDevice lhand = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
        if (lhand.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 axis))
        {
            float threshold = 0.00f;

            if (Mathf.Abs(axis.x) > threshold || Mathf.Abs(axis.y) > threshold)
            {
                Debug.Log($"x axis: {axis.x:F2}, y axis: {axis.y:F2}");
            }

            //invert since the image (displayed) is mirrored, forward and backward are the same but left right are not
            byte xMapped = MapThumbstick(axis.x);
            byte yMapped = MapThumbstick(axis.y);

            byte[] moveData = new byte[2] {xMapped, yMapped};

            moveClient.Send(moveData, moveData.Length, raspberryPiIP, movePort);
        }
        else
        {
            Debug.LogWarning("Left hand device is not valid!");
        }
    }

    float NormalizeAngle(float angle)
    {
        // Converts angles from [0, 360] to [-180, 180]
        if (angle > 180) angle -= 360;
        return Mathf.Clamp(angle, -70f, 70f);
    }

    byte MapThumbstick(float value)
    {
        // Clamp value
        value = Mathf.Clamp(value, -1f, 1f);
        // Normalize from [-1,1] to [0,254]
        return (byte)((value + 1f) * 127f);
    }

}
