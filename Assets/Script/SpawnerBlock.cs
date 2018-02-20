using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerBlock : MonoBehaviour {

    public GameObject[] entity_list;
    public GameObject[] boss_entity_list;
    private Player player;
    public Vector2 distance_detect;
    private bool isDone;
    private List<Door> all_door_script = new List<Door>();

    private bool spawned;
    private bool spawn_boss = false;
    private bool contain_statue = false;

    private List<GameObject> entity_pool_list = new List<GameObject>();
    private List<GameObject> boss_pool_list = new List<GameObject>(); 
    private GameObject statue_script;
    public int pool_amount = 10;

    // Use this for initialization
    void Start () {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        for (int entity_list_count = 0; entity_list_count < entity_list.Length; ++entity_list_count)
        {
            for (int i = 0; i < pool_amount; ++i)
            {
                GameObject obj = Instantiate(entity_list[entity_list_count]);
                obj.SetActive(false);
                entity_pool_list.Add(obj);
            }
        }

        for (int entity_list_count = 0; entity_list_count < boss_entity_list.Length; ++entity_list_count)
        {
            for (int i = 0; i < pool_amount; ++i)
            {
                GameObject obj = Instantiate(boss_entity_list[entity_list_count]);
                obj.SetActive(false);
                boss_pool_list.Add(obj);
            }
        }

        //Debug.Log("SPAWNER BLOCK X: " + transform.position.x + " Y: " + transform.position.z);
        //Debug.Log("PLAYER POS X: " + player.transform.position.x + " Y: " + player.transform.position.y);
    }
	
	// Update is called once per frame
	void Update () {
		
        if(!spawned)
        {
            if(player.transform.position.x < transform.position.x + distance_detect.x && player.transform.position.x > transform.position.x - distance_detect.x &&
                player.transform.position.z < transform.position.z + distance_detect.y && player.transform.position.z > transform.position.z - distance_detect.y)
            {
                // Debug.Log("PLAYER DETECTED");

                for (int i = 0; i < 10; ++i)
                {
                    bool is_not_in_block = false;

                    while (!is_not_in_block)
                    {
                        int type = UnityEngine.Random.Range(0, entity_list.Length);

                        float rand_x = UnityEngine.Random.Range(transform.position.x - distance_detect.x, transform.position.x + distance_detect.x);
                        float rand_z = UnityEngine.Random.Range(transform.position.z - distance_detect.y, transform.position.z + distance_detect.y);

                        Vector3 temp_vec = new Vector3(rand_x, -5, rand_z);

                        foreach(Door d in all_door_script)
                        {
                            float dist = (d.transform.position - temp_vec).sqrMagnitude;
                          
                            if(dist > 2)
                            {
                                is_not_in_block = true;
                            }
                            else
                            {
                                is_not_in_block = false;
                                break;
                            }
                        }

                        if(is_not_in_block)
                        {
                            GameObject temp_obj = GetObjectFromPool(type);
                            temp_obj.transform.position = temp_vec;
                        }
                    }
                    
                }

                if (spawn_boss)
                {
                    for (int i = 0; i < (player.GetLevel() / 10) + 1; ++i)
                    {
                        int type = UnityEngine.Random.Range(0, boss_entity_list.Length);

                        float rand_x = UnityEngine.Random.Range(transform.position.x - (distance_detect.x * 0.5f), transform.position.x + (distance_detect.x * 0.5f));
                        float rand_z = UnityEngine.Random.Range(transform.position.z - (distance_detect.y * 0.5f), transform.position.z + (distance_detect.y * 0.5f));

                        GameObject temp_obj = GetBossObjectFromPool(type);
                        temp_obj.transform.position = new Vector3(rand_x, 0.05f, rand_z);
                    }
                }

                spawned = true;

                foreach (Door d in all_door_script)
                {
                    d.gameObject.SetActive(true);
                    d.TriggerDoor();
                }
            }
        }


        if (spawned)
        {
            foreach (GameObject i in entity_pool_list)
            {
                if (i.activeSelf)
                {
                    isDone = false;
                    break;
                }

                if (!isDone)
                    isDone = true;
            }

            if(isDone)
            {
                foreach (GameObject i in boss_pool_list)
                {
                    if (i.activeSelf)
                    {
                        isDone = false;
                        break;
                    }

                    if (!isDone)
                        isDone = true;
                }
            }
        }

        if (isDone)
        {
            foreach (Door d in all_door_script)
            {
                d.TriggerDoor();
            }

            if(contain_statue)
            {
                statue_script.SetActive(true);
            }

            gameObject.SetActive(false);
        }
	}

    public void SpawnBoss(bool input)
    {
        spawn_boss = input;
    }

    public GameObject GetObjectFromPool(int type)
    {
        foreach(GameObject entity_obj in entity_pool_list)
        {
            if (!entity_obj.activeSelf && entity_obj.CompareTag(entity_list[type].tag))
            {
                entity_obj.SetActive(true);
                return entity_obj;
            }
        }

        GameObject obj = Instantiate(entity_list[type]);
        obj.SetActive(false);
        entity_pool_list.Add(obj);

        return GetObjectFromPool(type);
    }

    public GameObject GetBossObjectFromPool(int type)
    {
        foreach (GameObject entity_obj in boss_pool_list)
        {
            if (!entity_obj.activeSelf && entity_obj.CompareTag(entity_list[type].tag))
            {
                entity_obj.SetActive(true);
                return entity_obj;
            }
        }

        GameObject obj = Instantiate(boss_entity_list[type]);
        obj.SetActive(false);
        boss_pool_list.Add(obj);

        return GetBossObjectFromPool(type);
    }

    public void GetListOfEnemies(ref List<GameObject> list_input)
    {
        list_input.Clear();
        
        foreach(GameObject i in entity_pool_list)
        {
            list_input.Add(i);
        }

         foreach(GameObject i in boss_pool_list)
        {
            list_input.Add(i);
        }
    }

    public void AddDoors(GameObject input)
    {
        all_door_script.Add(input.GetComponent<Door>());
    }

    public void Reset()
    {
        isDone = false;
        spawned = false;
        contain_statue = false;
        SpawnBoss(false);

        all_door_script.Clear();

        foreach(GameObject obj in entity_pool_list)
        {
            if (obj.activeSelf)
            {
                obj.GetComponent<BasicEnemyScript>().Reset();
                obj.SetActive(false);
            }
        }

        foreach (GameObject obj in boss_pool_list)
        {
            if (obj.activeSelf)
            {
                switch(obj.tag)
                {
                    case "BossMelee":
                        obj.GetComponent<BossScript>().Reset();
                        break;
                }
                
                obj.SetActive(false);
            }
        }

    }

    public void SetStatue(GameObject _input)
    {
        statue_script = _input;
        contain_statue = true;
    }

    public bool IsSpawningBoss()
    {
        return spawn_boss;
    }
}
