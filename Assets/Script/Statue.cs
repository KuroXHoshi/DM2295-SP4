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

    public IntRange cost_range;
    public Vector2 distance_push;

    private bool is_risen, is_bought, is_done;
    private TYPE type;
    private int cost;
    private Vector2 starting_pos;

    [SerializeField]
    private GameObject text_mesh;

    // Use this for initialization
    void Start ()
    {
        player_obj = GameObject.FindGameObjectWithTag("Player");

        player_obj_script = player_obj.GetComponent<Player>();

        is_risen = false;
        is_bought = false;
        is_done = false;
        cost_range = new IntRange(1000, 3000);
        cost = cost_range.Random;

        starting_pos = new Vector2(transform.position.x, transform.position.z);

        text_mesh.GetComponent<TextMesh>().text = "Cost: " + cost;
        text_mesh.SetActive(false);
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (!is_risen)
        {
           
            if (player_obj_script.transform.position.x < transform.position.x + distance_push.x && player_obj_script.transform.position.x > transform.position.x - distance_push.x &&
                player_obj_script.transform.position.z < transform.position.z + distance_push.y && player_obj_script.transform.position.z > transform.position.z - distance_push.y)
            {
                Vector3 normalized_vec = (player_obj_script.transform.position - transform.position).normalized * 0.2f;
                player_obj_script.transform.position += new Vector3(normalized_vec.x, 0, normalized_vec.z);            
            }

            if (transform.position.y >= 0)
            {
                is_risen = true;
                transform.position = new Vector3(starting_pos.x, 0.1f, starting_pos.y);
            }

            float temp = UnityEngine.Random.Range(.001f, .015f);

            float shaking_speed = 0.1f;

            transform.position = new Vector3(
                transform.position.x + ((transform.position.x > starting_pos.x + shaking_speed) ? -shaking_speed : ((transform.position.x < starting_pos.x - shaking_speed) ? shaking_speed : -shaking_speed)), 
                transform.position.y + temp, 
                transform.position.z + ((transform.position.z > starting_pos.y + shaking_speed) ? -shaking_speed : ((transform.position.z < starting_pos.y - shaking_speed) ? shaking_speed : -shaking_speed)));

        }
        else
        {
            if (player_obj_script.transform.position.x < transform.position.x + distance_push.x && player_obj_script.transform.position.x > transform.position.x - distance_push.x &&
               player_obj_script.transform.position.z < transform.position.z + distance_push.y && player_obj_script.transform.position.z > transform.position.z - distance_push.y)
            {
                if(!text_mesh.activeSelf)
                    text_mesh.SetActive(true);

                if (Input.GetKeyDown("return") && !is_done)
                {
                    if (player_obj_script.GetGold() >= cost)
                    {
                        is_bought = true;
                    }
                }
            }
            else
            {
                if (text_mesh.activeSelf)   
                    text_mesh.SetActive(false);
            }
        }

        if(is_bought && !is_done)
        {
            player_obj_script.SetGold(player_obj_script.GetGold() - cost);
            text_mesh.GetComponent<TextMesh>().text = "Bought!";
            is_done = true;
        }
	}

    public void SetType(int _type)
    {
        type = (TYPE)_type;
    }
}
