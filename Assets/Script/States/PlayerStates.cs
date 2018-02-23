using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStates : MonoBehaviour
{

    public class Idle : State
    {
        //private Player player;
        //private Animator anim;

        public Idle(Player _player) : base("Idle")
        {
            //player = _player;
            //anim = player.GetAnim();
        }

        public override void Enter()
        {
            
        }

        public override void Update()
        {
            
        }

        public override void Exit()
        {

        }
    }

    public class Movement : State
    {
        private Player player;
        private Animator anim;

        public Movement(Player _player) : base("Movement")
        {
            player = _player;
            anim = _player.GetAnim();
        }

        public override void Enter()
        {
            anim.SetBool("moving", true);
        }

        public override void Update()
        {
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
                return;

            Vector3 prevPos = player.transform.position;
            //transform.position += new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")) * MoveSpd * Time.deltaTime;
            player.transform.position += new Vector3(player.joystick.Horizontal(), 0, player.joystick.Vertical()) * player.GetpStats().moveSpd * Time.deltaTime;
            float step = player.GetRotaSpd() * Time.deltaTime;
            Vector3 newDir = Vector3.RotateTowards(player.transform.forward, player.transform.position - prevPos, step, 0.0f);
            player.transform.rotation = Quaternion.LookRotation(newDir);
        }

        public override void Exit()
        {
            anim.SetBool("moving", false);
        }
    }

    public class Attack : State
    {
        private Player player;
        private Animator anim;

        public Attack(Player _player) : base("Attack")
        {
            player = _player;
            anim = _player.GetAnim();
        }

        public override void Enter()
        {

        }

        public override void Update()
        {
            if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
            {
                anim.SetBool("attacking", true);
                player.PlayerAudio.MusicSource.clip = player.PlayerAudio.Attack;
                player.PlayerAudio.attackclip();
            }

            // Checks if state transits to attack
            if (!anim.IsInTransition(0) && anim.GetCurrentAnimatorStateInfo(0).IsName("Attack") && player.hitParticleDelay <= 0)
            {
                Vector3 offset = new Vector3(player.transform.forward.x * player.GetpStats().atkDist, player.transform.forward.y + 1, player.transform.forward.z * player.GetpStats().atkDist);
                Instantiate(player.GetParticle(), player.transform.position + offset, Quaternion.identity);
                player.hitParticleDelay = 0.3f;
                anim.SetBool("attacking", false);

                //playerState = PlayerState.Idle;
                player.sm.SetNextState("Idle");
            }
        }

        public override void Exit()
        {
            
        }
    }

}