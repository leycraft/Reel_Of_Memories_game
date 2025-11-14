using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    AudioManager audio_player;
    public TextMeshProUGUI quest_line;
    public GameObject story_fish1;
    public GameObject box1;
    public fish_spawner six_area;
    public GameObject dialogue_bar;
    public TextMeshProUGUI dialogue_text;

    bool story_sound_on = false;

    // used once for story progress. start off false
    bool story_use_flashlight = false;
    bool story_use_special_bait = false;


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
        audio_player = GameObject.Find("STORY_player").GetComponent<AudioManager>();

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
                quest_line.SetText("Catch 3 fishes.");

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
                quest_line.SetText("Explore the river.");

                float depth = lineDrawer.get_depth_from_surface_no_abs();
                if (depth > 27)
                {
                    story_segment = 1;
                    play_dialogue("It’s way too dark down here… I can’t see a thing.\r\nI should probably go back and buy a light from that shop.");
                }
            }
            else if (story_segment == 1)
            {
                // buy light
                quest_line.SetText("Buy light bulb.");

                if (hook.bought_light)
                {
                    story_segment = 2;
                }
            }
            else if (story_segment == 2)
            {
                // find 6 eyes
                // check in function
                quest_line.SetText("Explore the deep.");

                float depth = lineDrawer.get_depth_from_surface_no_abs();
                if (depth > 27 && !story_use_flashlight)
                {
                    play_dialogue("This should be the deepest part…\r\nThat Six-Eyed Fish has to be somewhere around here.");
                    story_use_flashlight = true;
                }
            }
            else if (story_segment == 3)
            {
                // 6 eyes appears
                if (!story_sound_on)
                {
                    audio_player.sound_Play("six_jumpscare");
                    story_sound_on = true;
                }
                

                // 6 eyes swims
                if (story_fish1.activeInHierarchy)
                {
                    Vector3 new_pos = story_fish1.transform.position;
                    new_pos.x = new_pos.x - (15 * Time.deltaTime);

                    story_fish1.transform.position = new_pos;

                    if (story_fish1.transform.localPosition.x < -35)
                    {
                        story_fish1.SetActive(false);
                        story_segment = 4;
                        story_sound_on = false;
                        play_dialogue("What the hell was that!?\r\nThat must’ve been the Six-Eyed Fish everyone’s been talking about!\r\nBut… it didn’t even care about the bait.\r\nMaybe that shopkeeper knows another way to lure it out.\r\n");
                    }
                }
            }
            else if (story_segment == 4)
            {
                // go to shop
                // check in function
                quest_line.SetText("Go to the shop.");
            }
            else if (story_segment == 5)
            {
                // go to shop again after get box
                // check in function
                quest_line.SetText("Fish for the metal box for Shopkeeper.");
            }
            else if (story_segment == 6)
            {
                // go to shop again after get box, again
                // check in function
                quest_line.SetText("Fish for the metal box for Shopkeeper, again.");
            }
            else if (story_segment == 7)
            {
                // go to shop again after get box
                // check in function
                quest_line.SetText("Catch the mysterious fish, then go see the Shopkeeper.");

                float depth = lineDrawer.get_depth_from_surface_no_abs();
                if (!story_use_special_bait && depth > 5 && hook.lureType == 3)
                {
                    play_dialogue("Looks like that shopkeeper wasn’t kidding… These fish are a lot more aggressive than before.");
                    story_use_special_bait = true;
                }
            }


            // 5 - 7 check from functions
            // 7 is go to shop with 6 eyes, if yes go to 8

            // 8 check end of demo, if yes go to 9 
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
        yield return new WaitForSeconds(2.5f);
        change_phase(phase);
        story_segment = segment;
    }

    void play_dialogue(string text)
    {
        dialogue_bar.SetActive(true);
        dialogue_text.SetText(text);
        StartCoroutine(off_dialogue());
    }

    IEnumerator off_dialogue()
    {
        yield return new WaitForSeconds(8f);
        dialogue_bar.SetActive(false);
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
        if (story_phase == 1)
        {
            if (story_segment == 4)
            {
                cut_stater.LoadCutScene("third_cutscene");
                story_segment = 5;
            }
            else if (story_segment == 5)
            {
                if (box_collected == 1)
                {
                    cut_stater.LoadCutScene("forth_cutscene");
                    box1.SetActive(true);
                    story_segment = 6;
                }
            }
            else if (story_segment == 6)
            {
                if (box_collected == 2)
                {
                    cut_stater.LoadCutScene("fifth_cutscene");
                    hook.unlock_special = true;
                    six_area.fish_limit_num = 1;
                    story_segment = 7;
                }
            }
            else if (story_segment == 7)
            {
                if (fish_list[6] >= 1)
                {
                    cut_stater.LoadCutScene("final_cutscene");
                    story_segment = 8;
                }
            }
        }
    }

    public void end_demo_check()
    {
        if (story_phase == 1)
        {
            if (story_segment == 8)
            {
                SceneManager.LoadScene("menu");
            }
        }

    }
}
