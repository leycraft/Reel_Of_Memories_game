using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class book_ui : MonoBehaviour
{
    TextMeshProUGUI fish_name;
    TextMeshProUGUI location;
    TextMeshProUGUI desc;

    player_stats stats;

    Image fish_img;

    public List<Image> icon_list = new List<Image>();


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        fish_name = transform.Find("book/txt/name").gameObject.GetComponent<TextMeshProUGUI>();
        location = transform.Find("book/txt/location").gameObject.GetComponent<TextMeshProUGUI>();
        desc = transform.Find("book/txt/desc").gameObject.GetComponent<TextMeshProUGUI>();

        stats = GameObject.Find("man_obj").GetComponent<player_stats>();

        fish_img = transform.Find("book/screen/fish_img").gameObject.GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < stats.fish_list.Length; i++) {
            if (stats.fish_list[i] > 0)
            {
                Sprite newSprite = Resources.Load<Sprite>("fish_icon_small/fish" + i.ToString());
                icon_list[i].sprite = newSprite;
            }
            else
            {
                Sprite newSprite = Resources.Load<Sprite>("fish_icon_small/mystery_small");
                icon_list[i].sprite = newSprite;
            }
        }
    }

    public void read_json(string json_name)
    {
        string fish_id = json_name.Replace("fish", string.Empty);

        if (stats.fish_list[int.Parse(fish_id)] != 0)
        {
            TextAsset loaded_json = Resources.Load<TextAsset>("fish_JSON/" + json_name);

            if (loaded_json != null)
            {
                fish_json fish_file = JsonUtility.FromJson<fish_json>(loaded_json.text);

                fish_name.SetText(fish_file.name);
                location.SetText($"Location: {fish_file.location}");
                desc.SetText($"Description: {fish_file.desc}");

                Sprite newSprite = Resources.Load<Sprite>("fish_image/" + json_name);
                fish_img.sprite = newSprite;
            }
        }
        else
        {
            fish_name.SetText("???");
            location.SetText($"Location: ???");
            desc.SetText($"Description: ???");

            Sprite newSprite = Resources.Load<Sprite>("fish_image/mystery");
            fish_img.sprite = newSprite;
        }
    }
}
