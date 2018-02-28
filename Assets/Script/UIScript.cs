using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIScript : MonoBehaviour
{

    private Player player;
    public GameObject pause_menu;
    public GameObject equip_menu;
    public Slider BOSShealthslider;
    public Slider healthslider;
    public Slider staminaslider;
    public Slider healthslidermini;
    public Slider staminaslidermini;
    public Slider weaponexp;
    public Slider armorexp;
    public Text goldText;
    public Text HealthText;
    public Text DamageText;
    public Text StaminaRegenSpdText;
    public Text DefenseText;
    public Text MovementSpdText;
    public Text AttackSpdText;
    public Text StaminaText;
    public Image instruct;
    public Text textObjective;
    public RawImage bloodscreen;
    public Text levels;
    bool stop;
    int level_data;//CHANGE ME if needed
    

    [SerializeField]
    private Text healthpercent = null, staminapercent = null;

    private void Awake()
    {
        if ((player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>()) == null) Debug.Log(this.GetType() + ".cs : Player not loaded");

        //if (!(BOSS = GameObject.FindGameObjectWithTag("BossMelee").GetComponent<BossMelee>())) Debug.Log(this.GetType() + ".cs : Boss not loaded");

    }

    // Use this for initialization
    void Start()
    {
        // level_data = 100;
        if (!healthpercent) Debug.Log(this.GetType() + ".cs : Health Text not linked!");
        if (!staminapercent) Debug.Log(this.GetType() + ".cs : Stamina Text not linked!");
        BOSShealthslider.gameObject.SetActive(false);
        healthslider.maxValue = player.GetpStats().MAXHEALTH;
        staminaslider.maxValue = player.GetpStats().MAXSTAMINA;
        healthslidermini.maxValue = player.GetpStats().MAXHEALTH;
        staminaslidermini.maxValue = player.GetpStats().MAXSTAMINA;
       //weaponexp.value = player.GetpStats().exp
        //level_data = (int)player.GetpStats().level;//ME TOO
        //levels.text = "Level  " + level_data;
        //levels.canvasRenderer.SetAlpha(1.0f);
        //levels.CrossFadeAlpha(0.0f, 2.5f, false);
        SetRoomLevelLayout();

        //sets to can see
        textObjective.text = "Explore";
        textObjective.canvasRenderer.SetAlpha(1.0f);
        textObjective.CrossFadeAlpha(0.0f, 2.5f, false);

        bloodscreen.canvasRenderer.SetAlpha(0.0f);
        stop = false;
        //fades away
       
       

    }
    public void SetRoomLevelLayout()
    {

        // level_data = 100;
        level_data = (int)player.GetpStats().level;//ME TOO
        levels.text = "Level  " + level_data;
        levels.canvasRenderer.SetAlpha(1.0f);
        levels.CrossFadeAlpha(0.0f, 2.5f, false);
    }
    
    public void SetRoomObjective(string objective)
    {
        textObjective.text = objective;
        textObjective.canvasRenderer.SetAlpha(1.0f);
        textObjective.CrossFadeAlpha(0.0f, 2.5f, false);
    }


    public void SpawnBossHP()
    {
        // BOSShealthslider.image.canvasRenderer.SetAlpha(1.0f);
        BOSShealthslider.gameObject.SetActive(true);
    }
    public void UpdateBossHP( float data)
    {
        BOSShealthslider.value = data;
    }
    public void BossMax(float data)
    {
        BOSShealthslider.maxValue = data;
    }


    private void FixedUpdate()
    {
        healthIndication();
        StaminaIndication();
        AttackSpeedIndication();
        MovementSpdIndication();
        damageIndication();
        DefenseIndication();
        StaminaRSPDIndication();
        healthbarmini();
        staminabarmini();
        healthbar();
        staminabar();
        goldIndication();
    }

    // Update is called once per frame
    void Update ()
    {
        if (Input.GetKey("l"))
        {
            instruct.enabled = true;
        }
        else
        {
            instruct.enabled = false;
        }
        if (player.GetpStats().health <= 0)
        {
            //  player.SetHealth(100.0f);
            SceneManager.LoadScene("gameover");
            player.Reset();
        }
        if (player.GetpStats().gothit == true)
        {
            bloodscreen.CrossFadeAlpha(1.0f, 0, false);
            //bloodscreen.canvasRenderer.SetAlpha(1.0f);
            stop = false;
        }
        else if(!stop)
        {
            bloodscreen.CrossFadeAlpha(0.0f, 2.5f, false);
            stop = true;
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pause_menu.SetActive(!pause_menu.activeSelf);

            if (pause_menu.activeSelf == true)
            {
                //Time.timeScale = 0;
            }
            else
            {
                //Time.timeScale = 1;
            }
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            if (equip_menu.activeSelf == false)
            {
                equip_menu.SetActive(true);
                //Time.timeScale = 0;
            }
            else
            {
                equip_menu.SetActive(false);
                //Time.timeScale = 1;
            }
        }
    }

    public void EQscreenup()
    {

        if (equip_menu.activeSelf == false)
        {
            equip_menu.SetActive(true);
            //Time.timeScale = 0;
        }
    }
    public void EQscreenclose()
    {
        if (equip_menu.activeSelf == true)
        {
            equip_menu.SetActive(false);
            //Time.timeScale = 1;
        }
    }

    public void Pause()
    {


        if (pause_menu.activeSelf == false)
        {
            pause_menu.SetActive(true);
            //Time.timeScale = 0;
        }


    }

    public void Resume()
    {
        if (pause_menu.activeSelf == true)
        {
            pause_menu.SetActive(false);
            //Time.timeScale = 1;
        }
    }

    public void Quit(int SceneIndex)
    {
        SceneManager.LoadSceneAsync(SceneIndex);
    }

    public void healthbar()
    {
        healthslider.value = player.GetpStats().health;
        healthpercent.text = Mathf.CeilToInt(player.GetpStats().health).ToString();
    }

    public void staminabar()
    {
        staminaslider.value = player.GetpStats().stamina;
        staminapercent.text = Mathf.CeilToInt(player.GetpStats().stamina).ToString();
    }

    public void healthbarmini()
    {
        healthslidermini.value = player.GetpStats().health;
        healthpercent.text = Mathf.CeilToInt(player.GetpStats().health).ToString();
    }

    public void staminabarmini()
    {
        staminaslidermini.value = player.GetpStats().stamina;
        staminapercent.text = Mathf.CeilToInt(player.GetpStats().stamina).ToString();
    }


    public void goldIndication()
    {
        goldText.text = "Gold : " + player.GetpStats().gold;
    }

    public void damageIndication()
    {
        DamageText.text = "Gold : " + player.GetpStats().damage;
    }

    public void healthIndication()
    {
        HealthText.text = "Health : " + player.GetpStats().health;
    }

    public void StaminaRSPDIndication()
    {
        StaminaRegenSpdText.text = "StaminaRegenSpd : " + player.GetpStats().staminaRegenSpd;
    }
    public void DefenseIndication()
    {
        DefenseText.text = "Defense : " + player.GetpStats().activeDefMultiplyer;
    }
    public void MovementSpdIndication()
    {
        MovementSpdText.text = "MovementSpd : " + player.GetpStats().moveSpd;
    }
    public void AttackSpeedIndication()
    {
        AttackSpdText.text = "AttackSpd : " + player.GetpStats().atkSpd;
    }

    public void StaminaIndication()
    {
        StaminaText.text = "Stamina : " + player.GetpStats().stamina;
    }

    //public void armorlevel()
    //{
    //    armorexp.value = ""
    //}

    //public void wepeaonlevel()
    //{
    //    armorexp.value = ""
    //}
}
