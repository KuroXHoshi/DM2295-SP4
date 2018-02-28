using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillScript : MonoBehaviour {

    protected int parent_ID; 

    public ParticleSystem particle;

    protected Rigidbody rigid_entity_body;

    protected Animator animator;

    [SerializeField]
    protected Transform model;

    public StateMachine sm { get; protected set; }
    public Animator GetAnim() { return animator; }
    public Transform GetModel() { return model; }

    protected virtual void Awake()
    {      
    }

    // Use this for initialization
    protected virtual void Start()
    {      
    }

    protected virtual void FixedUpdate()
    {
    }

    protected virtual void LateUpdate()
    {   
    }

    protected virtual void OnCollisionEnter(Collision collision)
    {
    }

    protected virtual void OnTriggerEnter(Collider other)
    {    
    }

    public void SetParent(int _input)
    {
        parent_ID = _input;
    }

    public virtual void Reset()
    {
        gameObject.SetActive(false);
    }
}
