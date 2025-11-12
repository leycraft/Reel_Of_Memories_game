using UnityEngine;

public class toggle_ui_button : MonoBehaviour
{
    public GameObject to_toggle;
    public bool start_visible = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        to_toggle.SetActive(start_visible);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void toggle_visibility()
    {
        start_visible = !start_visible;
        to_toggle.SetActive(start_visible);
    }
}
