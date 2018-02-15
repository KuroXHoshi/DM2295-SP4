using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemyScript : MonoBehaviour
{
    private Player player; //: Transform;
   // public Transform playerTransform;
    public int MoveSpeed = 2;
    public int MaxDist = 15;
    public int MinDist = 1;
    public int HP = 10;
    public int DMG = 1;
    //public float movementSpd = 10;
    public bool NEAR_ATTACK = false;
    public float rotSpd = 10;
    private bool starting_done;

    private Rigidbody rigid_entity_body;

    Animator animator;

    Vector3 player_pos;
    Vector3 enemy_pos;
    Vector3 new_enemy_pos;
    Vector3 target_player_DIR;
    float Distance;
    // Use this for initialization
    void Start()
    {
        if (!(player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>()))
            Debug.Log("BasicEnemyScript.cs : Player not loaded");

        animator = GetComponent<Animator>();
        //transform.position = Player.transform.position - Vector3.forward * MoveSpeed;
        starting_done = false;

        rigid_entity_body = GetComponent<Rigidbody>();

        rigid_entity_body.detectCollisions = false;
        rigid_entity_body.useGravity = false;
    }
    
    // Update is called once per frame
    void Update()
    {

        if (!starting_done)
        {
            if (transform.position.y >= 0)
            {
                starting_done = true;
                transform.position = new Vector3(transform.position.x, 0.05f, transform.position.z);
                rigid_entity_body.detectCollisions = true;
                rigid_entity_body.useGravity = true;
            }

            float temp = UnityEngine.Random.Range(.001f, .2f);
            transform.position =  new Vector3(transform.position.x, transform.position.y + temp, transform.position.z);
        }
        else
        {
            player_pos = player.Get_Player_Pos();
            Distance = Vector3.Distance(enemy_pos, player_pos);
            new_enemy_pos = transform.position + player_pos * MoveSpeed * Time.deltaTime;
            enemy_pos = transform.position;
            target_player_DIR = player_pos - enemy_pos;


            if (Distance <= MinDist)//distance reachable,attack
            {
                //attack melee animation activate pls
                //NEAR_ATTACK = true;
                animator.SetBool("attack", true);

                animator.SetBool("walk", false);
                animator.SetBool("idle", false);

            }
            else if (Distance <= MaxDist)//Saw player and go to player
            {
                transform.position += transform.forward * MoveSpeed * Time.deltaTime;
                float step = rotSpd * Time.deltaTime;
                Vector3 newDir = Vector3.RotateTowards(transform.forward, target_player_DIR, step, 0.0f);
                transform.rotation = Quaternion.LookRotation(newDir);

                animator.SetBool("walk", true);

                animator.SetBool("idle", false);
                animator.SetBool("attack", false);
                //Here Call any function U want Like Shoot at here or something
            }
            else//player unseen
            {
                animator.SetBool("idle", true);

                animator.SetBool("walk", false);
                animator.SetBool("attack", false);
            }

        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("BasicEnemyScript.cs : Enemy got hit! <" + gameObject.GetHashCode() + ">");
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("BasicEnemyScript.cs : Hash<" + gameObject.GetHashCode() + "> Enemy HP is " + HP);
        if (other.tag == "Player Hitting")
        {
            HP -= player.GetPlayerDamageOutput();
            Debug.Log("BasicEnemyScript.cs : Enemy HashCode<" + gameObject.GetHashCode() + "> HP is " + HP);
            //Debug.Log("BasicEnemyScript.cs : Enemy got hit! <" + gameObject.GetHashCode() + ">");
        }
    }
}