using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyScript : MonoBehaviour
{
    
    protected Player player; //: Transform;
   // public Transform playerTransform;
    public float MoveSpeed = 2;
    public float MaxDist = 15;
    public float MinDist = 1.5f;
    public float HP = 10;
    public float MAX_HP = 10;
    public int DMG = 1;
    private float dmgMultiplier = 1f;
    //public float movementSpd = 10;
    public bool NEAR_ATTACK = false;
    public float rotSpd = 10;
    public IntRange coin_range = new IntRange(10, 100);

    protected bool starting_done;
    private int gold = 0;
    protected bool pathFind = false;

    public GameObject gold_pile;
    public ParticleSystem particle;

    protected Rigidbody rigid_entity_body;

    Animator animator;

    //[Header("Unity Stuff")]
    public Image health;
    //health = starthealth;
    //when take damage (take damage function)
    //healthBar.fillAmount = health / starthealth ;

    [SerializeField]
    protected Transform model;

    public StateMachine sm { get; protected set; }
    public Animator GetAnim() { return animator; }
    public Player GetPlayer() { return player; }
    public Vector3 GetPlayerPos() { return player.transform.position; }
    public Transform GetModel() { return model; }
    public void SetPathFind(bool _set) { pathFind = _set; }

    protected virtual void Awake()
    {
        animator = model.GetComponent<Animator>();
        rigid_entity_body = GetComponent<Rigidbody>();

        if (sm == null)
            sm = new StateMachine();

        starting_done = false;
        rigid_entity_body.detectCollisions = false;
        rigid_entity_body.useGravity = false;
    }

    // Use this for initialization
    protected virtual void Start()
    {
        if (!(player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>()))
            Debug.Log(this.GetType() + ".cs : Player not loaded");

        //if (!(camera = GameObject.FindGameObjectWithTag("MainCamera").transform))
        //Debug.Log(this.GetType() + ".cs : Camera not loaded");

        //transform.position = Player.transform.position - Vector3.forward * MoveSpeed;
        gold = coin_range.Random;
    }

    protected virtual void FixedUpdate()
    {
    }

    protected virtual void LateUpdate()
    {
        // If enemy HP reaches 0 set gameObject to false
        if (HP <= 0)
        {
            GameObject obj = Instantiate(gold_pile, new Vector3(transform.position.x, 0.05f, transform.position.z), gold_pile.transform.rotation);
            obj.GetComponent<Gold>().SetGoldValue(gold);
          
            transform.position = new Vector3(transform.position.x, -5f, transform.position.z);
            Reset();
        }
    }

    protected virtual void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("BasicEnemyScript.cs : Enemy got hit! <" + gameObject.GetHashCode() + ">");
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player Hitting")
        {
            OnAttacked(player.GetPlayerDamage());
        }
    }

    public virtual void OnAttacked(float _damage)
    {
        HP -= _damage;
        if (health != null)
        {
            health.gameObject.SetActive(true);
            health.fillAmount = HP / MAX_HP;
        }
    } 
    
    public void Reset()
    {
        starting_done = false;
        rigid_entity_body.detectCollisions = false;
        rigid_entity_body.useGravity = false;
        HP = MAX_HP;
        if (health != null)
        {
            health.fillAmount = HP / MAX_HP;
            health.gameObject.SetActive(false);
        }
        gameObject.SetActive(false);
    }
}