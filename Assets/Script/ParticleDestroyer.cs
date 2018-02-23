using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleDestroyer : MonoBehaviour {

	public float lifeTime = 5.0f;

    private void FixedUpdate()
    {
        if (lifeTime <= 0)
            Destroy(gameObject);
        lifeTime -= Time.deltaTime;
    }
}
