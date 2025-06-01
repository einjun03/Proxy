using UnityEngine;
using UnityEngine.AI;

public class PathDrawer : MonoBehaviour
{
    public Transform playerTransform;        // Assign AR Camera here
    public Transform destinationTransform;   // Assign destination point here
    public LineRenderer lineRenderer;        // Assign LineRenderer component

    private NavMeshPath path;
    private float updateRate = 0.2f;

    void Start()
    {
        path = new NavMeshPath();
        InvokeRepeating(nameof(UpdatePath), 0f, updateRate);
    }

    void UpdatePath()
    {
        if (playerTransform == null || destinationTransform == null)
            return;

        if (NavMesh.CalculatePath(playerTransform.position, destinationTransform.position, NavMesh.AllAreas, path))
        {
            lineRenderer.positionCount = path.corners.Length;
            lineRenderer.SetPositions(path.corners);
        }
    }
}
