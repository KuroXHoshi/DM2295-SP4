using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour {

    private bool opened;
    private bool done;
    //private Rigidbody rigid_entity_body;

    // Use this for initialization
    void Start () {
        opened = true;
        done = false;

        //rigid_entity_body = GetComponent<Rigidbody>();
    }
	
	// Update is called once per frame
	void Update () {

        if (!done)
        {
            if (opened)
            {
                if (transform.position.y > -5)
                {
                    float temp_rand = UnityEngine.Random.Range(.001f, .2f);
                    Vector3 temp = new Vector3(transform.position.x, transform.position.y - temp_rand, transform.position.z);

                    transform.position = temp;                   
                }
                else
                {                   
                    done = true;
                    gameObject.SetActive(false);
                }
            }
            else
            {
                if (transform.position.y < 0)
                {
                    float temp_rand = UnityEngine.Random.Range(.001f, .2f);
                    Vector3 temp = new Vector3(transform.position.x, transform.position.y + temp_rand, transform.position.z);

                    transform.position = temp;
                }
                else
                {                   
                    done = true;

                    //if (!rigid_entity_body.detectCollisions)
                    //    rigid_entity_body.detectCollisions = true;
                }
            }
        }

	}

    public void TriggerDoor()
    {
        if (done)
        {
            opened = !opened;
            done = false;
        }
    }
}
