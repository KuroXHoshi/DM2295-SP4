﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMelee : BossScript
{
    public GameObject[] blessing_list;

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
        sm = new StateMachine();
        
        sm.AddState(new StateMelee("Melee"));
        int temp = Random.Range(0, blessing_list.Length);

        chosen_blessing = Instantiate(blessing_list[temp], transform.position, blessing_list[temp].transform.rotation);
        chosen_blessing.GetComponent<Blessing>().SetBlessingType(((Blessing.TYPE)temp + 5));
        chosen_blessing.SetActive(false);
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        float step = rotSpd * Time.deltaTime;
        Vector3 newDir = Vector3.RotateTowards(transform.forward, target_player_DIR, step, 0.0f);
        if (HP > 0)
        {
            //   sm.Update();


            if (Distance <= MinDist)//distance reachable, attack!!
            {
                //attack melee animation activate pls
                //NEAR_ATTACK = true;

                bool which_attack = (Random.value > 0.5f);

                transform.rotation = Quaternion.LookRotation(newDir);
                if (which_attack)
                {
                    //sm.Update();
                    animator.SetBool("attack 1", true);
                    animator.SetBool("attack 2", false);
                }
                else
                {
                    // sm.Update();
                    animator.SetBool("attack 1", false);
                    animator.SetBool("attack 2", true);
                }


                animator.SetBool("walk", false);
                //animator.SetBool("idle", false);


                animator.SetBool("getting hit", false);
                animator.SetBool("Victory", false);
                animator.SetBool("Dies", false);
                animator.SetBool("Run", false);

            }
            else if (Distance <= MaxDist)//Saw player and go to player
            {
                transform.rotation = Quaternion.LookRotation(newDir);

                if (HP < critical_HP)
                {
                    transform.position += transform.forward * MoveSpeed * 2 * Time.deltaTime;
                    //   sm.Update();
                    animator.SetBool("walk", false);
                    animator.SetBool("Run", true);
                }
                else
                {
                    //   sm.Update();
                    transform.position += transform.forward * MoveSpeed * Time.deltaTime;
                    animator.SetBool("walk", true);
                    animator.SetBool("Run", false);
                }


                animator.SetBool("idle", false);
                animator.SetBool("attack 1", false);
                //Here Call any function U want Like Shoot at here or something
                animator.SetBool("attack 2", false);
                animator.SetBool("getting hit", false);
                animator.SetBool("Victory", false);
                animator.SetBool("Dies", false);

            }
            else//player unseen
            {
                //animator.SetBool("idle", true);
                //    sm.Update();
                animator.SetBool("walk", false);
                animator.SetBool("attack 1", false);

                animator.SetBool("attack 2", false);
                animator.SetBool("getting hit", false);
                animator.SetBool("Victory", false);
                animator.SetBool("Dies", false);
                animator.SetBool("Run", false);
            }
        }
        else
        {
            animator.SetBool("Dies", true);
        }
        //float progress = Mathf.Clamp01(operation.progress / 0.9f);

        //slider.value = HP;
        //progresstext.text = HP * 100f + "%";
    }

    protected void LateUpdate()
    {
        if(HP <= 0)
        {
            chosen_blessing.SetActive(true);
            chosen_blessing.transform.position = new Vector3(transform.position.x, transform.position.y + 10, transform.position.z);

            base.Reset();
        }
    }
}