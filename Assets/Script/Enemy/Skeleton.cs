using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : EnemyScript
{
    
    protected override void Awake()
    {
        base.Awake();

        enemyType = EnemyType.NORMAL;

        sm.AddState(new EnemyStates.Idle(this));
        sm.AddState(new EnemyStates.Movement(this));
        sm.AddState(new EnemyStates.Attack(this));
    }

    // Use this for initialization
    protected override void Start()
    {
        base.Start();

        //StartCoroutine(UpdatePath());
        //UpdatePath();

        HP = MAX_HP;

        if (health != null)
            health.gameObject.SetActive(false);
    }

    // Update is called once per frame
    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if (!starting_done)
        {
            if (transform.position.y >= 1)
            {
                starting_done = true;
                //transform.position = new Vector3(transform.position.x, 1, transform.position.z);
                rigid_entity_body.detectCollisions = true;
                rigid_entity_body.useGravity = true;
            }

            float temp = UnityEngine.Random.Range(.001f, .2f);
            //transform.position =  new Vector3(transform.position.x, transform.position.y + temp, transform.position.z);
            transform.Translate(transform.up * temp);
        }
        else
        {
            if (sm != null)
                sm.Update();
        }
    }

    protected override void LateUpdate()
    {
        base.LateUpdate();
    }
}
