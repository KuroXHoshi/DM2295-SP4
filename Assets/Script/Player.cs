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
    public enum PlayerState
    {
        Idle,
        Movement,
        NormalAttack,
        Dash,
        Bash,
        TotalState
    }

    [SerializeField]
    private PlayerStatistics pStats;

    [SerializeField]
    private bool RegenSkill = false, IronWillSkill = false, EvasionSkill = false;

    [SerializeField]
    private ParticleSystem particle;

    private Animator anim;
    //private Rigidbody rb;
    private float RotaSpd = 10f;
    private int current_room;
    private bool set_prev;

    //public PlayerStatistic playerStat { get; set; }
    public PlayerState playerState { get; set; }
    public StateMachine sm { get; protected set; }
    public float MaxHealth { get; protected set; }
    public float MaxStamina { get; protected set; }
    public JoyStick joystick;
    public GameObject button_attack;
    public GameObject button_defend;

    private Player()
    {

    }

    public int GetPlayerCurrentRoom() { return current_room; }
    public Vector3 Get_Player_Pos() { return transform.position; }
    public float GetRotaSpd() { return RotaSpd; }
    public float hitParticleDelay { get; set; }
    public Animator GetAnim() { return anim; }
    public ParticleSystem GetParticle() { return particle; }
    public PlayerStatistics GetpStats() { return pStats; }

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

            if (Input.GetMouseButtonDown(0) && (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer))
            {
                sm.SetNextState("Attack");
            }
            else if (Input.GetMouseButtonDown(1) && playerState != PlayerState.Dash && (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor))
            {
                sm.SetNextState("Dash");
            }
        }

        if (hitParticleDelay > 0)
            hitParticleDelay -= Time.deltaTime;
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
