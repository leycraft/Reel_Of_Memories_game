using UnityEngine;

public class shop_story_button : MonoBehaviour
{
    player_stats stats;

    GameObject shop_button;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        stats = GameObject.Find("man_obj").GetComponent<player_stats>();
        shop_button = transform.Find("shop").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if(stats.story_phase == 0)
        {
            shop_button.SetActive(false);
        }
        else
        {
            shop_button.SetActive(true);
        }
    }


}
