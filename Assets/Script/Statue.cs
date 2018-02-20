using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Statue : MonoBehaviour {

    public enum TYPE
    {
        HP_REGEN = 0,
        DEFENCE,
        EVASION,
        TOTAL_TYPE
    }

    public GameObject player_obj;
    private Player player_obj_script;

    public IntRange cost;
    public Vector2 distance_push;

    private bool is_risen;
    private TYPE type;

    private Rigidbody rigid_entity_body;

    private Vector2 starting_pos;

    // Use this for initialization
    void Start ()
    {
        rigid_entity_body = GetComponent<Rigidbody>();
        player_obj = GameObject.FindGameObjectWithTag("Player");

        player_obj_script = player_obj.GetComponent<Player>();

        is_risen = false;
        cost = new IntRange(1000, 3000);

        ///TEMP RAND OF CHOOSING BLESSING TYPE, TO BE CHANGE TO GET FROM TYPE ITSELF IN THE FUTURE
        IntRange temp = new IntRange(0, (int)TYPE.TOTAL_TYPE);
        type = (TYPE)temp.Random;

        starting_pos = new Vector2(transform.position.x, transform.position.z);
        rigid_entity_body.detectCollisions = false;
        rigid_entity_body.useGravity = false;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (!is_risen)
        {
            if (player_obj_script.transform.position.x < transform.position.x + distance_push.x && player_obj_script.transform.position.x > transform.position.x - distance_push.x &&
                player_obj_script.transform.position.z < transform.position.z + distance_push.y && player_obj_script.transform.position.z > transform.position.z - distance_push.y)
            {
                Vector3 normalized_vec = (player_obj_script.transform.position - transform.position).normalized * 0.5f;
                player_obj_script.transform.position += new Vector3(normalized_vec.x, 0, normalized_vec.z);            
            }

            if (transform.position.y >= 0)
            {
                is_risen = true;
                transform.position = new Vector3(starting_pos.x, 0.1f, starting_pos.y);
                rigid_entity_body.detectCollisions = true;
                rigid_entity_body.useGravity = true;
            }

            float temp = UnityEngine.Random.Range(.001f, .1f);

            transform.position = new Vector3(
                transform.position.x + ((transform.position.x > starting_pos.x + 0.2f) ? -0.2f : ((transform.position.x < starting_pos.x - 0.2f) ? 0.2f : -0.2f)), 
                transform.position.y + temp, 
                transform.position.z + ((transform.position.z > starting_pos.y + 0.2f) ? -0.2f : ((transform.position.z < starting_pos.y - 0.2f) ? 0.2f : -0.2f)));

        }
        else
        {

        }
	}

    public void SetType(int _type)
    {
        type = (TYPE)_type;
    }
}
