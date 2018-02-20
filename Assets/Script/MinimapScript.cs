using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MinimapScript : MonoBehaviour
{
    public Transform player;

    // Use this for initialization
    private void Start()
    {
        if (!(player = GameObject.FindGameObjectWithTag("Player").transform))
            Debug.Log(this.GetType() + ": Player not Loaded!");
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 newPos = player.position;
        newPos.y = transform.position.y;
        transform.position = newPos;
    }
}
