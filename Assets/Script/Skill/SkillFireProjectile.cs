using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillFireProjectile : SkillScript
{
    public float timer;
    private float max_timer;
    public Vector2 distance_detect;
    public float projectile_spd = 1;
    public float dmg; 

    private bool done = false;

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

        if (timer <= 0 || done)
        {
            List<List<GameObject>> list = SpawnerManager.Instance.GetAllEntity();

            foreach (List<GameObject> i in list)
            {
                foreach (GameObject obj in i)
                {
                    if (obj.transform.position.x < transform.position.x + distance_detect.x && obj.transform.position.x > transform.position.x - distance_detect.x &&
                obj.transform.position.z < transform.position.z + distance_detect.y && obj.transform.position.z > transform.position.z - distance_detect.y)
                    {
                        if (!obj.GetInstanceID().Equals(parent_ID) && !obj.tag.Equals(tag))
                        {
                            if (obj.GetComponent<EnemyScript>() != null)
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

            Reset();
            Instantiate(particle, transform.position, transform.rotation);
        }

        transform.position -= transform.forward.normalized * projectile_spd * Time.deltaTime;
    }

    protected override void LateUpdate()
    {
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetInstanceID() != parent_ID && other != this)
        {         
            done = true;
        }
    }

    public override void Reset()
    {
        base.Reset();
        timer = max_timer;
        done = false;
    }

}
