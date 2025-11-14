using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CutsceneStater : MonoBehaviour
{
    player_stats stats;

    public CutsceneLoader CutsceneLoader;

    public Image black_cutscene;

    float opacity = 0;

    float fade_time = 0.07f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        stats = GameObject.Find("man_obj").GetComponent<player_stats>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void LoadCutScene(string JSONName)
    {
        StartCoroutine(load_cutscene_part2(JSONName));
    }

    IEnumerator load_cutscene_part2(string JSONName)
    {
        black_cutscene.gameObject.SetActive(true);

        while (opacity < 1)
        {
            opacity += 0.1f;
            Color c = black_cutscene.color;
            c.a = opacity;
            black_cutscene.color = c;

            yield return new WaitForSeconds(fade_time);
        }

        CutsceneLoader.gameObject.SetActive(true);
        CutsceneLoader.LoadAndPlayCutscene(JSONName);
    }

    public void off_cutscene()
    {
        stats.end_demo_check();
        StartCoroutine(off_cutscene_part2());
    }

    IEnumerator off_cutscene_part2()
    {
        while (opacity > 0)
        {
            opacity -= 0.1f;
            Color c = black_cutscene.color;
            c.a = opacity;
            black_cutscene.color = c;

            yield return new WaitForSeconds(fade_time);
        }
        black_cutscene.gameObject.SetActive(false);
    }
}
