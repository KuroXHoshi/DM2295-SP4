using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkills : MonoBehaviour
{

    public void Dash(ref Vector3 _translation, ref float _stamina)
    {
        //_translation += _transform.forward * 5f;
        _stamina -= 1;
    }

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}
}
