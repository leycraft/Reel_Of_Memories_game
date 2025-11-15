using UnityEngine;

public class bgm_menu : MonoBehaviour
{
    AudioManager bgm;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        bgm = GameObject.Find("BGM_player").GetComponent<AudioManager>();

        bgm.sound_Play("menu");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
