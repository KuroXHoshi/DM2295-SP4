using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AI_Boss03 : EnemyScript
{
    public Slider uiHealth;


    protected override void Awake()
    {
        base.Awake();

        starting_done = true;
        rigid_entity_body.detectCollisions = true;
        rigid_entity_body.useGravity = true;

        enemyType = EnemyType.BOSS;

        sm.AddState(new EnemyStates.Idle(this));
        sm.AddState(new EnemyStates.Movement(this));
    }

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
        //sm = new StateMachine();

        //sm.AddState(new EnemyStates.Idle());
    }

    // Update is called once per frame
    protected override void FixedUpdate()
    {
        base.FixedUpdate(); //nimabei

        //float step = rotSpd * Time.deltaTime;
        //Vector3 newDir = Vector3.RotateTowards(transform.forward, target_player_DIR, step, 0.0f);

        
        //float progress = Mathf.Clamp01(operation.progress / 0.9f);

        //slider.value = HP;
        //progresstext.text = HP * 100f + "%";
    }

    protected override void LateUpdate()
    {
        base.LateUpdate();
    }
}
