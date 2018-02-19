using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioScript : MonoBehaviour {
    public AudioClip MusicClip;
    public Slider Volume;
    public AudioSource MusicSource;
    public AudioSource Attack;
    public AudioSource Defend;
    public AudioSource PlayerMovement;
    public AudioSource EnemyMovement;
    public AudioSource EnemySpell;
    public AudioSource PlayerSpell;
    // Use this for initialization
    void Start () {
        MusicSource.clip = MusicClip;
        MusicSource = GetComponent<AudioSource>();
        //main menu only
        MusicSource.Play();
    }
	
	// Update is called once per frame
	void Update () {
        MusicSource.volume = Volume.value;
	}

    public void attackclip()
    {
        Attack.Play();
    }
    public void defend()
    {
        Defend.Play();
    }
    public void playermove()
    {
        PlayerMovement.Play();
    }
    public void enemymove()
    {
        EnemyMovement.Play();
    }
    public void enemyspell()
    {
       EnemySpell.Play();
    }
    public void playerspell()
    {
        PlayerSpell.Play();
    }
}
