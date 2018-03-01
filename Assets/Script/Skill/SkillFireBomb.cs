using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillFireBomb : SkillScript
{
    public float timer;
    private float max_timer;
    public Vector2 distance_detect;
    public ParticleSystem effect;
    bool added_effect = false;
    public float dmg;

    protected override void Awake()
    {
    }

    // Use this for initialization
    protected override void Start()
    {
        max_timer = timer;
    }

    protected override void FixedUpdate()
    {
        timer -= Time.deltaTime;

        if(!added_effect)
        {
            Instantiate(effect, transform.position, transform.rotation);
            added_effect = true;
        }

        if(timer <= 0)
        {
            List<List<GameObject>> list = SpawnerManager.Instance.GetAllEntity();

            foreach(List<GameObject> i in list)
            {
                foreach(GameObject obj in i)
                {
                    if(obj.transform.position.x < transform.position.x + distance_detect.x && obj.transform.position.x > transform.position.x - distance_detect.x &&
                obj.transform.position.z < transform.position.z + distance_detect.y && obj.transform.position.z > transform.position.z - distance_detect.y)
                    {
                        if(!obj.GetInstanceID().Equals(parent_ID))
                        {
                            if(obj.GetComponent<EnemyScript>() != null)
                            {
                                obj.GetComponent<EnemyScript>().OnAttacked(dmg);
                                //Debug.Log("HITTTO RIMNO");
                            }
                            else if (obj.GetComponent<BossScript>() != null)
                            {
                                obj.GetComponent<BossScript>().OnAttacked(dmg);
                            }
                            else if (obj.GetComponent<Player>() != null)
                            {
                                obj.GetComponent<Player>().TakeDamage(dmg, transform.position);
                            }
                        }
                    }
                }
            }

            Instantiate(particle, transform.position, transform.rotation);
            Reset();
        }
    }

    protected override void LateUpdate()
    {
    }

    protected override void OnCollisionEnter(Collision collision)
    {
    }

    protected override void OnTriggerEnter(Collider other)
    {
    }

    public override void Reset()
    {
        base.Reset();
        timer = max_timer;
        added_effect = false;
    }

}
