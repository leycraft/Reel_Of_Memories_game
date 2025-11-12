using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Experimental.GraphView.Port;

public class caught_ui : MonoBehaviour
{
    public GameObject obj_group;
    public List<Image> UI_objects = new List<Image>();
    public List<TextMeshProUGUI> txt_objects = new List<TextMeshProUGUI>();

    float opacity = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void open_UI(string json_name)
    {
        obj_group.SetActive(true);

        TextAsset loaded_json = Resources.Load<TextAsset>("fish_JSON/" + json_name);

        if (loaded_json != null)
        {
            fish_json fish_file = JsonUtility.FromJson<fish_json>(loaded_json.text);

            txt_objects[0].SetText($"You caught {fish_file.name} ({fish_file.price})");
            txt_objects[1].SetText(fish_file.comment);

            Sprite newSprite = Resources.Load<Sprite>("fish_image/" + json_name);
            UI_objects[3].sprite = newSprite;
        }


        StartCoroutine(open_UI_part2());
    }

    IEnumerator open_UI_part2()
    {
        while (opacity < 1)
        {
            opacity += 0.1f;

            foreach (Image obj in UI_objects)
            {
                Color c = obj.color;
                c.a = opacity;
                obj.color = c;
            }

            foreach (TextMeshProUGUI txt in txt_objects)
            {
                Color c = txt.color;
                c.a = opacity;
                txt.color = c;
            }

            yield return new WaitForSeconds(0.03f);
        }
    }

    public void close_UI()
    {
        StartCoroutine(close_UI_part2());
    }

    IEnumerator close_UI_part2()
    {
        while (opacity > 0)
        {
            opacity -= 0.1f;

            foreach (Image obj in UI_objects)
            {
                Color c = obj.color;
                c.a = opacity;
                obj.color = c;
            }

            foreach (TextMeshProUGUI txt in txt_objects)
            {
                Color c = txt.color;
                c.a = opacity;
                txt.color = c;
            }

            yield return new WaitForSeconds(0.03f);
        }

        obj_group.SetActive(false);
    }
}
