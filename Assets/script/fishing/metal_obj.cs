using UnityEngine;

public class metal_obj : MonoBehaviour
{
    hook_movement hook;
    Vector3 original_pos;

    float dist;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        hook = GameObject.Find("hook_obj").GetComponent<hook_movement>();
        original_pos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (hook.magnet_on)
        {
            dist = Vector3.Distance(hook.get_hookLocation().position, transform.position);
            get_hooked();
        }

    }

    void get_hooked()
    {
        if (dist <= 0.8 && hook.item_caught == null)
        {
            hook.box_on_hook(this);
        }
    }

    public void reset_pos()
    {
        transform.position = original_pos;
    }
}
