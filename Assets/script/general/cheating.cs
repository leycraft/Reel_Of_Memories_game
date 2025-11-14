using Unity.VisualScripting;
using UnityEngine;

public class cheating : MonoBehaviour
{
    player_stats stats;
    hook_movement hook;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        stats = GameObject.Find("man_obj").GetComponent<player_stats>();
        hook = GameObject.Find("hook_obj").GetComponent<hook_movement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            stats.change_phase(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            stats.change_phase(1);
        }

        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            // god hook

            hook.bought_light = true;
            hook.bought_magnet = true;
            hook.bought_seaweed = true;
            hook.bought_luxury = true;
            hook.unlock_special = true;

            hook.moveSpeed_level = 4;
            hook.reelStrength_level = 9;
            hook.reelLength_level = 4;
        }

        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            // catch all fish
            for (int i = 0; i < stats.fish_list.Length; i++) {
                stats.fish_list[i] = 10;
            }
        }

        if (Input.GetKeyDown(KeyCode.Keypad3))
        {
            // infinite money
            stats.money = 999999;
        }
    }
}
