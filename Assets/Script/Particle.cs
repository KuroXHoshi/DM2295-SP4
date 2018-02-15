using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle : MonoBehaviour
{
    
    private ParticleSystem particle;

    // Use this for initialization
    void Start ()
    {
        if (!(particle = gameObject.GetComponent<ParticleSystem>()))
            Debug.Log("Particle.cs : Unable to load ParticleSystem!");
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (!particle.isPlaying)
            Destroy(gameObject);
	}
}
