using UnityEngine;

public class bgm_game : MonoBehaviour
{
    public GameObject cutscene;
    public GameObject shop;
    LineDrawer hook;

    AudioManager bgm;

    float bgm_sound = 0.3f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        bgm = GameObject.Find("BGM_player").GetComponent<AudioManager>();
        hook = GameObject.Find("hook_obj").GetComponent<LineDrawer>();

        for (int i = 0; i < bgm.sounds.Length; i++)
        {
            bgm.sounds[i].source.Play();
            bgm.sounds[i].source.volume = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < bgm.sounds.Length; i++)
        {
            bgm.sounds[i].source.volume = 0;
        }

        if (cutscene.activeInHierarchy)
        {
            // do nothing lol
        }
        else if (shop.activeInHierarchy)
        {
           bgm.sound_volume("shop", bgm_sound);
        }
        else
        {
            if (hook.get_depth_from_surface_no_abs() > 27)
            {
                bgm.sound_volume("layer3", bgm_sound);
            }
            else if (hook.get_depth_from_surface_no_abs() > 15)
            {
                bgm.sound_volume("layer2", bgm_sound);
            }
            else
            {
                bgm.sound_volume("layer1", bgm_sound);
            }
        }
    }
}
