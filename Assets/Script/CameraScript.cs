using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{

    [SerializeField]
    private Transform player;

    [SerializeField]
    private float defaultHeight = 20, defaultZ = -10;

    private void Awake()
    {
        if (!(player = GameObject.FindGameObjectWithTag("Player").transform))
            Debug.Log("Camera.cs: Unable to load Player!");
    }

    // Use this for initialization
    void Start ()
    {
        transform.position = new Vector3(13, defaultHeight, 6 + defaultZ);
	}
	
	// Update is called once per frame
	void Update ()
    {
        
    }

    private void LateUpdate()
    {
        float offsetX = 0, offsetZ = 0;

        //if (player.position.x > 13 && player.position.x < 25)
        //{
            offsetX = player.position.x - transform.position.x;
        //}

        //if (player.position.z > 6 && player.position.z < 29)
        //{
            offsetZ = player.position.z - transform.position.z + defaultZ;
        //}
        
        transform.position += new Vector3(offsetX, 0, offsetZ) * 5 * Time.deltaTime;
    }
}
