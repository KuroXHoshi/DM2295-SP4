﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Statue : MonoBehaviour {

    public enum TYPE
    {
        HP_REGEN = 0,
        DEFENCE,
        EVASION,
        TOTAL_TYPE
    }

    public GameObject player_obj;
    private Player player_obj_script;

    public IntRange cost;
    public Vector2 distance_push;

    private bool is_risen;
    private TYPE type;

    private Vector2 starting_pos;

    // Use this for initialization
    void Start ()
    {
        player_obj = GameObject.FindGameObjectWithTag("Player");

        player_obj_script = player_obj.GetComponent<Player>();

        is_risen = false;
        cost = new IntRange(1000, 3000);

        ///TEMP RAND OF CHOOSING BLESSING TYPE, TO BE CHANGE TO GET FROM TYPE ITSELF IN THE FUTURE
        IntRange temp = new IntRange(0, (int)TYPE.TOTAL_TYPE);
        type = (TYPE)temp.Random;

        starting_pos = new Vector2(transform.position.x, transform.position.z);

        transform.Rotate(0, -90, 0);

    }
	
	// Update is called once per frame
	void Update ()
    {
        if (!is_risen)
        {
            if (player_obj_script.transform.position.x < transform.position.x + distance_push.x && player_obj_script.transform.position.x > transform.position.x - distance_push.x &&
                player_obj_script.transform.position.z < transform.position.z + distance_push.y && player_obj_script.transform.position.z > transform.position.z - distance_push.y)
            {
                Vector3 normalized_vec = (player_obj_script.transform.position - transform.position).normalized * 0.1f;
                player_obj_script.transform.position += new Vector3(normalized_vec.x, 0, normalized_vec.z);            
            }

            if (transform.position.y >= 0)
            {
                is_risen = true;
                transform.position = new Vector3(starting_pos.x, 0.1f, starting_pos.y);
            }

            float temp = UnityEngine.Random.Range(.001f, .1f);

            float shaking_speed = 0.1f;

            transform.position = new Vector3(
                transform.position.x + ((transform.position.x > starting_pos.x + shaking_speed) ? -shaking_speed : ((transform.position.x < starting_pos.x - shaking_speed) ? shaking_speed : -shaking_speed)), 
                transform.position.y + temp, 
                transform.position.z + ((transform.position.z > starting_pos.y + shaking_speed) ? -shaking_speed : ((transform.position.z < starting_pos.y - shaking_speed) ? shaking_speed : -shaking_speed)));

        }
        else
        {

        }
	}

    public void SetType(int _type)
    {
        type = (TYPE)_type;
    }
}
