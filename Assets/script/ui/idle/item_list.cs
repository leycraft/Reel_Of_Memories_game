using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class item_list : MonoBehaviour
{
    hook_movement hook;


    GameObject light;
    GameObject magnet;

    GameObject seaweed;
    GameObject luxury;
    GameObject special;

    GameObject light_select;
    GameObject magnet_select;

    public List<GameObject> bait_select = new List<GameObject>();


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        hook = GameObject.Find("hook_obj").GetComponent<hook_movement>();

        light = transform.Find("light").gameObject;
        magnet = transform.Find("magnet").gameObject;

        seaweed = transform.Find("seaweed").gameObject;
        luxury = transform.Find("luxury").gameObject;
        special = transform.Find("special").gameObject;

        light_select = transform.Find("light/selected").gameObject;
        magnet_select = transform.Find("magnet/selected").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        check_visibility();
        check_select();
    }

    void check_visibility()
    {
        if (hook.bought_light)
        {
            light.SetActive(true);
        }
        else
        {
            light.SetActive(false);
        }

        if (hook.bought_magnet)
        {
            magnet.SetActive(true);
        }
        else
        {
            magnet.SetActive(false);
        }

        if (hook.bought_seaweed)
        {
            seaweed.SetActive(true);
        }
        else
        {
            seaweed.SetActive(false);
        }

        if (hook.bought_luxury)
        {
            luxury.SetActive(true);
        }
        else
        {
            luxury.SetActive(false);
        }

        if (hook.unlock_special)
        {
            special.SetActive(true);
        }
        else
        {
            special.SetActive(false);
        }
    }

    void check_select()
    {
        if (hook.magnet_on)
        {
            magnet_select.SetActive(true);
        }
        else
        {
            magnet_select.SetActive(false);
        }

        if (hook.flashlight_on)
        {
            light_select.SetActive(true);
        }
        else
        {
            light_select.SetActive(false);
        }

        for (int i = 0; i < bait_select.Count; i++)
        {
            if (i == hook.lureType)
            {
                bait_select[i].SetActive(true);
            }
            else{
                bait_select[i].SetActive(false);
            }
        }
    }

    public void select_light()
    {
        hook.flashlight_on = !hook.flashlight_on;
    }

    public void select_magnet()
    {
        hook.magnet_on = !hook.magnet_on;
    }

    public void select_food(int i)
    {
        if (i == hook.lureType)
        {
            hook.lureType = 0;
        }
        else{
            hook.lureType = i;
        }
    }
}
