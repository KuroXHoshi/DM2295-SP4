using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : PlayerSkills
{
    public enum PlayerState
    {
        Idle,
        Movement,
        NormalAttack,
        Dash,
        Bash,
        TotalState
    }

    public struct PlayerStatistic
    {
        public float level;
        public float health;
        public float stamina;
        public float atkspd;
        public float movespd;
        public float healthregenspd;
        public float staminaregenspd;
        public float atkdist;
    }
    
    [SerializeField]
    private float Level = 1, Health = 10, Stamina = 5, AtkSpd = 1, MoveSpd = 10, HealthRegenSpd = 0.5f, StaminaRegenSpd = 0.1f, AtkDist = 1.5f;
    [SerializeField]
    private int Damage = 1;

    [SerializeField]
    private bool RegenSkill = false, IronWillSkill = false, EvasionSkill = false;

    [SerializeField]
    private ParticleSystem particle;

    private Animator anim;
    private Rigidbody rb;
    private float RotaSpd = 10;
    private float hitParticleDelay = 0;

    //public PlayerStatistic playerStat { get; set; }
    public PlayerState playerState { get; set; }

    private Player()
    {
        
    }

    public Vector3 Get_Player_Pos()
    {
        return transform.position;
    }

    public float GetHealth()
    {
        return Health;
    }

    public float GetLevel()
    {
        return Level;
    }

    public float GetStamina()
    {
        return Stamina;
    }

    public int GetPlayerDamageOutput()
    {
        return Damage;
    }

    public void TakeDamage(float _dmg)
    {
        Health -= _dmg;
    }

    public void SetLevel(float level_input)
    {
        Level = level_input;
    }

    void Regeneration()
    {

    }

    private void Awake()
    {
        
    }

    // Use this for initialization
    void Start ()
    {
        if (!(anim = gameObject.GetComponent<Animator>()))
            Debug.Log("Player.cs : Animator Controller not Loaded!");

        if (!(rb = gameObject.GetComponent<Rigidbody>()))
            Debug.Log("Player.cs : Rigidbody component not Loaded!");
    }
	
	// Update is called once per frame
	void Update ()
    {
        ProcessStates();

        //Vector3 translation = new Vector3(0f, 0f, 0f);
        //if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        //{
        //    translation.Set(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        //    anim.SetBool("moving", (translation.x == 0 && translation.z == 0) ? false : true);
        //}

        //if (Input.GetMouseButtonDown(1))
        //    Dash(ref translation, ref Stamina);

        //Vector3 prevPos = transform.position;
        //transform.position += translation * MoveSpd * Time.deltaTime;
        //float step = RotaSpd * Time.deltaTime;
        //Vector3 newDir = Vector3.RotateTowards(transform.forward, transform.position - prevPos, step, 0.0f);
        //transform.rotation = Quaternion.LookRotation(newDir);

        // Attack using Left Click
        //anim.SetTrigger("attacking");

        // Checks if state transits to attack
        //if (!anim.IsInTransition(0) && anim.GetCurrentAnimatorStateInfo(0).IsName("Attack") && hitParticleDelay <= 0)
        //{
        //    Vector3 offset = new Vector3(transform.forward.x * AtkDist, transform.forward.y + 1, transform.forward.z * AtkDist);
        //    Instantiate(particle, transform.position + offset, Quaternion.identity);
        //    hitParticleDelay = 0.3f;
        //}
    }

    private void LateUpdate()
    {
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Attack") && playerState != PlayerState.NormalAttack)
            playerState = (Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0) ? PlayerState.Idle : PlayerState.Movement;

        if (Input.GetMouseButtonDown(0))
            playerState = PlayerState.NormalAttack;

        if (hitParticleDelay > 0)
            hitParticleDelay -= Time.deltaTime;
    }

    private void ProcessStates()
    {
        switch (playerState)
        {
            case PlayerState.Idle:
                Idle();
                break;

            case PlayerState.Movement:
                Movement();
                break;

            case PlayerState.NormalAttack:
                NormalAttack();
                break;

            case PlayerState.Dash:
                break;

            default:
                break;
        }
    }

    private void Idle()
    {
        anim.SetBool("moving", false);
    }

    private void Movement()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
            return;

        Vector3 prevPos = transform.position;
        transform.position += new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")) * MoveSpd * Time.deltaTime;
        float step = RotaSpd * Time.deltaTime;
        Vector3 newDir = Vector3.RotateTowards(transform.forward, transform.position - prevPos, step, 0.0f);
        transform.rotation = Quaternion.LookRotation(newDir);

        anim.SetBool("moving", true);
    }

    private void NormalAttack()
    {
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
            anim.SetBool("attacking", true);

        // Checks if state transits to attack
        if (!anim.IsInTransition(0) && anim.GetCurrentAnimatorStateInfo(0).IsName("Attack") && hitParticleDelay <= 0)
        {
            Vector3 offset = new Vector3(transform.forward.x * AtkDist, transform.forward.y + 1, transform.forward.z * AtkDist);
            Instantiate(particle, transform.position + offset, Quaternion.identity);
            hitParticleDelay = 0.3f;
            anim.SetBool("attacking", false);

            playerState = PlayerState.Idle;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        
    }

    private void OnCollisionStay(Collision collision)
    {

    }
}
