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
    public Slider healthslider;
    public Slider staminaslider;
    public Text goldText;
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
        if (!(player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>())) Debug.Log(this.GetType() + ".cs : Player not loaded");
    }

    // Use this for initialization
    void Start()
    {
        // level_data = 100;
        if (!healthpercent) Debug.Log(this.GetType() + ".cs : Health Text not linked!");
        if (!staminapercent) Debug.Log(this.GetType() + ".cs : Stamina Text not linked!");

        healthslider.maxValue = player.MaxHealth;
        staminaslider.maxValue = player.MaxStamina;

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
    private void FixedUpdate()
    {
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
        healthpercent.text = player.GetpStats().health.ToString();
    }

    public void staminabar()
    {
        staminaslider.value = player.GetpStats().stamina;
        staminapercent.text = player.GetpStats().stamina.ToString();
    }

    public void goldIndication()
    {
        goldText.text = "Gold : " + player.GetpStats().gold;
    }
}
