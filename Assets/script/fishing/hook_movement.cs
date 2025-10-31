using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class hook_movement : MonoBehaviour
{

    float moveSpeed_final = 5f;
    public float moveSpeed_base = 5f;
    public int moveSpeed_level = 1;

    float reelLength_final = 5f;
    public float reelLength_base = 5f;
    public int reelLength_level = 1;

    float reelStrength_final = 5f;
    public float reelStrength_base = 5f;
    public int reelStrength_level = 1;

    public float stress_level = 0f;
    public float stress_increment = 1f;
    public float stress_decay = 0.1f;

    public bool magnet_on = false;
    public bool flashlight_on = false;

    public int lureType = 0;


    public float push_down_force = 5f;

    public float pull_back_power = 50f;

    public float fish_extra_strength = 1;
    float extra_strength_increment = 0.2f;

    float fish_extra_direction = 0;

    public fish_basic fish_caught;
    public GameObject item_caught;

    man_control man;
    player_stats player_stats;
    Rigidbody2D m_Rigidbody;
    Transform hook_location;
    LineDrawer fishing_line;
    AudioManager audio_manager;

    GameObject lure_sprite;

    Vector3 original_location;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        man = GameObject.Find("man_obj").GetComponent<man_control>();
        player_stats = GameObject.Find("man_obj").GetComponent<player_stats>();
        m_Rigidbody = GetComponent<Rigidbody2D>();
        fishing_line = GetComponent<LineDrawer>();
        audio_manager = GameObject.Find("REEL_player").GetComponent<AudioManager>();

        hook_location = transform.Find("line_point_end");

        lure_sprite = transform.Find("lure_all").gameObject;

        original_location = transform.position;

        transform.position = new Vector3(999, 999, 999);
        //fishing_line.enabled = false;

        StartCoroutine(hook_wiggle());
        StartCoroutine(fish_direction());

    }

    // Update is called once per frame
    void Update()
    {
        hook_moving();

        lure_visibility();

        calculate_stats();

        if (fish_caught != null)
        {
            fish_positioning();
            fishing_gameplay();
        }
    }

    public void hook_moving()
    {
        if (!man.is_idle && fish_caught == null)
        {
            float horizontalInput = Input.GetAxis("Horizontal"); // A/D or Left/Right Arrow keys
            float verticalInput = Input.GetAxis("Vertical");   // W/S or Up/Down Arrow keys

            Vector3 movement = new Vector3(horizontalInput, verticalInput, 0f);

            transform.Translate(movement * moveSpeed_final * Time.deltaTime);

            Vector3 new_scale = new Vector3(1f, 1f, 1f);

            if (horizontalInput >= 0f)
            {
                new_scale.x = 1f;
            }
            else
            {
                new_scale.x = -1f;
            }

            transform.localScale = new_scale;

            // sound control

            float audio_level = Vector3.Magnitude(movement);
            if (audio_level > 1f)
            {
                audio_level = 1f;
            }

            audio_manager.sound_volume("slow_reel", audio_level * 0.05f);
        }
    }

    public void start_fishing()
    {
        audio_manager.sound_Play("slow_reel");
        audio_manager.sound_volume("slow_reel", 0);

        audio_manager.sound_Play("fast_reel");
        audio_manager.sound_volume("fast_reel", 0);

        //fishing_line.enabled = true;
        transform.position = original_location;

        m_Rigidbody.AddForce(Vector3.down * 1.5f, ForceMode2D.Impulse);
        StartCoroutine(stop_force_down());
    }

    IEnumerator stop_force_down()
    {
        yield return new WaitForSeconds(0.6f);
        m_Rigidbody.linearVelocity = Vector3.zero;
    }

    public void stop_fishing()
    {
        // don't use this function. use one in man_control.cs

        audio_manager.sound_Stop("slow_reel");
        audio_manager.sound_Stop("fast_reel");

        if (fish_caught != null)
        {
            fish_caught.set_fishState(2);
            fish_caught = null;
        }

        transform.position = new Vector3(999, 999, 999);

        stress_level = 0;
    }

    public void calculate_stats()
    {
        moveSpeed_final = moveSpeed_base;
        reelLength_final = reelLength_base;
        reelStrength_final = reelStrength_base;
    }

    public void fish_positioning()
    {
        Vector3 mouth = fish_caught.get_mouthLocation();
        fish_caught.transform.position = hook_location.position - Vector3.Scale(mouth, fish_caught.transform.lossyScale);
    }

    public void fishing_gameplay()
    {
        Vector3 movement = new Vector3(1f, fish_extra_direction, 0f);
        Vector3 pull_back = fishing_line.get_direction().normalized;

        fish_extra_strength += extra_strength_increment * Time.deltaTime;

        if(fish_extra_strength > 1.5)
        {
            extra_strength_increment = -Mathf.Abs(extra_strength_increment);
        }
        else if (fish_extra_strength < 1)
        {
            extra_strength_increment = Mathf.Abs(extra_strength_increment);
        }


        if (Input.GetKey(KeyCode.Space))
        {
            transform.Translate(pull_back * (pull_back_power - (pull_back_power * (fish_extra_strength - 1))) * Time.deltaTime);
            stress_level += stress_increment * Time.deltaTime;

            audio_manager.sound_volume("fast_reel", 0.05f);
        }
        else
        {
            audio_manager.sound_volume("fast_reel", 0);

            transform.Translate(movement * fish_caught.pull_strength * fish_extra_strength * Time.deltaTime);

            if (stress_level > 0)
            {
                stress_level -= stress_decay * Time.deltaTime;
                if (stress_level < 0)
                {
                    stress_level = 0;
                }
            }
        }

        if (stress_level > reelStrength_final)
        {
            audio_manager.sound_Play("string_snap");
            man.stop_fishing();
        }
    }

    public void sell_fish()
    {
        if (fish_caught != null)
        {
            player_stats.money += fish_caught.price;
            Destroy(fish_caught.gameObject);
        }

    }

    public void fish_on_hook(fish_basic fish)
    {
        fish_caught = fish;
        transform.localScale = new Vector3(1f, 1f, 1f);
    }

    public void lure_visibility()
    {
        if (fish_caught == null)
        {
            lure_sprite.SetActive(true);
        }
        else
        {
            lure_sprite.SetActive(false);
        }
    }

    IEnumerator hook_wiggle()
    {
        m_Rigidbody.linearVelocity = Vector3.zero;
        if (!man.is_idle)
        {
            m_Rigidbody.AddForce(new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), 0), ForceMode2D.Impulse);
        }
        yield return new WaitForSeconds(0.5f);

        StartCoroutine(hook_wiggle());
    }

    IEnumerator fish_direction()
    {
        fish_extra_direction = Random.Range(-1f, -0.1f);
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(fish_direction());
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name == "surface")
        {
            sell_fish();
            man.stop_fishing();
        }
    }

    public Transform get_hookLocation()
    {
        return hook_location;
    }

    public float get_reelStrength()
    {
        return reelStrength_final;
    }
}
