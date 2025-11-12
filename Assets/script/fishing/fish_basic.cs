using System.Collections;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Rendering;
using static UnityEngine.GraphicsBuffer;

public class fish_basic : MonoBehaviour
{
    GameObject mouth_location;
    hook_movement hook;

    GameObject topleft_limit;
    GameObject bottomright_limit;

    Animator f_Animator;

    public int fish_id = 1;
    public string json_name;

    public float weight = 1f;
    public int price = 100;

    public float speed = 1f;
    public int movement_cooldown = 0;
    public int movement_cooldown_time = 5;
    public float chase_speed = 1f;

    public float pull_strength = 1f;
    public int preferred_lure = 0;

    public float eyesight_length = 1f;
    public int eat_chance = 1;
    public int eat_cooldown_time = 2;
    public int eat_cooldown = 0;

    int fish_state = 0;
    bool chasing_flag = false;

    float dist;

    Vector3 destination;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mouth_location = transform.Find("mouth_location").gameObject;
        hook = GameObject.Find("hook_obj").GetComponent<hook_movement>();

        f_Animator = transform.Find("sprites/Square").gameObject.GetComponent<Animator>();

        TextAsset loaded_json = Resources.Load<TextAsset>("fish_JSON/" + json_name);

        if (loaded_json != null)
        {
            fish_json fish_file = JsonUtility.FromJson<fish_json>(loaded_json.text);

            price = fish_file.price;
            speed = fish_file.speed;
            movement_cooldown_time = fish_file.moveFreq;
            chase_speed = fish_file.chaseSpd;
            pull_strength = fish_file.pullStr;
            preferred_lure = fish_file.bait;
            eyesight_length = fish_file.eyesight;
        }

        StartCoroutine(eating_cooldown());
        StartCoroutine(move_normal());
    }

    // Update is called once per frame
    void Update()
    {
        dist = Vector3.Distance(hook.get_hookLocation().position, mouth_location.transform.position);

        fish_swimming();
    }

    public Vector3 get_mouthLocation()
    {
        return mouth_location.transform.localPosition;
    }

    public void fish_swimming()
    {

        if (fish_state == 0)
        {
            // moving normally

            if (movement_cooldown == 0)
            {
                float random_x = Random.Range(topleft_limit.transform.position.x, bottomright_limit.transform.position.x);
                float random_y = Random.Range(topleft_limit.transform.position.y, bottomright_limit.transform.position.y);

                destination = new Vector3(random_x, random_y, transform.position.z);

                movement_cooldown = movement_cooldown_time;
            }

            // move to destination
            float step = speed * Time.deltaTime; // calculate distance to move
            transform.position = Vector3.MoveTowards(transform.position, destination, step);


            // change fish side
            change_fish_direction(destination);


            if (dist < eyesight_length && hook.fish_caught == null)
            {
                // check condition to eat
                bool can_eat = true;

                // magnet must not be on
                if (hook.magnet_on)
                {
                    can_eat = false;
                }

                
                if (can_eat)
                {


                    // think about eating 

                    int random_num = Random.Range(0, 100);
                    if (random_num <= eat_chance && eat_cooldown == 0)
                    {
                        // go eat

                        fish_state = 1;
                        f_Animator.SetTrigger("go_fast");
                    }
                    else
                    {
                        if (eat_cooldown == 0)
                        {
                            eat_cooldown = eat_cooldown_time;
                        }

                    }
                }

            }
        }
        else if (fish_state == 1)
        {
            // moving to lure

            float step = chase_speed * Time.deltaTime; // calculate distance to move
            transform.position = Vector3.MoveTowards(transform.position, hook.get_hookLocation().position - Vector3.Scale(mouth_location.transform.localPosition, mouth_location.transform.lossyScale), step);
            change_fish_direction(hook.get_hookLocation().position);

            if (hook.fish_caught != null || dist > 15)
            {
                // can't eat because hook is too far or occupied

                fish_state = 0;
                f_Animator.SetTrigger("go_slow");
                chasing_flag = false;
            }

            if (!chasing_flag)
            {
                StartCoroutine(chasing_bored());
                chasing_flag = true;
            }


            eat_bait();
        }
        else if (fish_state == 2)
        {
            // running away
            float new_x = transform.position.x + 10;

            Vector3 run_away_destination = new Vector3(new_x, transform.position.y, transform.position.z);

            transform.position = Vector3.MoveTowards(transform.position, run_away_destination, 40f * Time.deltaTime);
            change_fish_direction(run_away_destination);

            Destroy(gameObject, 3f);

        }
        else if (fish_state == 3)
        {
            // do nothing state
        }
    }

    public void change_fish_direction(Vector3 destination)
    {
        // change side

        Vector3 new_scale = gameObject.transform.localScale;
        if (transform.position.x > destination.x)
        {
            new_scale.x = Mathf.Abs(new_scale.x);
        }
        else if (transform.position.x < destination.x)
        {
            new_scale.x = -Mathf.Abs(new_scale.x);
        }

        transform.localScale = new_scale;

        // change rotation

        Vector3 relativePos = destination - transform.position;
        relativePos.Normalize();
        float look_at = Vector3.Angle(new Vector3(1, 0, 0), relativePos);

        if (look_at > 90)
        {
            look_at = 180 - look_at;
        }

        if (relativePos.y < 0)
        {
            look_at = -look_at;
        }

        if (transform.localScale.x > 0)
        {
            look_at = -look_at;
        }

        Quaternion new_rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, look_at);

        transform.rotation = new_rotation;
    }

    IEnumerator move_normal()
    {
        yield return new WaitForSeconds(1);

        if (movement_cooldown > 0)
        {
            movement_cooldown--;
        }

        StartCoroutine(move_normal());
    }

    IEnumerator eating_cooldown()
    {
        yield return new WaitForSeconds(1);

        if (eat_cooldown > 0)
        {
            eat_cooldown--;
        }

        StartCoroutine(eating_cooldown());
    }

    public void eat_bait()
    {
        if (dist <= 0.4 && hook.fish_caught == null)
        {
            hook.fish_on_hook(this);
            fish_state = 3;

            Vector3 new_scale = gameObject.transform.localScale;
            new_scale.x = -Mathf.Abs(new_scale.x);

            transform.localScale = new_scale;

            transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, 0);
        }


    }

    IEnumerator chasing_bored()
    {
        yield return new WaitForSeconds(5);
        if (hook.fish_caught == null && fish_state == 1)
        {
            // can't eat because chase too long

            eat_cooldown = eat_cooldown_time;
            fish_state = 0;
            f_Animator.SetTrigger("go_slow");
            chasing_flag = false;
        }
    }

    public void set_fishState(int num)
    {
        fish_state = num;
    }

    public void set_boundary(GameObject topleft, GameObject bottomright)
    {
        topleft_limit = topleft;
        bottomright_limit = bottomright;
    }
}