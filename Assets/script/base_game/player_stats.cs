using System.Collections.Generic;
using UnityEngine;

public class player_stats : MonoBehaviour
{
    public int money = 0;

    public int story_phase = 0;
    public int story_segment = 0;

    public List<GameObject> map_list = new List<GameObject>();
    public int[] fish_list;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        change_phase(0);

        foreach (int i in fish_list)
        {
            fish_list[i] = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            change_phase(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            change_phase(1);
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
}
