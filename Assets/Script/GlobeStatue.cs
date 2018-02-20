using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobeStatue : MonoBehaviour
{
    [SerializeField]
    private Transform globe;

	// Use this for initialization
	void Start ()
    {
		
	}

    private void FixedUpdate()
    {
        globe.Rotate(0f, 5f, 0f);
    }

    // Update is called once per frame
    void Update ()
    {
		
	}
}
