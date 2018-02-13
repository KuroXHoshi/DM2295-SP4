using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour {

    public float movementSpd = 10;

    private float rotSpd = 10;
    Vector3 newPos;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        newPos = transform.position + new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")) * movementSpd * Time.deltaTime;
        Vector3 targetDir = newPos - transform.position;
        float step = rotSpd * Time.deltaTime;
        Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0f);
        transform.rotation = Quaternion.LookRotation(newDir);
        transform.position = newPos;
	}

    public Vector3 Get_Player_Pos()
    {
        return newPos;
    }
}
