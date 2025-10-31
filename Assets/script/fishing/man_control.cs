using UnityEngine;
using System.Collections;

public class man_control : MonoBehaviour
{ 
    public bool is_idle = true;
    public ui_switch game_ui;

    cam_control camera_main;
    hook_movement hook;

    Animator m_Animator;

    AudioManager audio_manager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        camera_main = GameObject.Find("Main Camera").GetComponent<cam_control>();
        hook = GameObject.Find("hook_obj").GetComponent<hook_movement>();
        m_Animator = GameObject.Find("PlayerRigged").GetComponent<Animator>();
        audio_manager = GameObject.Find("SFX_player").GetComponent<AudioManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void start_fishing()
    {
        if (is_idle)
        {
            m_Animator.SetTrigger("startCasting");
            StartCoroutine(start_fishing_part2());
            is_idle = false;
        }
    }

    IEnumerator start_fishing_part2()
    {
        yield return new WaitForSeconds(0.75f);

        audio_manager.sound_Play("rod_cast");
        hook.start_fishing();
        camera_main.set_camera(1);
        camera_main.set_speed(10f);
        game_ui.change_UI(1);
    }

    public void stop_fishing()
    {
        m_Animator.SetTrigger("finishCasting");
        is_idle = true;
        hook.stop_fishing();
        camera_main.set_camera(0);
        camera_main.set_speed(17f);
        game_ui.change_UI(0);
    }
}
