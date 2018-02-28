using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle : MonoBehaviour {
    [System.Serializable]
    private struct ParticleSettings
    {
        public GameObject Obj;
        public ParticleSystem ParticleSys;
        public bool Rotation;
        public float RotationSpeed;
        public bool ReverseRotation;
        public bool Decay;
        public bool DecayTimer;
        public float lifeTime;
    }

    [SerializeField]
    private ParticleSettings[] Particles;

    //[SerializeField]

    //[SerializeField]
    //private float lifeTime = 5.0f;

    private void FixedUpdate()
    {
        for (int i = 0; i < Particles.Length; ++i)
        {
            if (Particles[i].Obj != null)
            {
                if (Particles[i].Rotation)
                {
                    if (Particles[i].ReverseRotation)
                        Particles[i].Obj.transform.Rotate(new Vector3(0f, 0f, -1f) * Particles[i].RotationSpeed * Time.deltaTime);
                    else
                        Particles[i].Obj.transform.Rotate(new Vector3(0f, 0f, 1f) * Particles[i].RotationSpeed * Time.deltaTime);
                }

                if (Particles[i].Decay)
                {
                    if (Particles[i].DecayTimer)
                    {
                        if (Particles[i].lifeTime <= 0)
                            Destroy(gameObject);

                        Particles[i].lifeTime -= Time.deltaTime;
                    }
                    else
                    {
                        if (!Particles[i].ParticleSys.isPlaying)
                            Destroy(gameObject);
                    }
                }
            }
        }


    }
}
