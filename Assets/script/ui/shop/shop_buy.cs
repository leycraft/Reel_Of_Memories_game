using TMPro;
using UnityEngine;

public class shop_buy : MonoBehaviour
{
    player_stats stats;
    hook_movement hook;
    AudioManager sound_player;

    public TextMeshProUGUI length_txt;
    public TextMeshProUGUI strength_txt;
    public TextMeshProUGUI speed_txt;

    public TextMeshProUGUI length_now;
    public TextMeshProUGUI strength_now;
    public TextMeshProUGUI speed_now;

    public TextMeshProUGUI length_next;
    public TextMeshProUGUI strength_next;
    public TextMeshProUGUI speed_next;

    public GameObject length_buy_button;
    public GameObject strength_buy_button;
    public GameObject speed_buy_button;

    public GameObject length_x;
    public GameObject strength_x;
    public GameObject speed_x;

    public GameObject lure1_sold;
    public GameObject lure2_sold;
    public GameObject light_sold;
    public GameObject magnet_sold;


    int speed_price = 200;
    int strength_price = 180;
    int length_price = 200;

    int seaweed_price = 150;
    int luxury_price = 400;
    int light_price = 500;
    int magnet_price = 700;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        stats = GameObject.Find("man_obj").GetComponent<player_stats>();
        hook = GameObject.Find("hook_obj").GetComponent<hook_movement>();
        sound_player = GameObject.Find("SHOP_player").GetComponent<AudioManager>();
    }

    // Update is called once per frame
    void Update()
    {
        calculate_price();
        check_status();
    }

    void calculate_price()
    {
        switch (hook.moveSpeed_level)
        {
            case 1:
                speed_price = 200;
                speed_now.SetText("100%");
                speed_next.SetText("120%");
                break;
            case 2:
                speed_price = 350;
                speed_now.SetText("120%");
                speed_next.SetText("140%");
                break;
            case 3:
                speed_price = 600;
                speed_now.SetText("140%");
                speed_next.SetText("160%");
                break;
            case 4:
                speed_now.SetText("160%");
                speed_next.SetText("Max");
                break;
        }

        switch (hook.reelLength_level)
        {
            case 1:
                length_price = 200;
                length_now.SetText("20m");
                length_next.SetText("45m");
                break;
            case 2:
                length_price = 300;
                length_now.SetText("45m");
                length_next.SetText("70m");
                break;
            case 3:
                length_price = 500;
                length_now.SetText("70m");
                length_next.SetText("95m");
                break;
            case 4:
                length_now.SetText("95m");
                length_next.SetText("Max");
                break;
        }

        switch (hook.reelStrength_level)
        {
            case 1:
                strength_price = 180;
                strength_now.SetText("100%");
                strength_next.SetText("115%");
                break;
            case 2:
                strength_price = 280;
                strength_now.SetText("115%");
                strength_next.SetText("130%");
                break;
            case 3:
                strength_price = 450;
                strength_now.SetText("130%");
                strength_next.SetText("150%");
                break;
            case 4:
                strength_now.SetText("150%");
                strength_next.SetText("Max");
                break;
        }

        length_txt.SetText($"${length_price}");
        strength_txt.SetText($"${strength_price}");
        speed_txt.SetText($"${speed_price}");


    }

    void check_status()
    {
        if (hook.reelLength_level == 4)
        {
            length_buy_button.SetActive(false);
            length_x.SetActive(true);
        }
        else
        {
            length_buy_button.SetActive(true);
            length_x.SetActive(false);
        }

        if(hook.reelStrength_level == 4)
        {
            strength_buy_button.SetActive(false);
            strength_x.SetActive(true);
        }
        else
        {
            strength_buy_button.SetActive(true);
            strength_x.SetActive(false);
        }

        if(hook.moveSpeed_level == 4)
        {
            speed_buy_button.SetActive(false);
            speed_x.SetActive(true);
        }
        else
        {
            speed_buy_button.SetActive(true);
            speed_x.SetActive(false);
        }

        if (hook.bought_seaweed)
        {
            lure1_sold.SetActive(true);
        }
        else
        {
            lure1_sold.SetActive(false);
        }

        if (hook.bought_luxury)
        {
            lure2_sold.SetActive(true);
        }
        else
        {
            lure2_sold.SetActive(false);
        }

        if (hook.bought_light)
        {
            light_sold.SetActive(true);
        }
        else
        {
            light_sold.SetActive(false);
        }

        if (hook.bought_magnet)
        {
            magnet_sold.SetActive(true);
        }
        else
        {
            magnet_sold.SetActive(false);
        }
    }

    public void buy_speed()
    {
        if (stats.money >= speed_price)
        {
            stats.pay_money(speed_price);
            hook.moveSpeed_level++;
            sound_player.sound_Play("buy_sound");
        }
    }

    public void buy_length()
    {
        if (stats.money >= length_price)
        {
            stats.pay_money(length_price);
            hook.reelLength_level++;
            sound_player.sound_Play("buy_sound");
        }
    }

    public void buy_strength()
    {
        if (stats.money >= strength_price)
        {
            stats.pay_money(strength_price);
            hook.reelStrength_level++;
            sound_player.sound_Play("buy_sound");
        }
    }

    public void buy_seaweed()
    {
        if (stats.money >= seaweed_price)
        {
            stats.pay_money(seaweed_price);
            hook.bought_seaweed = true;
            sound_player.sound_Play("buy_sound");
        }
    }

    public void buy_luxury()
    {
        if (stats.money >= luxury_price)
        {
            stats.pay_money(luxury_price);
            hook.bought_luxury = true;
            sound_player.sound_Play("buy_sound");
        }
    }

    public void buy_light()
    {
        if (stats.money >= light_price)
        {
            stats.pay_money(light_price);
            hook.bought_light = true;
            sound_player.sound_Play("buy_sound");
        }
    }

    public void buy_magnet()
    {
        if (stats.money >= magnet_price)
        {
            stats.pay_money(magnet_price);
            hook.bought_magnet = true;
            sound_player.sound_Play("buy_sound");
        }
    }
}
