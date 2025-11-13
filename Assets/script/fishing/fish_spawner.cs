using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fish_spawner : MonoBehaviour
{
    public GameObject fish;
    List<GameObject> fish_list = new List<GameObject>();
    GameObject topleft;
    GameObject bottomright;

    public int starting_fish = 0;
    public float seconds_until_spawn = 0;
    public int fish_limit_num = 0;
    public float fish_size = 1;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        topleft = transform.Find("bound_topleft").gameObject;
        bottomright = transform.Find("bound_bottomright").gameObject;

        for (int i = 0; i < starting_fish; i++)
        {
            spawn_fish();
        }

        StartCoroutine(spawn_fish_loop());
    }

    void OnEnable()
    {
        StartCoroutine(spawn_fish_loop());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator spawn_fish_loop()
    {
        fish_list.RemoveAll(item => item == null);

        yield return new WaitForSeconds(seconds_until_spawn);

        if (fish_list.Count < fish_limit_num)
        {
            spawn_fish();
        }

        StartCoroutine(spawn_fish_loop());
    }

    public void spawn_fish()
    {
        float spawn_x = 0;
        float spawn_y = 0;

        if (Random.Range(0,2) == 0)
        {
            spawn_x = topleft.transform.position.x;
        }
        else
        {
            spawn_x = bottomright.transform.position.x;
        }

        spawn_y = Random.Range(topleft.transform.position.y, bottomright.transform.position.y);

        Vector3 spawnPosition = new Vector3(spawn_x, spawn_y, 0);
        Quaternion spawnRotation = Quaternion.identity;

        GameObject newFish = Instantiate(fish, spawnPosition, spawnRotation);
        newFish.transform.localScale = new Vector3(fish_size, fish_size, 1f);
        newFish.transform.parent = gameObject.transform;

        fish_basic fish_attribute = newFish.GetComponent<fish_basic>();
        fish_attribute.set_boundary(topleft, bottomright);

        fish_list.Add(newFish);
    }
}
