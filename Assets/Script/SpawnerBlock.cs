using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerBlock : MonoBehaviour {

    private Player player;
    private Vector2 distance_detect;
    private bool isDone;
    private List<Door> all_door_script = new List<Door>();

    private bool spawned;
    private bool spawn_boss = false;
    private bool contain_statue = false;
    private int spawner_room_id;

    private GameObject statue_script;
    public int pool_amount = 10;
    
    // Use this for initialization
    void Start () {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

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

                for (int i = 0; i < pool_amount; ++i)
                {
                    bool is_not_in_block = false;

                    while (!is_not_in_block)
                    {
                        int type = UnityEngine.Random.Range(0, SpawnerManager.Instance.GetSizeOfEntity(0));

                        float rand_x;
                        float rand_z;

                        if (UnityEngine.Random.Range(0, 100) < 50)
                        {
                            rand_x = UnityEngine.Random.Range(transform.position.x - (distance_detect.x * 0.5f), transform.position.x + (distance_detect.x * 0.5f));
                            rand_z = transform.position.z;
                        }
                        else
                        {
                            rand_x = transform.position.x;
                            rand_z = UnityEngine.Random.Range(transform.position.z - (distance_detect.y * 0.5f), transform.position.z + (distance_detect.y * 0.5f));
                        }

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
                            GameObject temp_obj = SpawnerManager.Instance.GetEntityObjectFromPool(type);
                            temp_obj.transform.position = temp_vec;
                        }
                    }
                    
                }

                if (spawn_boss)
                {
                    for (int i = 0; i < (player.GetpStats().level * 0.1f); ++i)
                    {
                        int type = UnityEngine.Random.Range(0, SpawnerManager.Instance.GetSizeOfEntity(1));

                        float rand_x = UnityEngine.Random.Range(transform.position.x - (distance_detect.x * 0.5f), transform.position.x + (distance_detect.x * 0.5f));
                        float rand_z = UnityEngine.Random.Range(transform.position.z - (distance_detect.y * 0.5f), transform.position.z + (distance_detect.y * 0.5f));

                        GameObject temp_obj = SpawnerManager.Instance.GetBossEntityObjectFromPool(type);
                        temp_obj.transform.position = new Vector3(rand_x, 0.05f, rand_z);

                    }

                    //<-
                }

                spawned = true;
                player.SetPlayerCurrentRoom(GetSpawnerRoomID());
                foreach (Door d in all_door_script)
                {
                    d.gameObject.SetActive(true);
                    d.TriggerDoor();
                }
            }
        }


        if (spawned)
        {
            isDone = SpawnerManager.Instance.IsAllEntityDead();
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

    public int GetSpawnerRoomID()
    {
        return spawner_room_id;
    }

    public void SetSpawnerRoomID(int _id)
    {
        spawner_room_id = _id;
    }

    public void SetDistanceDetect(Vector2 _input)
    {
        distance_detect = _input;
    }
}
