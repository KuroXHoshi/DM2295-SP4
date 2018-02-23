using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct PlayerStatistics
{
    public float level;
    public float health;
    public float stamina;
    public float atkSpd, moveSpd, healthRegenSpd, staminaRegenSpd;
    public float atkDist, dashDist, dashSpd;
    public int damage;
    public int gold;
}

public class Player : MonoBehaviour
{
    [SerializeField]
    private PlayerStatistics pStats;

    [SerializeField]
    private bool RegenSkill = false, IronWillSkill = false, EvasionSkill = false;

    [SerializeField]
    public float HealthRegenAmount = 1, StaminaRegenAmount = 1;

    [SerializeField]
    private ParticleSystem particle;

    private Animator anim;
    //private Rigidbody rb;
    private float RotaSpd = 10f;
    private int current_room;
    private bool set_prev;

    public AudioScript PlayerAudio;
    public JoyStick joystick;
    public GameObject button_attack;
    public GameObject button_defend;

    private Player() { }

    public int GetPlayerCurrentRoom() { return current_room; }
    public Vector3 Get_Player_Pos() { return transform.position; }
    public Animator GetAnim() { return anim; }
    public ParticleSystem GetParticle() { return particle; }
    public PlayerStatistics GetpStats() { return pStats; }
    public StateMachine sm { get; protected set; }
    public float MaxHealth { get; protected set; }
    public float MaxStamina { get; protected set; }
    public float GetRotaSpd() { return RotaSpd; }
    public float hitParticleDelay { get; set; }
    public float hpRegenDelay { get; set; }
    public float stamRegenDelay { get; set; }

    [SerializeField]

    public void TakeDamage(float _dmg) { pStats.health -= _dmg; }

    public void SetLevel(float level_input) {
        pStats.level = level_input;
    }

    public void SetGold(int gold_input)
    {
        pStats.gold = gold_input;
    }

    public void SetPlayerCurrentRoom(int _current_room)
    {
        current_room = _current_room;
    }

    private void Awake()
    {
        MaxHealth = pStats.health;
        MaxStamina = pStats.stamina;
        hitParticleDelay = 0f;
        set_prev = false;

        if (!(anim = gameObject.GetComponent<Animator>())) Debug.Log(this.GetType() + " : Animator Controller not Loaded!");
        //if (!(rb = gameObject.GetComponent<Rigidbody>())) Debug.Log(this.GetType() + " : Rigidbody component not Loaded!");
        
        if (sm == null)
            sm = new StateMachine();

        sm.AddState(new PlayerStates.Idle(this));
        sm.AddState(new PlayerStates.Movement(this));
        sm.AddState(new PlayerStates.Attack(this));
        sm.AddState(new Dash(this));
        sm.AddState(new Bash(this));

        if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor)
        {
            joystick.gameObject.SetActive(false);
            button_attack.SetActive(false);
            button_defend.SetActive(false);
        }
    }

    // Use this for initialization
    void Start()
    {
        RegenSkill = true;
       // Debug.Log(gameObject.GetHashCode());
    }

    private void FixedUpdate()
    {
        if (sm.HasStates())
            sm.Update();

        if (RegenSkill)
            PassiveRegen();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void LateUpdate()
    {
        if (sm.IsCurrentState("Idle") || sm.IsCurrentState("Movement"))
        {
            sm.SetNextState((joystick.Horizontal() == 0 && joystick.Vertical() == 0) ? "Idle" : "Movement");

            if (set_prev)
                set_prev = false;

            if (Input.GetButtonDown("Fire1") && (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer))
            {
                sm.SetNextState("Attack");
            }
            else if (Input.GetButtonDown("Skill1") && !sm.IsCurrentState("Dash") && (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor) && pStats.stamina > 0)
            {
                sm.SetNextState("Dash");
                pStats.stamina -= 1;
            }
        }

        if (hitParticleDelay > 0)
            hitParticleDelay -= Time.deltaTime;
    }

    void PassiveRegen()
    {
        
        if(sm.IsCurrentState("Idle"))
        {
            if (pStats.health < MaxHealth && hpRegenDelay <= 0f)
            {
                pStats.health += HealthRegenAmount;
                hpRegenDelay = pStats.healthRegenSpd;
            }

            hpRegenDelay -= Time.deltaTime;
        }

        if (pStats.stamina < MaxStamina && stamRegenDelay <= 0f)
        {
            pStats.stamina += StaminaRegenAmount;
            stamRegenDelay = pStats.staminaRegenSpd;
        }

        stamRegenDelay -= Time.deltaTime;
    }

    void PassiveIronSkin()
    {

    }

    void PassiveEvasion()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {      
        if(collision.gameObject.tag.Equals("Coin"))
        {
            pStats.gold += collision.gameObject.GetComponent<Gold>().GetGoldValue();
            Destroy(collision.gameObject);
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if ((collision.gameObject.tag.Equals("Wall") || collision.gameObject.tag.Equals("Door") || collision.gameObject.tag.Equals("Statue")) && sm.IsCurrentState("Dash"))  
        {
            anim.SetBool("dash", false);
            sm.SetNextState("Idle");
        }
    }

    public void SetPlayerState(string _state)
    {
        sm.SetNextState(_state);
    }
}
