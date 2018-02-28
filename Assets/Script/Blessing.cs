using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blessing : MonoBehaviour {

    private float dur;

    [SerializeField]
    private GameObject text_mesh;
   
    private BoxCollider rg;

    private Player player;

    public enum TYPE
    {       
        REGEN,
        IRON_WILL,
        EVASION,

        SUMMON,
        SMITE,
        BASH,

        DASH,
        WARCRY,
        ULT_DEF,

        NONE,

        TOTAL_TYPE
    }

    private TYPE type;

    protected void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        rg = GetComponent<BoxCollider>();
        string temp = "";

        switch(type)
        {
            case TYPE.REGEN:
                temp = "Regeneration";
                break;

            case TYPE.IRON_WILL:
                temp = "Iron Will";
                break;

            case TYPE.EVASION:
                temp = "Evasion";
                break;

            case TYPE.SUMMON:
                temp = "Summoning";
                break;

            case TYPE.SMITE:
                temp = "Fire Strike";
                break;

            case TYPE.BASH:
                temp = "Bash";
                break;

            case TYPE.DASH:
                temp = "Dash";
                break;

            case TYPE.WARCRY:
                temp = "War Cry";
                break;

            case TYPE.ULT_DEF:
                temp = "Ultimate Defence";
                break;

            default:
                temp = "Regeneration";
                break;
        }

        text_mesh.GetComponent<TextMesh>().text = temp;
        rg.enabled = false;
    }

    public void Update()
    {
        if(dur >= 0)
        {
            dur -= Time.deltaTime;
        }

        if (transform.position.y > 0.1f)
        {
            transform.position = new Vector3(player.transform.position.x, transform.position.y - 3 * Time.deltaTime, player.transform.position.z);
        }
        else
        {
            if(!rg.enabled)
             rg.enabled = true;
        }

        if (player.transform.position.x < transform.position.x + 4 && player.transform.position.x > transform.position.x - 4 &&
                player.transform.position.z < transform.position.z + 4 && player.transform.position.z > transform.position.z - 4)
        {
            if (!text_mesh.activeSelf)
                text_mesh.SetActive(true);       
        }
        else
        {
            if (text_mesh.activeSelf)
                text_mesh.SetActive(false);
        }
    }

    public void SetBlessingType(TYPE _type)
    {
        type = _type;
    }

    public TYPE GetBlessingType()
    {
        return type;
    }

    public float GetDuration()
    {
        return dur;
    }

    public void SetDuration(float _input)
    {
        dur = _input;
    }
}
