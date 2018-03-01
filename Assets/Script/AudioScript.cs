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
    public AudioClip damage;
    public AudioClip damage2;
    public AudioClip gameover;
    public AudioClip warcry;
    public AudioClip Ulti;
    // Use this for initialization
    void Start () {
       // int temp;
        MusicSource.clip = MusicClip;
        MusicSource.Play();
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


        MusicSource.clip = Defend;
        MusicSource.Play();
    }
    public void playermove()
    {

        MusicSource.clip = PlayerMovement;
        MusicSource.Play();
    }
    public void enemymove()
    {

        MusicSource.clip = EnemyMovement;
        MusicSource.Play();
    }
    public void enemyspell()
    {

        MusicSource.clip = EnemySpell;
        MusicSource.Play();
    }
    public void playerspell1()
    {

        MusicSource.clip = PlayerSpell;
        MusicSource.Play();
    }
    public void playerdash()
    {
        MusicSource.clip = dash;
        MusicSource.Play();
    }
    public void collide()
    {
        MusicSource.clip = Collided;
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
    public void GO()
    {
        MusicSource.clip = gameover;
        MusicSource.Play();
    }
    public void Ult()
    {
        MusicSource.clip = Ulti;
        MusicSource.Play();
    }
    public void Roar()
    {
        MusicSource.clip = warcry;
        MusicSource.Play();
    }
}
