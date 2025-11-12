using TMPro;
using UnityEngine;

public class shop_buy : MonoBehaviour
{
    player_stats stats;
    hook_movement hook;

    public TextMeshProUGUI length_txt;
    public TextMeshProUGUI strength_txt;
    public TextMeshProUGUI speed_txt;


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

        calculate_price();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void calculate_price()
    {
        switch (hook.moveSpeed_level)
        {
            case 1:
                speed_price = 200;
                break;
            case 2:
                speed_price = 350;
                break;
            case 3:
                speed_price = 600;
                break;
        }

        switch (hook.reelLength_level)
        {
            case 1:
                length_price = 200;
                break;
            case 2:
                length_price = 300;
                break;
            case 3:
                length_price = 500;
                break;
        }

        switch (hook.reelStrength_level)
        {
            case 1:
                strength_price = 180;
                break;
            case 2:
                strength_price = 280;
                break;
            case 3:
                strength_price = 450;
                break;
        }

        length_txt.SetText($"${length_price}");
        strength_txt.SetText($"${strength_price}");
        speed_txt.SetText($"${speed_price}");


    }

    void check_status()
    {

    }

    public void buy_speed()
    {
        if(stats.money >= speed_price)
        {
            stats.pay_money(speed_price);
            hook.moveSpeed_level++;
        }

        calculate_price();
        check_status();
    }

    public void buy_length()
    {
        if (stats.money >= length_price)
        {
            stats.pay_money(length_price);
            hook.reelLength_level++;
        }

        calculate_price();
        check_status();
    }

    public void buy_strength()
    {
        if (stats.money >= strength_price)
        {
            stats.pay_money(strength_price);
            hook.reelStrength_level++;
        }

        calculate_price();
        check_status();
    }

    public void buy_seaweed()
    {
        if (stats.money >= seaweed_price)
        {
            stats.pay_money(seaweed_price);
            hook.bought_seaweed = true;
        }

        check_status();
    }

    public void buy_luxury()
    {
        if (stats.money >= luxury_price)
        {
            stats.pay_money(luxury_price);
            hook.bought_luxury = true;
        }

        check_status();
    }

    public void buy_light()
    {
        if (stats.money >= light_price)
        {
            stats.pay_money(light_price);
            hook.bought_light = true;
        }

        check_status();
    }

    public void buy_magnet()
    {
        if (stats.money >= magnet_price)
        {
            stats.pay_money(magnet_price);
            hook.bought_magnet = true;
        }

        check_status();
    }
}
