﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Boss02 : MonoBehaviour {

 //// Use this for initialization
 //   public new void Start()
 //   {
 //       base.Start();
 //       sm = new StateMachine();

 //       sm.AddState(new StateMage("Mage"));
 //   }

 //   // Update is called once per frame
 //   public new void Update()
 //   {
 //       base.Update();

 //       float step = rotSpd * Time.deltaTime;
 //       Vector3 newDir = Vector3.RotateTowards(transform.forward, target_player_DIR, step, 0.0f);
 //       if (HP > 0)
 //       {
 //           //   sm.Update();


 //           if (Distance <= MinDist)//distance reachable, attack!!
 //           {
 //               //attack melee animation activate pls
 //               //NEAR_ATTACK = true;

 //               bool which_attack = (Random.value > 0.5f);

 //               transform.rotation = Quaternion.LookRotation(newDir);
 //               if (which_attack)
 //               {
 //                   //sm.Update();
 //                   animator.SetBool("attack1", true);
 //                   animator.SetBool("attack2", false);
 //               }
 //               else
 //               {
 //                   // sm.Update();
 //                   animator.SetBool("attack1", false);
 //                   animator.SetBool("attack2", true);
 //               }


 //               animator.SetBool("walk", false);
 //               //animator.SetBool("idle", false);


 //               //animator.SetBool("getting hit", false);
 //              // animator.SetBool("Victory", false);
 //               animator.SetBool("die", false);
 //               animator.SetBool("run", false);

 //           }
 //           else if (Distance <= MaxDist)//Saw player and go to player
 //           {
 //               transform.rotation = Quaternion.LookRotation(newDir);

 //               if (HP < critical_HP)
 //               {
 //                   transform.position += transform.forward * MoveSpeed * 2 * Time.deltaTime;
 //                   //   sm.Update();
 //                   animator.SetBool("walk", false);
 //                   animator.SetBool("run", true);
 //               }
 //               else
 //               {
 //                   //   sm.Update();
 //                   transform.position += transform.forward * MoveSpeed * Time.deltaTime;
 //                   animator.SetBool("walk", true);
 //                   animator.SetBool("run", false);
 //               }


 //               animator.SetBool("idle", false);
 //               animator.SetBool("attack1", false);
 //               //Here Call any function U want Like Shoot at here or something
 //               animator.SetBool("attack2", false);
 //               //animator.SetBool("getting hit", false);
 //              // animator.SetBool("Victory", false);
 //               animator.SetBool("die", false);

 //           }
 //           else//player unseen
 //           {
 //               //animator.SetBool("idle", true);
 //               //    sm.Update();
 //               animator.SetBool("walk", false);
 //               animator.SetBool("attack1", false);

 //               animator.SetBool("attack2", false);
 //              // animator.SetBool("gettinghit", false);
 //             //  animator.SetBool("Victory", false);
 //               animator.SetBool("die", false);
 //               animator.SetBool("run", false);
 //           }
 //       }
 //       else
 //       {
 //           animator.SetBool("die", true);
 //       }
 //       //float progress = Mathf.Clamp01(operation.progress / 0.9f);

 //       //slider.value = HP;
 //       //progresstext.text = HP * 100f + "%";
 //   }
}