using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_stats : MonoBehaviour
{
    public int money = 0;

    public int story_phase = 0;
    public int story_segment = 0;

    public List<GameObject> map_list = new List<GameObject>();
    public int[] fish_list;
    public int box_collected = 0;

    // story control

    CutsceneStater cut_stater;
    LineDrawer lineDrawer;
    hook_movement hook;
    public GameObject story_fish1;
    public GameObject box1;
    public fish_spawner six_area;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        change_phase(0);

        foreach (int i in fish_list)
        {
            fish_list[i] = 0;
        }

        cut_stater = GameObject.Find("CutsceneController").GetComponent<CutsceneStater>();
        lineDrawer = GameObject.Find("hook_obj").GetComponent<LineDrawer>();
        hook = GameObject.Find("hook_obj").GetComponent<hook_movement>();

        story_fish1 = GameObject.Find("six_eye_fish_for_story1");

        cut_stater.LoadCutScene("first_cutscene");

    }

    // Update is called once per frame
    void Update()
    {
        check_story();
    }

    void check_story()
    {
        // tutorial map
        if (story_phase == 0)
        {
            if (story_segment == 0)
            {
                // get 3 fishes
                if (fish_list[0] >= 3)
                {
                    story_segment = 1;

                    cut_stater.LoadCutScene("second_cutscene");
                }
            }
            else if (story_segment == 1)
            {
                StartCoroutine(delay_change(1, 0));
                story_segment = 2;
            }
        }

        // big map
        else if (story_phase == 1)
        {
            if (story_segment == 0)
            {
                // get very deep
                float depth = lineDrawer.get_depth_from_surface_no_abs();
                if (depth > 27)
                {
                    story_segment = 1;
                }
            }
            else if (story_segment == 1)
            {
                // buy light
                if (hook.bought_light)
                {
                    story_segment = 2;
                }
            }
            else if (story_segment == 2)
            {
                // found 6 eyes
                // check in function
            }
            else if (story_segment == 3)
            {
                // 6 eyes swim
                if (story_fish1.activeInHierarchy)
                {

                    Vector3 new_pos = story_fish1.transform.position;
                    new_pos.x = new_pos.x - (15 * Time.deltaTime);

                    story_fish1.transform.position = new_pos;

                    if (story_fish1.transform.localPosition.x < -35)
                    {
                        story_fish1.SetActive(false);
                        story_segment = 4;
                    }
                }
            }
            else if (story_segment == 4)
            {
                // go to shop
                // check in function
            }
            else if (story_segment == 5)
            {
                // go to shop again after get box
                // check in function
            }
        }
    }

    public void add_money(int money)
    {
        this.money += money;
    }

    public void pay_money(int money)
    {
        if (this.money >= money)
        {
            this.money -= money;
        }
    }

    public void change_phase(int num)
    {
        story_phase = num;

        for (int i = 0; i < map_list.Count; i++)
        {
            if (i == num)
            {
                map_list[i].SetActive(true);
            }
            else
            {
                map_list[i].SetActive(false);
            }
        }
    }

    IEnumerator delay_change(int phase, int segment)
    {
        yield return new WaitForSeconds(3);
        change_phase(phase);
        story_segment = segment;
    }

    public void hit_area_6eye_check()
    {
        if (story_segment == 2 && story_phase == 1)
        {
            if (hook.flashlight_on)
            {
                story_fish1 = GameObject.Find("six_eye_fish_for_story1");
                story_fish1.SetActive(true);
                story_segment = 3;
            }

        }
    }

    public void go_to_shop_check()
    {
        if (story_segment == 4 && story_phase == 1)
        {
            cut_stater.LoadCutScene("third_cutscene");
            story_segment = 5;
        }
        else if (story_segment == 5 && story_phase == 1)
        {
            if (box_collected == 1)
            {
                cut_stater.LoadCutScene("forth_cutscene");
                box1.SetActive(true);
                story_segment = 6;
            }
        }
        else if (story_segment == 6 && story_phase == 1)
        {
            if (box_collected == 2)
            {
                cut_stater.LoadCutScene("fifth_cutscene");
                hook.unlock_special = true;
                six_area.fish_limit_num = 1;
                story_segment = 7;
            }
        }
        else if (story_segment == 7 && story_phase == 1)
        {
            if (fish_list[6] >= 1)
            {
                cut_stater.LoadCutScene("final_cutscene");
                story_segment = 8;
            }
        }
    }
}
