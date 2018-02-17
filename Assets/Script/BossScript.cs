using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossScript : MonoBehaviour {

    public Slider slider;
    public Text progresstext;

    public float HP = 1000;
    public float MAX_HP = 1000;
    float critical_HP = 100;
    public GameObject player; //: Transform;
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
        player = GameObject.FindGameObjectWithTag("Player");
        animator = GetComponent<Animator>();
        //transform.position = Player.transform.position - Vector3.forward * MoveSpeed;
    }
	
	// Update is called once per frame
	void Update () {
        Distance = Vector3.Distance(enemy_pos, player_pos);
        player_pos = player.transform.position;
        new_enemy_pos = transform.position + player_pos * MoveSpeed * Time.deltaTime;
        enemy_pos = transform.position;
        target_player_DIR = player_pos - enemy_pos;

        HP -= Time.deltaTime * 20;
        float step = rotSpd * Time.deltaTime;
        Vector3 newDir = Vector3.RotateTowards(transform.forward, target_player_DIR, step, 0.0f);

        if (Distance <= MinDist)//distance reachable, attack!!
        {
            //attack melee animation activate pls
            //NEAR_ATTACK = true;

            bool which_attack = (Random.value > 0.5f);

            transform.rotation = Quaternion.LookRotation(newDir);
            if (which_attack)
            {
                animator.SetBool("attack 1", true);
                animator.SetBool("attack 2", false);
            }
            else
            {
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
          
            if(HP<critical_HP)
            {
                transform.position += transform.forward * MoveSpeed*2 * Time.deltaTime;

                animator.SetBool("walk", false);
                animator.SetBool("Run", true);
            }
            else
            {
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

            animator.SetBool("walk", false);
            animator.SetBool("attack 1", false);

            animator.SetBool("attack 2", false);
            animator.SetBool("getting hit", false);
            animator.SetBool("Victory", false);
            animator.SetBool("Dies", false);
            animator.SetBool("Run", false);
        }

        //float progress = Mathf.Clamp01(operation.progress / 0.9f);

        //slider.value = HP;
        //progresstext.text = HP * 100f + "%";
    }

    public void Reset()
    {
        HP = MAX_HP;
        gameObject.SetActive(false);
    }
}


