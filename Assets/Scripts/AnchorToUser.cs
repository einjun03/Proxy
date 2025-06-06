using UnityEngine;

public class AnchorToUser : MonoBehaviour
{
    public Vector3 offset = new Vector3(0, -0.5f, 2f); // In front of the user

    void Update()
    {
        if (Camera.main != null)
        {
            transform.position = Camera.main.transform.position + Camera.main.transform.rotation * offset;
            transform.rotation = Quaternion.LookRotation(Camera.main.transform.forward);
        }
    }
}
