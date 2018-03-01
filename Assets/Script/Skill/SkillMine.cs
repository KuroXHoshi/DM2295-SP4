﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillMine : SkillScript
{
    public float timer;
    private float max_timer;
    public Vector2 distance_detect;
    bool triggered = false;
    public float dmg;

    protected override void Awake()
    {
    }

    // Use this for initialization
    protected override void Start()
    {
        max_timer = timer;
    }

    protected override void FixedUpdate()
    {
        List<List<GameObject>> list = SpawnerManager.Instance.GetAllEntity();

        foreach (List<GameObject> i in list)
        {
            foreach (GameObject obj in i)
            {
                if (obj.transform.position.x < transform.position.x + distance_detect.x && obj.transform.position.x > transform.position.x - distance_detect.x &&
            obj.transform.position.z < transform.position.z + distance_detect.y && obj.transform.position.z > transform.position.z - distance_detect.y)
                {
                    if (!obj.GetInstanceID().Equals(parent_ID))
                    {
                        if (obj.GetComponent<EnemyScript>() != null)
                        {
                            if(triggered && timer <= 0)
                             obj.GetComponent<EnemyScript>().OnAttacked(dmg);
                            triggered = true;
                            //Debug.Log("HITTTO RIMNO");
                        }
                        else if (obj.GetComponent<BossScript>() != null)
                        {
                            if (triggered && timer <= 0)
                                obj.GetComponent<BossScript>().OnAttacked(dmg);
                            triggered = true;
                        }
                        else if (obj.GetComponent<Player>() != null)
                        {
                            if (triggered && timer <= 0)
                                obj.GetComponent<Player>().TakeDamage(dmg, transform.position);
                            triggered = true;
                        }
                    }
                }
            }
        }

        if(triggered)
        {
            if (timer <= 0)
            {
                //EXPLOSION PARTICLE HERE
                Instantiate(particle, transform.position, transform.rotation);
                Reset();
            }

            timer -= Time.deltaTime;
        }
    }

    protected override void LateUpdate()
    {
    }

    protected override void OnCollisionEnter(Collision collision)
    {
    }

    protected override void OnTriggerEnter(Collider other)
    {
    }

    public override void Reset()
    {
        base.Reset();
        triggered = false;
        timer = max_timer;
    }
}
