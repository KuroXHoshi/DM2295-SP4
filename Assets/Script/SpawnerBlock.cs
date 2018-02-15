using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerBlock : MonoBehaviour {

    public GameObject[] entity_list;
    public GameObject[] boss_entity_list;
    private Player player;
    public Vector2 distance_detect;
    private bool isDone;
    private GameObject[] all_doors_obj;
    private List<Door> all_door_script = new List<Door>();

    private bool spawned;
    public bool spawn_boss;

    private List<GameObject> entity_pool_list = new List<GameObject>();
    private List<GameObject> boss_pool_list = new List<GameObject>();
    public int pool_amount = 10;

    // Use this for initialization
    void Start () {
        isDone = false;
        spawned = false;

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        all_doors_obj = GameObject.FindGameObjectsWithTag("Door");

        for(int i =0; i < all_doors_obj.Length; ++i)
        {
            all_door_script.Add(all_doors_obj[i].GetComponent<Door>());
        }


        Vector3 temp_vec_entity_pos = new Vector3(0, 0, 0);

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
                    int type = UnityEngine.Random.Range(0, entity_list.Length);

                    float rand_x = UnityEngine.Random.Range(transform.position.x - distance_detect.x, transform.position.x + distance_detect.x);
                    float rand_z = UnityEngine.Random.Range(transform.position.z - distance_detect.y, transform.position.z + distance_detect.y);

                    GameObject temp_obj = GetObjectFromPool(type);
                    temp_obj.transform.position = new Vector3(rand_x, -5, rand_z); 
                }

                if (spawn_boss)
                {
                    int type = UnityEngine.Random.Range(0, boss_entity_list.Length);

                    float rand_x = transform.position.x;
                    float rand_z = transform.position.z;

                    GameObject temp_obj = GetBossObjectFromPool(type);
                    temp_obj.transform.position = new Vector3(rand_x, 0.05f, rand_z);
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
            for (int i = 0; i < entity_pool_list.Count; ++i)
            {
                if (entity_pool_list[i].activeSelf)
                {
                    isDone = false;
                    break;
                }

                foreach (Door d in all_door_script)
                {
                    d.TriggerDoor();
                }

                isDone = true;
            }
        }

        if (isDone)
        {

            Destroy(this);
        }
	}

    public void SpawnBoss()
    {
        spawn_boss = true;
    }

    public GameObject GetObjectFromPool(int type)
    {

        int i = type * pool_amount;

        int end = (type * pool_amount) + pool_amount;

        for (; i < end; ++i)
        {
            if(!entity_pool_list[i].activeSelf)
            {
                entity_pool_list[i].SetActive(true);
                return entity_pool_list[i];
            }
        }

        GameObject obj = Instantiate(entity_list[type]);
        obj.SetActive(false);
        entity_pool_list.Add(obj);

        return GetObjectFromPool(type);
    }

    public GameObject GetBossObjectFromPool(int type)
    {

        int i = type * pool_amount;

        int end = (type * pool_amount) + pool_amount;

        for (; i < end; ++i)
        {
            if (!boss_pool_list[i].activeSelf)
            {
                boss_pool_list[i].SetActive(true);
                return boss_pool_list[i];
            }
        }

        GameObject obj = Instantiate(boss_entity_list[type]);
        obj.SetActive(false);
        boss_pool_list.Add(obj);

        return GetObjectFromPool(type);
    }
}
