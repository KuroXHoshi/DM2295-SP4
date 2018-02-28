using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerManager : MonoBehaviour
{
    private static SpawnerManager _instance;

    public GameObject[] entity_list;
    public GameObject[] boss_entity_list;
    public GameObject[] skill_entity_list;

    private List<GameObject> entity_pool_list = new List<GameObject>();
    private List<GameObject> boss_pool_list = new List<GameObject>();
    private List<GameObject> skill_pool_list = new List<GameObject>();

    public static SpawnerManager Instance { get { return _instance; } }

    //SETTING UP THE SPAWNER
    public void SetUpSpawnerManager()
    {
        for (int entity_list_count = 0; entity_list_count < entity_list.Length; ++entity_list_count)
        {
            for (int i = 0; i < 10; ++i)
            {
                GameObject obj = Instantiate(entity_list[entity_list_count]);
                obj.SetActive(false);
                entity_pool_list.Add(obj);
            }
        }

        for (int entity_list_count = 0; entity_list_count < boss_entity_list.Length; ++entity_list_count)
        {
            for (int i = 0; i < 10; ++i)
            {
                GameObject obj = Instantiate(boss_entity_list[entity_list_count]);
                obj.SetActive(false);
                boss_pool_list.Add(obj);
            }
        }

        for (int entity_list_count = 0; entity_list_count < skill_entity_list.Length; ++entity_list_count)
        {
            for (int i = 0; i < 10; ++i)
            {
                GameObject obj = Instantiate(skill_entity_list[entity_list_count]);
                obj.SetActive(false);
                skill_pool_list.Add(obj);
            }
        }
    }

    //RESETS ALL ENEMY
    public void ResetSpawnerManager()
    {
        foreach (GameObject obj in entity_pool_list)
        {
            if (obj.activeSelf)
            {
                obj.GetComponent<EnemyScript>().Reset();
                obj.SetActive(false);
            }
        }

        foreach (GameObject obj in boss_pool_list)
        {
            if (obj.activeSelf)
            {
                switch (obj.tag)
                {
                    case "BossMelee":
                        obj.GetComponent<BossScript>().Reset();
                        break;
                }

                obj.SetActive(false);
            }
        }

        List<List<GameObject>> list = GetAllEntity();

        foreach (List<GameObject> i in list)
        {
            foreach (GameObject obj in i)
            {
                if (obj.GetComponent<EnemyScript>() != null)
                {
                    obj.GetComponent<EnemyScript>().Reset();
                }
                else if (obj.GetComponent<BossScript>() != null)
                {
                    obj.GetComponent<BossScript>().Reset();
                }
                else if (obj.GetComponent<SkillScript>() != null)
                {
                    switch (obj.tag.ToLower())
                    {
                        case "firebomb":
                            obj.GetComponent<SkillFireBomb>().Reset();
                            break;
                        case "summon":
                            obj.GetComponent<SkillSummon>().Reset();
                            break;
                    }
                }
            }
        }
    }

    //RETURNS SIZE OF ENTITY TYPES
    public int GetSizeOfEntity(int _type)
    {
        switch(_type)
        {
            case 0:     //NORMAL ENTITIES
                return entity_list.Length;
            case 1:     //BOSS ENTITIES
                return boss_entity_list.Length;
            case 2:     //BOSS ENTITIES
                return skill_entity_list.Length;
            default:
                return 0;
        }
    }

    //RETURNS ALL ACTIVE ENTITES
    public List<List<GameObject>> GetAllEntity()
    {
        List<List<GameObject>> full_list = new List<List<GameObject>>();
        List<GameObject> list = new List<GameObject>();

        //MINIONS
        foreach (GameObject i in entity_pool_list)
        {
            if (i.activeSelf)
            {
                list.Add(i);
            }
        }

        full_list.Add(list);

        //BOSSES
        list = new List<GameObject>();

        foreach (GameObject i in boss_pool_list)
        {
            if (i.activeSelf)
            {
                list.Add(i);
            }
        }

        full_list.Add(list);

        //SKILLS
        list = new List<GameObject>();

        foreach (GameObject i in skill_pool_list)
        {
            if (i.activeSelf)
            {
                list.Add(i);
            }
        }

        full_list.Add(list);

        return full_list;
    }

    //RETURNS TRUE IF ALL ENTITES ARE DEAD
    public bool IsAllEntityDead()
    {
        foreach (GameObject i in entity_pool_list)
        {
            if (i.activeSelf)
            {
                return false;
            }         
        }

        foreach (GameObject i in boss_pool_list)
        {
            if (i.activeSelf)
            {
                return false;
            }
        }

        return true;
    }

    //RETURNS ENTITY OBJECT IF IT FITS THE CRITERIA
    public GameObject GetEntityObjectFromPool(int type)
    {
        foreach (GameObject entity_obj in entity_pool_list)
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

        return GetEntityObjectFromPool(type);
    }

    //RETURNS BOSS OBJECT IF IT FITS THE CRITERIA
    public GameObject GetBossEntityObjectFromPool(int type)
    {
        foreach (GameObject entity_obj in boss_pool_list)
        {
            if (!entity_obj.activeSelf && entity_obj.CompareTag(boss_entity_list[type].tag))
            {
                entity_obj.SetActive(true);
                return entity_obj;
            }
        }

        GameObject obj = Instantiate(boss_entity_list[type]);
        obj.SetActive(false);
        boss_pool_list.Add(obj);

        return GetBossEntityObjectFromPool(type);
    }

    //RETURNS SKILL OBJECT IF IT FITS THE CRITERIA
    public GameObject GetSkillEntityObjectFromPool(int type)
    {
        foreach (GameObject entity_obj in skill_pool_list)
        {
            if (!entity_obj.activeSelf && entity_obj.CompareTag(skill_entity_list[type].tag))
            {
                entity_obj.SetActive(true);
                return entity_obj;
            }
        }

        GameObject obj = Instantiate(skill_entity_list[type]);
        obj.SetActive(false);
        skill_pool_list.Add(obj);

        return GetSkillEntityObjectFromPool(type);
    }

    //RETURNS SKILL OBJECT IF IT FITS THE CRITERIA
    public GameObject GetSkillEntityObjectFromPool(string type)
    {
        foreach (GameObject entity_obj in skill_pool_list)
        {
            if (!entity_obj.activeSelf && entity_obj.tag.ToLower().Equals(type.ToLower()))
            {
                entity_obj.SetActive(true);
                return entity_obj;
            }
        }

        GameObject obj;

        foreach(GameObject entity_obj in skill_entity_list)
        {
            if(entity_obj.tag.ToLower().Equals(type.ToLower()))
            {
                obj = Instantiate(entity_obj);
                obj.SetActive(false);
                skill_pool_list.Add(obj);
                break;
            }
        }

        return GetSkillEntityObjectFromPool(type);
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }
}
