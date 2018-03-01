using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[System.Serializable]
public class PlayerStatisticsLevel
{
    public string name;
    public float level, max_level;
    public float exp_gain_timer , exp_gain_timer_max;

    public double exp, maxExp;

    public PlayerStatisticsLevel(string _name, float _level, float _max_level, float _exp, float _maxexp, float _timer = 0.0f)
    {
        name = _name;
        level = _level;
        max_level = _max_level;
        exp = _exp;
        maxExp = _maxexp;
        exp_gain_timer = _timer;
        exp_gain_timer_max = exp_gain_timer;
    }

    public void IncreaseExp(float _input)
    {
        if (level < max_level)
        {
            exp_gain_timer -= Time.deltaTime;

            if (exp_gain_timer <= 0)
            {
                exp += _input;
                exp_gain_timer = exp_gain_timer_max;
            }

            while (exp >= maxExp)
            {
                exp -= maxExp;
                maxExp = maxExp + (int)((double)maxExp * (8.0f / 100.0f));      //PLUS 8%
                level += 1;
            }

            if (level > max_level)
            {
                level = max_level;
                exp = maxExp;
            }

           // if(!name.Equals("Stamina"))
           //Debug.Log("STAT Name: " + name + " Level: " + level + " ExP: " + exp + " / " + maxExp + " TIMER: " + exp_gain_timer);
        }
    }
}

[System.Serializable]
public struct PlayerStatistics
{
    public float level;
    
    public float health;
    public float stamina;
    public float atkSpd, moveSpd, healthRegenSpd, staminaRegenSpd,
                 passiveHPRegenMultiplyer, passiveDefMultiplyer, passiveEvaMultiplyer, passiveDmgMultiplyer,
                 activeHPRegenMultiplyer, activeDefMultiplyer, activeDmgMultiplyer;

    public float atkDist, dashDist, dashSpd;
    public int damage;
    public int gold;



    public bool gothit;
    public float MAXHEALTH, MAXSTAMINA;
}

public class Player : MonoBehaviour
{
    #region Variables
    [SerializeField]
    private PlayerStatistics pStats;

    bool is_blocking;

    /*
     * 0 - SWORD
     * 1 - ARMOR
     * 2 - STAMINA
     * 3 - STRENGTH
     */
    private List<PlayerStatisticsLevel> pStatsLevel = new List<PlayerStatisticsLevel>(); 

    //[SerializeField]
    //private bool RegenSkill = false, IronWillSkill = false, EvasionSkill = false;

    [SerializeField]
    public float HealthRegenAmount = 1, StaminaRegenAmount = 1;

    [SerializeField]
    private ParticleSystem particle;

    [SerializeField]
    protected Transform model;

    [SerializeField]
    GameObject shield;

    private Animator anim;
    private Rigidbody rb;
    private float RotaSpd = 10f;
    private int current_room;
    private bool set_prev;

    public AudioScript PlayerAudio;
    public JoyStick joystick;
    public GameObject button_attack;
    public GameObject button_defend;
    public Swipe SwipeControls;

    private List<Action<Blessing>> skill_function_list = new List<Action<Blessing>>();

    private Blessing[] blessing_inven;

    public bool debugImmune = false;
    #endregion

    private Player() { }

    #region Getters/Setters
    public int GetPlayerCurrentRoom() { return current_room; }
    public Vector3 Get_Player_Pos() { return transform.position; }
    public Animator GetAnim() { return anim; }
    public ParticleSystem GetParticle() { return particle; }
    public PlayerStatistics GetpStats() { return pStats; }
    public PlayerStatisticsLevel GetpStatsLevel(int _input) { return pStatsLevel[_input]; }
    public StateMachine sm { get; protected set; }
    public float MaxHealth { get; protected set; }
    public float MaxStamina { get; protected set; }
    public float GetRotaSpd() { return RotaSpd; }
    public float hitParticleDelay { get; set; }
    public float hpRegenDelay { get; set; }
    public float stamRegenDelay { get; set; }
    #endregion

    public void TakeDamage(float _dmg, Vector3 enemy_pos)
    {
        if (debugImmune)
            return;
        
        if(is_blocking)
        {
            Vector3 target = new Vector3(transform.position.x, 0, transform.position.z) - new Vector3(enemy_pos.x, 0, enemy_pos.z);
            float Angle = Vector3.Angle(model.forward, target);

            if (Angle < 90f && Angle > -90f)
            {
                PlayerAudio.defend();
                pStatsLevel[3].IncreaseExp(2);
                pStats.stamina -= 1 + (_dmg * 0.3f);
                SetKnockBack(-target.normalized, (_dmg * 0.3f));
               // Debug.Log("DAMAGE BLOCKED");
                return;
            }
        }

        pStats.health -= _dmg * GetPlayerDefence();
        PlayerAudio.takedamage();
        pStats.gothit = true;
        pStatsLevel[1].IncreaseExp(2);
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
        pStats.MAXSTAMINA = MaxStamina;
        is_blocking = false;

        if (!(anim = gameObject.GetComponent<Animator>())) Debug.Log(this.GetType() + " : Animator Controller not Loaded!");
        if (!(rb = gameObject.GetComponent<Rigidbody>())) Debug.Log(this.GetType() + " : Rigidbody component not Loaded!");
        
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

        pStatsLevel.Add(new PlayerStatisticsLevel("Weapon", 0, 100, 0, 100));
        pStatsLevel.Add(new PlayerStatisticsLevel("Armor", 0, 50, 0, 100));
        pStatsLevel.Add(new PlayerStatisticsLevel("Stamina", 0, 100, 0, 100, 1));
        pStatsLevel.Add(new PlayerStatisticsLevel("Strength", 0, 100, 0, 100));
    }

    // Use this for initialization
    void Start()
    {
        blessing_inven[0] = new Blessing();
        blessing_inven[0].SetBlessingType(Blessing.TYPE.SMITE);   //SET BLESSING TYPE TO HEALING

        blessing_inven[1] = new Blessing();
        blessing_inven[1].SetBlessingType(Blessing.TYPE.SUMMON);   //SET BLESSING TYPE TO NONE

        // Debug.Log(gameObject.GetHashCode());
    }

    private void FixedUpdate()
    {
        if (sm.HasStates())
            sm.Update();

        if (pStats.stamina >= 0 && !sm.IsCurrentState("Attack"))
        {
            if (Input.GetButton("Fire2"))
            {
                is_blocking = true;
                rb.mass = 500;
                shield.SetActive(true);
                //Debug.Log("IS BLOCKING");

            }
            else
            {
                is_blocking = false;
                rb.mass = 1;
                shield.SetActive(false);
                //Debug.Log("IS NOT BLOCKING");
            }
        }
        else
        {
            if (is_blocking)
            {
                is_blocking = false;
                rb.mass = 1;
                shield.SetActive(false);
                //Debug.Log("IS NOT BLOCKING");
            }
        }

        for (int i = 0; i < blessing_inven.Length; ++i)
        {
            if (blessing_inven[i].GetBlessingType() != Blessing.TYPE.NONE)
            {
                skill_function_list[(int)blessing_inven[i].GetBlessingType()].DynamicInvoke(blessing_inven[i]);
            }
        }
        
        if (sm.IsCurrentState("Idle"))
        {
            if (pStats.health < pStats.MAXHEALTH)
            {
                if (hpRegenDelay <= 0f)
                {
                    pStats.health += HealthRegenAmount * pStats.passiveHPRegenMultiplyer;
                    hpRegenDelay = pStats.healthRegenSpd;

                    if (pStats.health > pStats.MAXHEALTH)
                    {
                        pStats.health = pStats.MAXHEALTH;
                    }

                }

                hpRegenDelay -= Time.deltaTime;
            }
            else
            {
                hpRegenDelay = pStats.healthRegenSpd;
            }

        }
     
        if (pStats.stamina < pStats.MAXSTAMINA)
        {
            if (stamRegenDelay <= 0f)
            {
                pStats.stamina += GetPlayerStaminaRegen();
                stamRegenDelay = pStats.staminaRegenSpd;

                if (pStats.stamina > pStats.MAXSTAMINA)
                {
                    pStats.stamina = pStats.MAXSTAMINA;
                }

            }

            stamRegenDelay -= Time.deltaTime;
        }
        else
        {
            stamRegenDelay = GetPlayerStaminaRegen();

            if (pStats.stamina > MaxStamina)
            {
                pStats.stamina = MaxStamina;
            }
        }

        if (hitParticleDelay > 0)
        {
            hitParticleDelay -= Time.deltaTime * GetPlayerAttackSpeed();
          //  Debug.Log(hitParticleDelay);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Slash))
        {
            debugImmune = !debugImmune;
        }

        if (sm.IsCurrentState("Idle") || sm.IsCurrentState("Movement"))
        {
            sm.SetNextState((joystick.Horizontal() == 0 && joystick.Vertical() == 0) ? "Idle" : ((is_blocking) ? "Idle" : "Movement"));

            if (set_prev)
                set_prev = false;

            if (Input.GetButtonDown("Fire1") && hitParticleDelay <= 0 && (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer))
            {
                PlayerAttack();
            }
        }

        pStats.passiveHPRegenMultiplyer = 0;
        pStats.passiveDefMultiplyer = 0;
        pStats.passiveEvaMultiplyer = 0;
        pStats.passiveDmgMultiplyer = 0;

        pStats.activeDefMultiplyer = 0;
        pStats.activeDmgMultiplyer = 0;
        pStats.activeHPRegenMultiplyer = 0;

        pStats.gothit = false;
    }

    #region Skills
    void PassiveRegen(Blessing _input)
    {
        pStats.passiveHPRegenMultiplyer++;
    }

    void PassiveIronSkin(Blessing _input)
    {
        pStats.passiveDefMultiplyer += 10;
    }

    void PassiveEvasion(Blessing _input)
    {
        pStats.passiveEvaMultiplyer += 10;
    }

    void ActiveSummon(Blessing _input)
    {
        if (pStats.stamina >= 5)
        {
            if(((Input.GetButtonDown("Skill_Use_Left" ) || SwipeControls.SwipeUp)  && blessing_inven[0].GetBlessingType() == _input.GetBlessingType() && _input.GetDuration() <= 0) ||
               ((Input.GetButtonDown("Skill_Use_Right")  ||  SwipeControls.SwipeDown) && blessing_inven[1].GetBlessingType() == _input.GetBlessingType() && _input.GetDuration() <= 0))
            {
                pStats.stamina -= 5;

                for (int i = 0; i < 3; ++i)
                {
                    GameObject obj = SpawnerManager.Instance.GetSkillEntityObjectFromPool("summon");
                    obj.GetComponent<SkillSummon>().SetParent(GetInstanceID());

                    obj.transform.position = new Vector3(transform.position.x + (new IntRange(-2, 2).Random), -5f, transform.position.z + (new IntRange(-2, 2).Random));
                }

                PlayerAudio.playerspell1();
                sm.SetNextState("Summon");

            }
        }
    }

    void ActiveSmite(Blessing _input)
    {
        if (pStats.stamina >= 5)
        {
            if (((Input.GetButtonDown("Skill_Use_Left") || SwipeControls.SwipeUp) && blessing_inven[0].GetBlessingType() == _input.GetBlessingType() && _input.GetDuration() <= 0) ||
                ((Input.GetButtonDown("Skill_Use_Right") || SwipeControls.SwipeDown) && blessing_inven[1].GetBlessingType() == _input.GetBlessingType() && _input.GetDuration() <= 0))
            {
                pStats.stamina -= 5;

                for(int i = 0; i < 3; ++i)
                {
                    GameObject obj = SpawnerManager.Instance.GetSkillEntityObjectFromPool("firebomb");
                    obj.GetComponent<SkillFireBomb>().SetParent(GetInstanceID());

                    obj.transform.position = new Vector3(transform.position.x + (new IntRange(-2, 2).Random), 0.05f, transform.position.z + (new IntRange(-2, 2).Random));
                }

                sm.SetNextState("Smite");

            }
        }
    }

    void ActiveBash(Blessing _input)
    {
        if (pStats.stamina >= 2)
        {
            if (((Input.GetButtonDown("Skill_Use_Left") || SwipeControls.SwipeUp) && blessing_inven[0].GetBlessingType() == _input.GetBlessingType()) ||
                ((Input.GetButtonDown("Skill_Use_Right") || SwipeControls.SwipeDown) && blessing_inven[1].GetBlessingType() == _input.GetBlessingType()))
            {
                List<List<GameObject>> list = SpawnerManager.Instance.GetAllEntity();

                foreach(List<GameObject> i in list)
                {
                    foreach(GameObject obj in i)
                    {
                        Vector3 target = new Vector3(transform.position.x, 0, transform.position.z) - new Vector3(obj.transform.position.x, 0, obj.transform.position.z);
                        float Angle = Vector3.Angle(model.forward, target);

                        if (Angle < 90f && Angle > -90f)
                        {
                            if(obj.GetComponent<EnemyScript>() != null)
                             obj.GetComponent<EnemyScript>().SetKnockBack(-target.normalized);    
                        }
                    }
                }

                pStats.stamina -= 2;
            }
        }
    }

    void ActiveDash(Blessing _input)
    {
        if (pStats.stamina >= 1 && !sm.IsCurrentState("Dash"))
        {
            if (((Input.GetButtonDown("Skill_Use_Left") || SwipeControls.SwipeUp) && blessing_inven[0].GetBlessingType() == _input.GetBlessingType()) ||
                ((Input.GetButtonDown("Skill_Use_Right") || SwipeControls.SwipeDown) && blessing_inven[1].GetBlessingType() == _input.GetBlessingType()))
            {
                //print("BLESSING TPYE: " + _input.GetBlessingType());
                //print("BLESSING SLOT 0: " + blessing_inven[0].GetBlessingType());
                //print("BLESSING SLOT 1: " + blessing_inven[1].GetBlessingType());

                sm.SetNextState("Dash");
                pStats.stamina -= 1;
            }
        }
    }

    void ActiveWarCry(Blessing _input)
    {
        if (pStats.stamina >= 5)
        {
            if (((Input.GetButtonDown("Skill_Use_Left") || SwipeControls.SwipeUp) && blessing_inven[0].GetBlessingType() == _input.GetBlessingType() && _input.GetDuration() <= 0) ||
                ((Input.GetButtonDown("Skill_Use_Right") || SwipeControls.SwipeDown) && blessing_inven[1].GetBlessingType() == _input.GetBlessingType() && _input.GetDuration() <= 0))
            {
                pStats.stamina -= 5;
                _input.SetDuration(10);

                PlayerAudio.Roar();
                sm.SetNextState("WarCry");

            }
        }

        if(_input.GetDuration() > 0)
        {
            pStats.passiveDmgMultiplyer += 30;
            pStats.passiveDefMultiplyer += 40;
        }
    }

    void ActiveUltDef(Blessing _input)
    {
        if (pStats.stamina >= 5)
        {
            if (((Input.GetButtonDown("Skill_Use_Left") || SwipeControls.SwipeUp) && blessing_inven[0].GetBlessingType() == _input.GetBlessingType() && _input.GetDuration() <= 0) ||
                ((Input.GetButtonDown("Skill_Use_Right") || SwipeControls.SwipeDown) && blessing_inven[1].GetBlessingType() == _input.GetBlessingType() && _input.GetDuration() <= 0))
            {
                pStats.stamina -= 10;
                _input.SetDuration(3);

                PlayerAudio.Ult();
                sm.SetNextState("UltDef");
            }
        }

        if (_input.GetDuration() > 0)
        {
            pStats.passiveDefMultiplyer += 100;
        }
    }
    #endregion

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

    public string GetBlessingName(int _input)
    {
        return blessing_inven[_input].GetBlessingType().ToString();
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

    public void SetKnockBack(Vector3 dir, float _strength)
    {
        float temp =  _strength;

        if (temp > 1)
            temp = 1;

        transform.position -= dir * temp;
    }

    public float GetPlayerDamage(bool get_data_only = false)
    {      
        float temp = pStats.passiveDmgMultiplyer + pStats.activeDmgMultiplyer + pStatsLevel[0].level;

        if (!get_data_only)
            pStatsLevel[0].IncreaseExp(2f);

        return pStats.damage * ((100 + temp) / 100);
    }

    public float GetPlayerAttackSpeed()
    {
        float temp = pStats.atkSpd + pStatsLevel[3].level;
        return pStats.atkSpd * ((50 + temp) / 50);  
    }

    public float GetPlayerDefence()
    {
        float temp = pStats.passiveDefMultiplyer + pStats.activeDefMultiplyer + pStatsLevel[1].level;

        if (temp > 100)
            temp = 100;

        return ((100 - temp) / 100);
    }

    public float GetPlayerSpeed(bool get_data_only = false)
    {
        if (!get_data_only)
            pStatsLevel[2].IncreaseExp(1f);       //STAMINA STAT
           
            return pStats.moveSpd * ((is_blocking) ? 0 : ((100 + pStatsLevel[2].level) / 100));
    }

    public void PlayerAttack()
    {
        if (hitParticleDelay <= 0)
        {
            sm.SetNextState("Attack");
            hitParticleDelay = pStats.atkSpd;
        }
    }

    public float GetPlayerStaminaRegen()
    {
        return StaminaRegenAmount * ((100 + pStatsLevel[2].level) / 100);
    }

    public void PlayerDefendPointerDown()
    {
        //Debug.Log("Down");
        if (pStats.stamina > 0 && !sm.IsCurrentState("Attack"))
        {
            is_blocking = true;
            rb.mass = 500;
            shield.SetActive(true);
        }
    }

    public void PlayerDefendPointerUp()
    {
        //Debug.Log("Up");
        is_blocking = false;
        rb.mass = 1;
        shield.SetActive(false);
    }

    public void Reset()
    {
        pStatsLevel = new List<PlayerStatisticsLevel>();
        pStatsLevel.Add(new PlayerStatisticsLevel("Weapon", 0, 100, 0, 100));
        pStatsLevel.Add(new PlayerStatisticsLevel("Armor", 0, 50, 0, 100));
        pStatsLevel.Add(new PlayerStatisticsLevel("Stamina", 0, 100, 0, 100, 1));
        pStatsLevel.Add(new PlayerStatisticsLevel("Strength", 0, 100, 0, 100));

        blessing_inven[0] = new Blessing();
        blessing_inven[0].SetBlessingType(Blessing.TYPE.REGEN);   //SET BLESSING TYPE TO HEALING

        blessing_inven[1] = new Blessing();
        blessing_inven[1].SetBlessingType(Blessing.TYPE.NONE);   //SET BLESSING TYPE TO NONE
    }
}
