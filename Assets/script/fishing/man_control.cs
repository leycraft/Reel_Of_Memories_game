using UnityEngine;

public class man_control : MonoBehaviour
{ 
    public bool is_idle = true;
    public ui_switch game_ui;

    cam_control camera_main;
    hook_movement hook;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        camera_main = GameObject.Find("Main Camera").GetComponent<cam_control>();
        hook = GameObject.Find("hook_obj").GetComponent<hook_movement>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void start_fishing()
    {
        is_idle = false;
        hook.start_fishing();
        camera_main.set_camera(GameObject.Find("hook_obj"));
        game_ui.change_UI(1);
    }

    public void stop_fishing()
    {
        is_idle = true;
        hook.stop_fishing();
        camera_main.set_camera(GameObject.Find("man_obj"));
        game_ui.change_UI(0);
    }
}
