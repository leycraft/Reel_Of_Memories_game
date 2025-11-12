using TMPro;
using UnityEngine;

public class shop_ui_general : MonoBehaviour
{
    TextMeshProUGUI money_txt;
    player_stats stats;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        money_txt = transform.Find("money").gameObject.GetComponent<TextMeshProUGUI>();
        stats = GameObject.Find("man_obj").GetComponent<player_stats>();
    }

    // Update is called once per frame
    void Update()
    {
        money_txt.SetText($"${stats.money}");
    }
}
