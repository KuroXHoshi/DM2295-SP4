using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour {

    public GameObject player_obj;
    private Player player_obj_script;

    public Vector2 distance_push;

    private bool is_risen, is_done;
    private Vector2 starting_pos;

    // Use this for initialization
    void Start()
    {
        player_obj = GameObject.FindGameObjectWithTag("Player");

        player_obj_script = player_obj.GetComponent<Player>();

        Reset();
    }

    // Update is called once per frame
    void Update()
    {
        if (!is_risen)
        {

            if (player_obj_script.transform.position.x < transform.position.x + distance_push.x && player_obj_script.transform.position.x > transform.position.x - distance_push.x &&
                player_obj_script.transform.position.z < transform.position.z + distance_push.y && player_obj_script.transform.position.z > transform.position.z - distance_push.y)
            {
                Vector3 normalized_vec = (player_obj_script.transform.position - transform.position).normalized * 0.2f;
                player_obj_script.transform.position += new Vector3(normalized_vec.x, 0, normalized_vec.z);
            }

            if (transform.position.y >= 1.2f)
            {
                is_risen = true;
                transform.position = new Vector3(starting_pos.x, 1.2f, starting_pos.y);
            }

            float temp = UnityEngine.Random.Range(.02f, .025f);

            float shaking_speed = 0.1f;

            transform.position = new Vector3(
                transform.position.x + ((transform.position.x > starting_pos.x + shaking_speed) ? -shaking_speed : ((transform.position.x < starting_pos.x - shaking_speed) ? shaking_speed : -shaking_speed)),
                transform.position.y + temp,
                transform.position.z + ((transform.position.z > starting_pos.y + shaking_speed) ? -shaking_speed : ((transform.position.z < starting_pos.y - shaking_speed) ? shaking_speed : -shaking_speed)));

        }
        else
        {
            if (player_obj_script.transform.position.x < transform.position.x + (distance_push.x * 0.5f) && player_obj_script.transform.position.x > transform.position.x - (distance_push.x * 0.5f) &&
               player_obj_script.transform.position.z < transform.position.z + (distance_push.y * 0.5f) && player_obj_script.transform.position.z > transform.position.z - (distance_push.y  * 0.5f))
            {
                if (!is_done)
                {
                    is_done = true;
                }
            }         
        }      
    }

    public bool GetIsDone()
    {
        return is_done;
    }

    public void Reset()
    {
        is_done = false;
        is_risen = false;

        starting_pos = new Vector2(transform.position.x, transform.position.z);
    }
}
