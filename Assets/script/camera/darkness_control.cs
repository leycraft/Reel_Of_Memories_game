using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.Port;

public class darkness_control : MonoBehaviour
{
    LineDrawer lineDrawer;
    man_control man;
    hook_movement hook;

    public SpriteRenderer darkness_sprite;

    public float depth = 0;
    public bool affected_by_light = false;

    int last_darkness = 0;
    int darkness_level = 0;
    float opacity = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        lineDrawer = GameObject.Find("hook_obj").GetComponent<LineDrawer>();
        man = GameObject.Find("man_obj").GetComponent<man_control>();
        hook = GameObject.Find("hook_obj").GetComponent<hook_movement>();
        StartCoroutine(sprite_change());
    }

    // Update is called once per frame
    void Update()
    {
        darkness_check();
    }

    void darkness_check()
    {

        if (!man.is_idle)
        {
            if (lineDrawer.get_depth_from_surface_no_abs() < depth)
            {
                darkness_level = 0;
            }
            else
            {
                if (!affected_by_light)
                {
                    darkness_level = 1;
                }
                else
                {
                    if (hook.flashlight_on)
                    {
                        darkness_level = 0;
                    }
                    else
                    {
                        darkness_level = 1;
                    }
                }
            }
        }
        else
        {
            darkness_level = 0;
        }

    }


    IEnumerator sprite_change()
    {
        if (last_darkness != darkness_level)
        {
            opacity = 0;
            while (opacity < 1)
            {
                opacity += 0.1f;
                if (darkness_level == 1)
                {
                    Color c = darkness_sprite.color;
                    c.a = opacity;
                    darkness_sprite.color = c;
                }
                else
                {
                    Color c = darkness_sprite.color;
                    c.a = 1 - opacity;
                    darkness_sprite.color = c;
                }
                yield return new WaitForSeconds(0.03f);
            }

            last_darkness = darkness_level;

        }
        else
        {
            yield return new WaitForSeconds(0.1f);
        }

        StartCoroutine(sprite_change());
    }
}


