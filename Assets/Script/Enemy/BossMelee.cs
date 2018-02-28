using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMelee : EnemyScript
{
    protected GameObject chosen_blessing;
    public GameObject[] blessing_list;
    public GameObject theUIcanvas;
    private UIScript uiScript;

    protected override void Awake()
    {
        base.Awake();

        enemyType = EnemyType.BOSS;
        
        starting_done = true;
        rigid_entity_body.detectCollisions = true;
        rigid_entity_body.useGravity = true;

        HP = MAX_HP;
    }

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
        //sm = new StateMachine();
        
        sm.AddState(new EnemyStates.Idle(this));
        sm.AddState(new EnemyStates.Movement(this));
        sm.AddState(new EnemyStates.Attack(this));

        int temp = Random.Range(0, blessing_list.Length);

        chosen_blessing = Instantiate(blessing_list[temp], transform.position, blessing_list[temp].transform.rotation);
        chosen_blessing.GetComponent<Blessing>().SetBlessingType(((Blessing.TYPE)temp + 5));
        chosen_blessing.SetActive(false);

        theUIcanvas = GameObject.FindGameObjectWithTag("UI");
        uiScript = theUIcanvas.GetComponent<UIScript>();
        uiScript.BossMax(MAX_HP);
        SpawnBossHPActive();
    }

    // Update is called once per frame
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        
        if (sm != null)
            sm.Update();

        uiScript.UpdateBossHP(HP);

        //float progress = Mathf.Clamp01(operation.progress / 0.9f);

        //slider.value = HP;
        //progresstext.text = HP * 100f + "%";
    }

    protected override void LateUpdate()
    {
        base.LateUpdate();

        if (HP <= 0)
        {
            chosen_blessing.SetActive(true);
            chosen_blessing.transform.position = new Vector3(transform.position.x, transform.position.y + 10, transform.position.z);
        }
    }

    public void SpawnBossHPActive()
    {
        uiScript.SpawnBossHP();
    }
}
