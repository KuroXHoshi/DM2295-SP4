using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blessing : MonoBehaviour {

    public float timer = 5.0f;

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

    public void Update()
    {
       if(transform.position.y > 0.1f)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y - 3 * Time.deltaTime, transform.position.z);
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
}
