using UnityEngine;
using System.Collections;

public class man_control : MonoBehaviour
{
    public bool is_idle = true;
    public ui_switch game_ui;
    public caught_ui caught_ui;
    public bool new_fish = false;

    cam_control camera_main;
    hook_movement hook;
    player_stats player_stats;

    Animator m_Animator;

    AudioManager audio_manager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        camera_main = GameObject.Find("Main Camera").GetComponent<cam_control>();
        hook = GameObject.Find("hook_obj").GetComponent<hook_movement>();
        player_stats = GetComponent<player_stats>();

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
        else if (new_fish)
        {
            caught_ui.close_UI();
            new_fish = false;
            is_idle = true;
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

    public void stop_fishing(fish_basic fish)
    {
        stop_fishing();

        if (fish != null)
        {
            receive_fish(fish);
            Destroy(fish.gameObject);
        }
    }

    public void stop_fishing()
    {
        float dist = Vector3.Magnitude(camera_main.camera_list[0].transform.position - camera_main.camera_list[1].transform.position);

        m_Animator.SetTrigger("finishCasting");
        is_idle = true;
        hook.stop_fishing();
        camera_main.set_camera(0);

        // set cam speed

        print(dist);
        if (dist < 17)
        {
            dist = 17f;
        }

        camera_main.set_speed(dist);
        game_ui.change_UI(0);
    }

    public void receive_fish(fish_basic fish)
    {
        for (int i = 0; i < player_stats.fish_list.Length; i++)
        {
            if (fish.fish_id == i)
            {
                player_stats.fish_list[i]++;
                player_stats.add_money(fish.price);

                if (player_stats.fish_list[i] == 1)
                {
                    caught_ui.open_UI(fish.json_name);
                    is_idle = false;
                    new_fish = true;
                }
            }
        }
    }
}
