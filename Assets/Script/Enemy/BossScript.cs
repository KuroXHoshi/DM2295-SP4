using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossScript : MonoBehaviour
{
    public Slider slider;
    public Text progresstext;

    public float HP = 1000;
    public float MAX_HP = 1000;
    protected float critical_HP = 100;
    public GameObject player; 
                             
    public int MoveSpeed = 2;
    public int MaxDist = 15;
    public int MinDist = 1;
    public int DMG = 1;
    public bool NEAR_ATTACK = false;
    public float rotSpd = 10;

    [SerializeField]
    protected Transform model;
    protected Animator animator;
    protected StateMachine sm;

    protected Vector3 player_pos;
    protected Vector3 enemy_pos;
    protected Vector3 new_enemy_pos;
    protected Vector3 target_player_DIR;
    protected float Distance;

    public GameObject theUIcanvas;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // Use this for initialization
    public void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        animator = GetComponent<Animator>();
        theUIcanvas.GetComponent<UIScript>().BossMax(MAX_HP);
    }

    // Update is called once per frame
    public void Update () {
        Distance = Vector3.Distance(enemy_pos, player_pos);
        player_pos = player.transform.position;
        new_enemy_pos = transform.position + player_pos * MoveSpeed * Time.deltaTime;
        enemy_pos = transform.position;
        target_player_DIR = player_pos - enemy_pos;

        HP -= Time.deltaTime * 20;
        theUIcanvas.GetComponent<UIScript>().UpdateBossHP(HP);
    }

    public void SpawnBossHPActive()
    {
        theUIcanvas.GetComponent<UIScript>().SpawnBossHP();
    }
    public void Reset()
    {
        HP = MAX_HP;
        gameObject.SetActive(false);
    }
}


