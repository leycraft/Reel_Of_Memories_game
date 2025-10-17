using UnityEngine;

public class LineDrawer : MonoBehaviour
{
    public GameObject startPoint; // Assign in Inspector
    public GameObject endPoint;   // Assign in Inspector
    private LineRenderer lineRenderer;

    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        if (lineRenderer == null)
        {
            Debug.LogError("LineRenderer component not found on this GameObject!");
            enabled = false; // Disable script if no LineRenderer
        }
    }

    void Update()
    {
        if (startPoint != null && endPoint != null)
        {
            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0, startPoint.transform.position);
            lineRenderer.SetPosition(1, endPoint.transform.position);
        }
    }

    public float get_distance()
    {
        float dist = Vector3.Distance(startPoint.transform.position, endPoint.transform.position);
        return dist;
    }

    public Vector3 get_direction()
    {
        Vector3 direction = startPoint.transform.position - endPoint.transform.position;
        return direction;
    }
}
