using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossScript : MonoBehaviour {

    public int HP = 1000;
    public GameObject Player; //: Transform;
                              // public Transform playerTransform;
    public int MoveSpeed = 2;
    public int MaxDist = 15;
    public int MinDist = 1;
    //public int HP = 10;
    public int DMG = 1;
    //public float movementSpd = 10;
    public bool NEAR_ATTACK = false;
    public float rotSpd = 10;

    Animator animator;
     
    Vector3 player_pos;
    Vector3 enemy_pos;
    Vector3 new_enemy_pos;
    Vector3 target_player_DIR;
    float Distance;
    // Use this for initialization
    void Start()
    {
        animator = GetComponent<Animator>();
        //transform.position = Player.transform.position - Vector3.forward * MoveSpeed;
    }
	
	// Update is called once per frame
	void Update () {
        Distance = Vector3.Distance(enemy_pos, player_pos);
        player_pos = Player.transform.position;
        new_enemy_pos = transform.position + Player.transform.position * MoveSpeed * Time.deltaTime;
        enemy_pos = transform.position;
        target_player_DIR = Player.transform.position - enemy_pos;


        if (Distance <= MinDist)//distance reachable,attack
        {
            //attack melee animation activate pls
            //NEAR_ATTACK = true;
            //animator.SetBool("attack", true);

            animator.SetBool("walk", false);
            //animator.SetBool("idle", false);

        }
        else if (Distance <= MaxDist)//Saw player and go to player
        {
            float step = rotSpd * Time.deltaTime;
            Vector3 newDir = Vector3.RotateTowards(transform.forward, target_player_DIR, step, 0.0f);
            transform.rotation = Quaternion.LookRotation(newDir);
            transform.position += transform.forward * MoveSpeed * Time.deltaTime;

            animator.SetBool("walk", true);

            //animator.SetBool("idle", false);
            //animator.SetBool("attack", false);
            //Here Call any function U want Like Shoot at here or something
        }
        else//player unseen
        {
            //animator.SetBool("idle", true);

            animator.SetBool("walk", false);
            //animator.SetBool("attack", false);
        }


    }
}


