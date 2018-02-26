using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blessing : MonoBehaviour {

    private float dur;

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
        if(dur >= 0)
        {
            dur -= Time.deltaTime;
        }

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

    public float GetDuration()
    {
        return dur;
    }

    public void SetDuration(float _input)
    {
        dur = _input;
    }
}
