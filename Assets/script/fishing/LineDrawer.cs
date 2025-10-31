using UnityEngine;

public class LineDrawer : MonoBehaviour
{
    public GameObject startPoint; // Assign in Inspector
    public GameObject endPoint;   // Assign in Inspector
    LineRenderer lineRenderer;

    GameObject stress_ball;
    GameObject stress_ball_red;

    hook_movement hook;

    float alpha_level = 0;

    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        if (lineRenderer == null)
        {
            Debug.LogError("LineRenderer component not found on this GameObject!");
            enabled = false; // Disable script if no LineRenderer
        }

        stress_ball = transform.Find("stress_ball").gameObject;
        stress_ball_red = transform.Find("stress_ball_r").gameObject;
        hook = GetComponent<hook_movement>();
    }

    void Update()
    {
        if (startPoint != null && endPoint != null)
        {
            if (startPoint.transform.position.y > endPoint.transform.position.y)
            {
                lineRenderer.positionCount = 2;
                lineRenderer.SetPosition(0, startPoint.transform.position);
                lineRenderer.SetPosition(1, endPoint.transform.position);
            }
            else
            {
                lineRenderer.positionCount = 0;
            }
                
        }


        ball_location();
        ball_beeping();
    }

    public float get_distance()
    {
        float dist = Vector3.Distance(startPoint.transform.position, endPoint.transform.position);
        return dist;
    }

    public float get_depth()
    {
        float dist = Mathf.Abs(startPoint.transform.position.y - endPoint.transform.position.y);
        return dist;
    }

    public Vector3 get_direction()
    {
        Vector3 direction = startPoint.transform.position - endPoint.transform.position;
        return direction;
    }

    public void ball_beeping()
    {
        if (hook.stress_level <= (hook.get_reelStrength() * 10) / 100)
        {
            alpha_level = 0;
        }
        else if (hook.stress_level >= (hook.get_reelStrength() * 90) / 100)
        {
            alpha_level = 1;
        }
        else
        {
            float hook_strength = (hook.stress_level / hook.get_reelStrength()) * 100;
            alpha_level = ((1.25f * hook_strength) - 12.5f) / 100;
        }

        stress_ball_red.GetComponent<SpriteRenderer>().material.color = new Color(1.0f, 1.0f, 1.0f, alpha_level);
    }

    public void ball_location()
    {
        Vector3 ball_location = new Vector3(endPoint.transform.position.x, endPoint.transform.position.y, stress_ball.transform.position.z);
        Vector3 ball_offset = get_direction().normalized;

        ball_location.x += ball_offset.x;
        ball_location.y += ball_offset.y;

        Vector3 ball_location_r = new Vector3(ball_location.x, ball_location.y, stress_ball_red.transform.position.z);

        stress_ball.transform.position = ball_location;
        stress_ball_red.transform.position = ball_location_r;
    }
}
