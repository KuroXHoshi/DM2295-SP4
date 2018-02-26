using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[System.Serializable]
public struct PlayerStatistics
{
    public float level;
    public float health;
    public float stamina;
    public float atkSpd, moveSpd, healthRegenSpd, staminaRegenSpd,
                 HPRegenMultiplyer, DefMultiplyer, EvaMultiplyer;

    public float atkDist, dashDist, dashSpd;
    public int damage;
    public int gold;

    public bool gothit;
    public float MAXHEALTH;
}

public class Player : MonoBehaviour
{
    [SerializeField]
    private PlayerStatistics pStats;

    //[SerializeField]
    //private bool RegenSkill = false, IronWillSkill = false, EvasionSkill = false;

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

    private List<Action> skill_function_list = new List<Action>();

    private Blessing[] blessing_inven;

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

    public void TakeDamage(float _dmg)
    {
        pStats.health -= _dmg;
        PlayerAudio.takedamage();
        pStats.gothit = true;
    }

    public void SetLevel(float level_input) {
        pStats.level = level_input;
    }

    public void SetHealth(float hp)
    {
        pStats.health = hp;
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
        pStats.MAXHEALTH = MaxHealth;

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

        skill_function_list.Add(PassiveRegen);
        skill_function_list.Add(PassiveIronSkin);
        skill_function_list.Add(PassiveEvasion);

        skill_function_list.Add(ActiveSummon);
        skill_function_list.Add(ActiveSmite);
        skill_function_list.Add(ActiveBash);
        skill_function_list.Add(ActiveDash);
        skill_function_list.Add(ActiveWarCry);
        skill_function_list.Add(ActiveUltDef);

        blessing_inven = new Blessing[2];
    }

    // Use this for initialization
    void Start()
    {
        blessing_inven[0] = new Blessing();
        blessing_inven[0].SetBlessingType(Blessing.TYPE.REGEN);   //SET BLESSING TYPE TO HEALING

        blessing_inven[1] = new Blessing();
        blessing_inven[1].SetBlessingType(Blessing.TYPE.NONE);   //SET BLESSING TYPE TO EMPTY


        // Debug.Log(gameObject.GetHashCode());
    }

    private void FixedUpdate()
    {
        if (sm.HasStates())
            sm.Update();

        for (int i = 0; i < blessing_inven.Length; ++i)
        {
            if (blessing_inven[i].GetBlessingType() != Blessing.TYPE.NONE)
            {
                skill_function_list[(int)blessing_inven[i].GetBlessingType()].DynamicInvoke();
            }
        }

        if (sm.IsCurrentState("Idle"))
        {
            if (pStats.health < MaxHealth && hpRegenDelay <= 0f)
            {
                pStats.health += HealthRegenAmount * pStats.HPRegenMultiplyer;
                hpRegenDelay = pStats.healthRegenSpd;
                pStats.gothit = false;
            }

            hpRegenDelay -= Time.deltaTime;

            if (pStats.health > MaxHealth)
            {
                pStats.health = MaxHealth;
            }
        }

        if (pStats.stamina < MaxStamina && stamRegenDelay <= 0f)
        {
            pStats.stamina += StaminaRegenAmount;
            stamRegenDelay = pStats.staminaRegenSpd;
        }

        stamRegenDelay -= Time.deltaTime;
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
                skill_function_list[6].DynamicInvoke();
            }
        }

        if (hitParticleDelay > 0)
            hitParticleDelay -= Time.deltaTime;

        pStats.HPRegenMultiplyer = 0;
        pStats.DefMultiplyer = 0;
        pStats.EvaMultiplyer = 0;
    }

    void PassiveRegen()
    {
        pStats.HPRegenMultiplyer++;
    }

    void PassiveIronSkin()
    {
        pStats.DefMultiplyer++;
    }

    void PassiveEvasion()
    {
        pStats.EvaMultiplyer++;
    }

    void ActiveSummon()
    {

    }

    void ActiveSmite()
    {

    }

    void ActiveBash()
    {

    }

    void ActiveDash()
    {
        sm.SetNextState("Dash");
        pStats.stamina -= 1;
    }

    void ActiveWarCry()
    {

    }

    void ActiveUltDef()
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

    private void OnTriggerStay(Collider collision)
    {
        if (collision.gameObject.tag.Equals("Blessing"))
        {
            if (Input.GetButtonDown("Skill_Select_Left"))
            {
                SetPlayerBlessing(true, collision.gameObject.GetComponent<Blessing>());
                Destroy(collision.gameObject);
            }
            else if (Input.GetButtonDown("Skill_Select_Right"))
            {
                SetPlayerBlessing(false, collision.gameObject.GetComponent<Blessing>());
                Destroy(collision.gameObject);
            }
        }
    }

    public void SetPlayerState(string _state)
    {
        sm.SetNextState(_state);
    }

    public void SetPlayerBlessing(bool set_first_slot, Blessing _input)
    {
        if (set_first_slot)
            blessing_inven[0] = _input;
        else
            blessing_inven[1] = _input;
    }
}
