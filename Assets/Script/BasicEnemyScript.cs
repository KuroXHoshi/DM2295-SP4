using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BasicEnemyScript : MonoBehaviour
{
    
    private Player player; //: Transform;
   // public Transform playerTransform;
    public float MoveSpeed = 2;
    public float MaxDist = 15;
    public float MinDist = 1.5f;
    public float HP = 10;
    public float MAX_HP = 10;
    public int DMG = 1;
    //public float movementSpd = 10;
    public bool NEAR_ATTACK = false;
    public float rotSpd = 10;
    public IntRange coin_range = new IntRange(10, 100);

    private bool starting_done;
    private int gold = 0;

    public GameObject gold_pile;

    private Rigidbody rigid_entity_body;

    Animator animator;

    Vector3 player_pos;
    Vector3 enemy_pos;
    Vector3 new_enemy_pos;
    Vector3 target_player_DIR;
    float Distance;

    //[Header("Unity Stuff")]
    public Image health;
    //health = starthealth;
    //when take damage (take damage function)
    //healthBar.fillAmount = health / starthealth ;

    [SerializeField]
    private Transform model;

    // Use this for initialization
    void Start()
    {
        if (!(player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>()))
            Debug.Log("BasicEnemyScript.cs : Player not loaded");

        //if (!(camera = GameObject.FindGameObjectWithTag("MainCamera").transform))
            //Debug.Log("BasicEnemyScript.cs : Camera not loaded");

        animator = model.GetComponent<Animator>();
        //transform.position = Player.transform.position - Vector3.forward * MoveSpeed;
        starting_done = false;

        rigid_entity_body = GetComponent<Rigidbody>();

        rigid_entity_body.detectCollisions = false;
        rigid_entity_body.useGravity = false;

        gold = coin_range.Random;

       // HP = MAX_HP;
    }
    
    // Update is called once per frame
    void Update()
    {
        if (!starting_done)
        {
            if (transform.position.y >= 1)
            {
                starting_done = true;
                //transform.position = new Vector3(transform.position.x, 1, transform.position.z);
                rigid_entity_body.detectCollisions = true;
                rigid_entity_body.useGravity = true;
            }

            float temp = UnityEngine.Random.Range(.001f, .2f);
            //transform.position =  new Vector3(transform.position.x, transform.position.y + temp, transform.position.z);
            transform.Translate(transform.up * temp);
        }
        else
        {
            if (gameObject.tag == "Skele_Medium")
                return;

            player_pos = player.Get_Player_Pos();
            enemy_pos = transform.position;
            Distance = Vector3.Distance(enemy_pos, player_pos);
            new_enemy_pos = transform.position + player_pos * MoveSpeed * Time.deltaTime;
            target_player_DIR = player_pos - enemy_pos;

            //health.fillAmount = HP / MAX_HP;

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
                //transform.position += transform.forward * MoveSpeed * Time.deltaTime;
                transform.Translate(model.forward * MoveSpeed * Time.deltaTime);
                float step = rotSpd * Time.deltaTime;
                Vector3 newDir = Vector3.RotateTowards(model.forward, target_player_DIR, step, 0.0f);
                model.rotation = Quaternion.LookRotation(newDir);

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

    private void LateUpdate()
    {
        // If enemy HP reaches 0 set gameObject to false
        if (HP <= 0)
        {
            GameObject obj = Instantiate(gold_pile, transform.position, gold_pile.transform.rotation);
            obj.GetComponent<Gold>().SetGoldValue(gold);
          
            transform.position = new Vector3(transform.position.x, -5f, transform.position.z);
            Reset();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("BasicEnemyScript.cs : Enemy got hit! <" + gameObject.GetHashCode() + ">");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player Hitting")
        {
            HP -= player.GetpStats().damage;
            health.fillAmount = HP / MAX_HP;
        }
    }




    public void Reset()
    {
        starting_done = false;
        rigid_entity_body.detectCollisions = false;
        rigid_entity_body.useGravity = false;
        HP = MAX_HP;
        health.fillAmount = HP / MAX_HP;
        gameObject.SetActive(false);
    }
}