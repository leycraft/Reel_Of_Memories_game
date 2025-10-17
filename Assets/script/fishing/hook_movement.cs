using UnityEngine;

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

    public fish_basic fish_caught;
    public GameObject item_caught;

    man_control man;
    player_stats player_stats;
    Rigidbody2D m_Rigidbody;
    Transform hook_location;
    LineDrawer fishing_line;

    Vector3 original_location;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        man = GameObject.Find("man_obj").GetComponent<man_control>();
        player_stats = GameObject.Find("man_obj").GetComponent<player_stats>();
        m_Rigidbody = GetComponent<Rigidbody2D>();
        fishing_line = GetComponent<LineDrawer>();

        hook_location = transform.Find("hook");

        original_location = transform.position;



        transform.position = new Vector3(999, 999, 999);
    }

    // Update is called once per frame
    void Update()
    {
        if (!man.is_idle && fish_caught == null)
        {
            float horizontalInput = Input.GetAxis("Horizontal"); // A/D or Left/Right Arrow keys
            float verticalInput = Input.GetAxis("Vertical");   // W/S or Up/Down Arrow keys

            Vector3 movement = new Vector3(horizontalInput, verticalInput, 0f);

            transform.Translate(movement * moveSpeed_final * Time.deltaTime);
        }

        calculate_stats();

        if (fish_caught != null)
        {
            fish_positioning();
            fishing_gameplay();
        }
    }

    public void start_fishing()
    {
        fishing_line.enabled = true;
        gameObject.SetActive(true);
        transform.position = original_location;
    }

    public void stop_fishing()
    {
        if (fish_caught != null)
        {
            fish_caught.set_fishState(2);
            fish_caught = null;
        }

        fishing_line.enabled = false;
        gameObject.SetActive(false);

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
        fish_caught.transform.position = hook_location.position - mouth;
    }

    public void fishing_gameplay()
    {
        Vector3 movement = new Vector3(1f, -1f, 0f);
        Vector3 pull_back = fishing_line.get_direction().normalized;

        if (Input.GetKey(KeyCode.Space))
        {
            transform.Translate(pull_back * (pull_back_power + fish_caught.pull_strength) * Time.deltaTime);
            stress_level += stress_increment;
        }
        else
        {
            transform.Translate(movement * fish_caught.pull_strength * Time.deltaTime);

            if (stress_level > 0)
            {
                stress_level -= stress_decay;
                if (stress_level < 0)
                {
                    stress_level = 0;
                }
            }
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

    public Transform get_hookLocation()
    {
        return hook_location;
    }

    public void fish_on_hook(fish_basic fish)
    {
        fish_caught = fish;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name == "surface")
        {
            sell_fish();
            man.stop_fishing();
        }
    }
}
