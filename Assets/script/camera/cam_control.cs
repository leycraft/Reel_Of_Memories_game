using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class cam_control : MonoBehaviour
{
    public GameObject camera_look_at;
    public List<GameObject> camera_list = new List<GameObject>();
    float speed = 2f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        float step = speed * Time.deltaTime;

        if (camera_look_at != null)
        {
            Vector3 tar_pos = new Vector3(camera_look_at.transform.position.x, camera_look_at.transform.position.y, -1);

            if (true)
            {
                if (tar_pos.x < 3)
                {
                    tar_pos.x = 3;
                }
                else if (tar_pos.x > 22)
                {
                    tar_pos.x = 22;
                }

                if(tar_pos.y < -8.5)
                {
                    tar_pos.y = -8.5f;
                }
            }


            this.transform.position = Vector3.MoveTowards(this.transform.position, tar_pos, step);
        }
    }

    public void set_camera(int num)
    {
        camera_look_at = camera_list[num];
    }

    public void set_speed(float new_speed)
    {
        speed = new_speed;
    }
}
