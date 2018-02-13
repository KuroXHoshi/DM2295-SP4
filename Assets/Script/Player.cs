using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float Health = 10, Stamina = 5, Damage = 1, AtkSpd = 1, MoveSpd = 10, HealthRegenSpd = 0.5f, StaminaRegenSpd = 0.1f;

    [SerializeField]
    private bool RegenSkill = false, IronWillSkill = false, EvasionSkill = false;

    [SerializeField]
    private ParticleSystem particle;

    private Animator anim;
    private Rigidbody rb;
    private float RotaSpd = 10;
    private float hitParticleDelay = 0;

    private Player()
    {

    }

    public float GetHealth()
    {
        return Health;
    }

    public float GetStamina()
    {
        return Stamina;
    }

    public void TakeDamage(float _dmg)
    {
        Health -= _dmg;
    }

    void Regeneration()
    {

    }

    // Use this for initialization
    void Start ()
    {
        anim = gameObject.GetComponent<Animator>();
        rb = gameObject.GetComponent<Rigidbody>();
        
        if (!anim)
            Debug.Log("Animator Controller not Loaded!");

        if (!rb)
            Debug.Log("Rigidbody component not Loaded!");
    }
	
	// Update is called once per frame
	void Update ()
    {
        Vector3 translation = new Vector3();
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            translation.Set(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            anim.SetBool("moving", (translation.x == 0 && translation.z == 0) ? false : true);
        }

        Vector3 newPos = transform.position + translation * MoveSpd * Time.deltaTime;
        Vector3 targetDir = newPos - transform.position;
        float step = RotaSpd * Time.deltaTime;
        Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0f);
        transform.rotation = Quaternion.LookRotation(newDir);
        transform.position = newPos;

        anim.SetBool("attacking", Input.GetMouseButtonDown(0));

        if (hitParticleDelay > 0)
            hitParticleDelay -= Time.deltaTime;

        if (!anim.IsInTransition(0) && anim.GetCurrentAnimatorStateInfo(0).IsName("Attack") && hitParticleDelay <= 0)
        {
            Vector3 offset = new Vector3(transform.forward.x * 2, transform.forward.y + 1, transform.forward.z * 2);
            Instantiate(particle, transform.position + offset, Quaternion.identity);
            hitParticleDelay = 0.5f;
        }

        // Ground Character
        //transform.position = new Vector3(transform.position.x, 0.0f, transform.position.z);
    }

    private void OnCollisionEnter(Collision collision)
    {
        
    }

    private void OnCollisionStay(Collision collision)
    {

    }
}
