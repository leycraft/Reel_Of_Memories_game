using TMPro;
using UnityEngine;

public class depth_number : MonoBehaviour
{
    LineDrawer hook_line;

    TextMeshProUGUI depth;
    TextMeshProUGUI length;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        hook_line = GameObject.Find("hook_obj").GetComponent<LineDrawer>();

        depth = transform.Find("depth_num").gameObject.GetComponent<TextMeshProUGUI>();
        length = transform.Find("length_num").gameObject.GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        float dep = hook_line.get_depth();
        float len = hook_line.get_distance();

        depth.SetText($"{(dep * 0.5) - 2.3:F1}m");
        length.SetText($"{len * 0.5:F1}/?? m");
    }
}
