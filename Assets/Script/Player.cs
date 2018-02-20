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
    private float Level = 1f, Health = 10f, Stamina = 5f, AtkSpd = 1f, MoveSpd = 10f, HealthRegenSpd = 0.5f, StaminaRegenSpd = 0.1f, AtkDist = 1.5f, DashDistance = 5f, DashSpd = 4f;
    [SerializeField]
    private int Damage = 1;

    [SerializeField]
    private bool RegenSkill = false, IronWillSkill = false, EvasionSkill = false;

    [SerializeField]
    private ParticleSystem particle;

    private Animator anim;
    private Rigidbody rb;
    private float RotaSpd = 10f;
    private float hitParticleDelay = 0f;
    private Vector3 prevPos = new Vector3();

    //public PlayerStatistic playerStat { get; set; }
    public PlayerState playerState { get; set; }
    public float MaxHealth { get; protected set; }
    public float MaxStamina { get; protected set; }

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
        MaxHealth = Health;
        MaxStamina = Stamina;
    }

    // Use this for initialization
    void Start()
    {
        if (!(anim = gameObject.GetComponent<Animator>()))
            Debug.Log("Player.cs : Animator Controller not Loaded!");

        if (!(rb = gameObject.GetComponent<Rigidbody>()))
            Debug.Log("Player.cs : Rigidbody component not Loaded!");
    }

    private void FixedUpdate()
    {
        ProcessStates();
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void LateUpdate()
    {
        //if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Attack") && playerState != PlayerState.NormalAttack && playerState != PlayerState.Dash)
        if (playerState == PlayerState.Idle || playerState == PlayerState.Movement)
        {
            playerState = (Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0) ? PlayerState.Idle : PlayerState.Movement;

            if (Input.GetMouseButtonDown(0))
                playerState = PlayerState.NormalAttack;
            else if (Input.GetMouseButtonDown(1) && playerState != PlayerState.Dash)
            {
                playerState = PlayerState.Dash;
                prevPos = transform.position;
            }
        }

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
                Dash();
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

    private void Dash()
    {
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Dash"))
        {
            anim.SetBool("dash", true);
        }

        transform.position += transform.forward * DashSpd * MoveSpd * Time.deltaTime;

        if (Vector3.Distance(prevPos, transform.position) > DashDistance)
        {
            anim.SetBool("dash", false);
            playerState = (anim.GetBool("attacking")) ? PlayerState.NormalAttack : PlayerState.Idle;
        }
    }

    void PassiveRegen()
    {

    }

    void PassiveIronSkin()
    {

    }

    void PassiveEvasion()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        if ((collision.gameObject.tag == "Wall" || collision.gameObject.tag == "Door") && playerState == PlayerState.Dash)
        {
            anim.SetBool("dash", false);
            playerState = PlayerState.Idle;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if ((collision.gameObject.tag == "Wall" || collision.gameObject.tag == "Door") && playerState == PlayerState.Dash)
        {
            anim.SetBool("dash", false);
            playerState = PlayerState.Idle;
        }
    }
}
