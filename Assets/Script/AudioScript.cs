using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioScript : MonoBehaviour {
    public AudioClip MusicClip;
    public Slider Volume;
    public AudioSource MusicSource;
    public AudioClip Attack;
    public AudioClip Defend;
    public AudioClip PlayerMovement;
    public AudioClip EnemyMovement;
    public AudioClip EnemySpell;
    public AudioClip PlayerSpell;
    public AudioClip dash;
    public AudioClip Collided;
    // Use this for initialization
    void Start () {
      
    }
	
	// Update is called once per frame
	void Update () {
       
	}

    public void attackclip()
    {
       

        MusicSource.Play();
    }
    public void defend()
    {
        


        MusicSource.Play();
    }
    public void playermove()
    {
        

        MusicSource.Play();
    }
    public void enemymove()
    {
       

        MusicSource.Play();
    }
    public void enemyspell()
    {
        

        MusicSource.Play();
    }
    public void playerspell()
    {
        

        MusicSource.Play();
    }
    public void playerdash()
    {
        MusicSource.Play();
    }
    public void collide()
    {
        MusicSource.Play();
    }
}
