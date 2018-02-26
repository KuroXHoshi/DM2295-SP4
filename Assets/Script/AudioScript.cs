﻿using System.Collections;
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
    public AudioClip damage;
    public AudioClip damage2;
    // Use this for initialization
    void Start () {
        int temp;
    }
	
	// Update is called once per frame
	void Update () {
       // attackclip();
       // playerdash();
    }

    public void attackclip()
    {
        MusicSource.clip = Attack;

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
        MusicSource.clip = dash;
        MusicSource.Play();
    }
    public void collide()
    {
         
        MusicSource.Play();
    }
    public void takedamage()
    {
        int temp = UnityEngine.Random.Range(0, 10);
        Debug.Log(temp);
        if (temp < 5)
        {
            MusicSource.clip = damage;
            MusicSource.Play();
        }
        else
        {
            MusicSource.clip = damage2;
            MusicSource.Play();
        }
    }
}
