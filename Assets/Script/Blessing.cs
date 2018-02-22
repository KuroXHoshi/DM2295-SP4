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

        TOTAL_TYPE
    }

    private TYPE type;

    public void Update()
    {
       
    }

    public void SetBlessingType(int _type)
    {
        type = (TYPE)_type;
    }

    public int GetBlessingType()
    {
        return (int)type;
    }
}
