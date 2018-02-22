﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIScript : MonoBehaviour {

    private Player player;
    public GameObject pause_menu;
    public GameObject equip_menu;
    public Slider healthslider;
    public Slider staminaslider;

    [SerializeField]
    private Text healthpercent = null, staminapercent = null;

    // Use this for initialization
    void Start () {
		if(!(player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>())) Debug.Log(this.GetType() + ".cs : Player not loaded");
        if (!healthpercent) Debug.Log(this.GetType() + ".cs : Health Text not linked!");
        if (!staminapercent) Debug.Log(this.GetType() + ".cs : Stamina Text not linked!");

        healthslider.maxValue = player.MaxHealth;
        staminaslider.maxValue = player.MaxStamina;
	}

    private void FixedUpdate()
    {
        healthbar();
        staminabar();
    }

    // Update is called once per frame
    void Update () {
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
}
