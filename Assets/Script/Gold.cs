using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gold : MonoBehaviour {

    public float timer = 5.0f;
    private int gold_value;

    public void Update()
    {
        timer -= Time.deltaTime;

        if(timer <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void SetGoldValue(int _gold_input)
    {
        gold_value = _gold_input;
    }

    public int GetGoldValue()
    {
        return gold_value;
    }
}
