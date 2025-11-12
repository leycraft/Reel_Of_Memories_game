using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Experimental.GraphView.Port;

public class ui_switch : MonoBehaviour
{
    public List<GameObject> game_ui = new List<GameObject>();
    public Image blackness;

    float opacity = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        change_UI(0);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void change_UI(int num)
    {
        for (int i = 0; i < game_ui.Count; i++)
        {
            if (i == num)
            {
                game_ui[i].SetActive(true);
            }
            else
            {
                game_ui[i].SetActive(false);
            }
        }
    }

    public void change_UI_fade(int num)
    {
        if (blackness != null)
        {
            StartCoroutine(fade_in_out(num));
        }
        else
        {
            change_UI(num);
        }
    }

    IEnumerator fade_in_out(int num)
    {
        blackness.gameObject.SetActive(true);
        while (opacity < 1)
        {
            opacity += 0.1f;
            Color c = blackness.color;
            c.a = opacity;
            blackness.color = c;

            yield return new WaitForSeconds(0.03f);
        }

        change_UI(num);

        while (opacity > 0)
        {
            opacity -= 0.1f;
            Color c = blackness.color;
            c.a = opacity;
            blackness.color = c;

            yield return new WaitForSeconds(0.03f);
        }
        blackness.gameObject.SetActive(false);
    }
}
