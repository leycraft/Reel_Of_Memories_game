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

    float dist;

    Vector3 destination;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mouth_location = transform.Find("mouth_location").gameObject;
        hook = GameObject.Find("hook_obj").GetComponent<hook_movement>();

        topleft_limit = GameObject.Find("bound_topleft");
        bottomright_limit = GameObject.Find("bound_bottomright");
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
                StartCoroutine(move_normal());
            }

            // move to destination
            float step = speed * Time.deltaTime; // calculate distance to move
            transform.position = Vector3.MoveTowards(transform.position, destination, step);


            // change fish side
            change_fish_direction(destination);


            if (dist < eyesight_length && hook.fish_caught == null)
            {
                // think about eating 

                int random_num = Random.Range(0, 100);
                if (random_num <= eat_chance && eat_cooldown == 0)
                {
                    fish_state = 1;
                }
                else
                {
                    if (eat_cooldown == 0)
                    {
                        eat_cooldown = eat_cooldown_time;
                        StartCoroutine(eating_cooldown());
                    }

                }

            }
        }
        else if (fish_state == 1)
        {
            // moving to lure

            float step = chase_speed * Time.deltaTime; // calculate distance to move
            transform.position = Vector3.MoveTowards(transform.position, hook.get_hookLocation().position - mouth_location.transform.localPosition, step);
            change_fish_direction(hook.get_hookLocation().position);

            if (hook.fish_caught != null)
            {
                fish_state = 0;
            }

            eat_bait();
        }
        else if (fish_state == 2)
        {
            // running away
            float new_x = transform.position.x + 10;

            Vector3 run_away_destination = new Vector3(new_x, transform.position.y, transform.position.z);

            transform.position = Vector3.MoveTowards(transform.position, run_away_destination, 50f * Time.deltaTime);
            change_fish_direction(run_away_destination);

        }
        else if (fish_state == 3) { 
            // do nothing state
        }
    }

    public void change_fish_direction(Vector3 destination)
    {
        Vector3 new_scale = new Vector3(1f, 1f, 1f);
        if (transform.position.x >= destination.x)
        {
            new_scale.x = 1f;
        }
        else
        {
            new_scale.x = -1f;
        }

        transform.localScale = new_scale;
    }

    IEnumerator move_normal()
    {
        yield return new WaitForSeconds(1);

        movement_cooldown--;
        if (movement_cooldown > 0)
        {
            StartCoroutine(move_normal());
        }
    }

    IEnumerator eating_cooldown()
    {
        yield return new WaitForSeconds(1);

        eat_cooldown--;
        if (eat_cooldown > 0)
        {
            StartCoroutine(eating_cooldown());
        }
    }

    public void eat_bait()
    {
        if (dist <= 0.4 && hook.fish_caught == null)
        {
            hook.fish_on_hook(this);
            fish_state = 3;
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
    }

    public void set_fishState(int num)
    {
        fish_state = num;
    }
}
