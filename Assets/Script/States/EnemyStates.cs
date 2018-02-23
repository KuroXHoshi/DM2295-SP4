using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStates : MonoBehaviour
{

    public class Idle : State
    {
        private BasicEnemyScript enemy;
        private Animator anim;

        public Idle(BasicEnemyScript _enemy) : base("Idle")
        {
            enemy = _enemy;
            anim = enemy.GetAnim();
        }

        public override void Enter()
        {
            anim.SetBool("walk", false);
            anim.SetBool("attack", false);
        }

        public override void Update()
        {
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
                return;

            float Distance = Vector3.Distance(enemy.transform.position, enemy.GetPlayerPos());

            if (Distance <= enemy.MinDist)
            {
                enemy.sm.SetNextState("Attack");
            }
            else if (Distance <= enemy.MaxDist)//Saw player and go to player
            {
                enemy.sm.SetNextState("Movement");
            }
        }

        public override void Exit()
        {

        }
    }

    public class Movement : State
    {
        private BasicEnemyScript enemy;
        private Animator anim;

        private Vector3 player_pos;
        private Vector3 enemy_pos;

        public Movement(BasicEnemyScript _enemy) : base("Movement")
        {
            enemy = _enemy;
            anim = enemy.GetAnim();
        }

        public override void Enter()
        {
            anim.SetBool("walk", true);
        }

        public override void Update()
        {
            player_pos = enemy.GetPlayerPos();
            enemy_pos = enemy.transform.position;
            Vector3 target_player_DIR = player_pos - enemy_pos;
            float step = enemy.rotSpd * Time.deltaTime;
            Vector3 newDir = Vector3.RotateTowards(enemy.GetModel().forward, target_player_DIR, step, 0.0f);
            enemy.GetModel().rotation = Quaternion.LookRotation(newDir);
            enemy.GetModel().rotation = new Quaternion(0f, enemy.GetModel().rotation.y, 0f, enemy.GetModel().rotation.w);
            enemy.transform.Translate(enemy.GetModel().forward * enemy.MoveSpeed * Time.deltaTime);
            float Distance = Vector3.Distance(enemy_pos, player_pos);

            if (Distance <= enemy.MinDist)//distance reachable,attack
            {
                enemy.sm.SetNextState("Attack");
            }
            else if (Distance > enemy.MaxDist)
            {
                enemy.sm.SetNextState("Idle");
            }
        }

        public override void Exit()
        {
            anim.SetBool("walk", false);
        }
    }

    public class Attack : State
    {
        private BasicEnemyScript enemy;
        private Animator anim;

        private Vector3 player_pos;
        private Vector3 enemy_pos;

        public Attack(BasicEnemyScript _enemy) : base("Attack")
        {
            enemy = _enemy;
            anim = enemy.GetAnim();
        }

        public override void Enter()
        {
            anim.SetBool("attack", true);
        }

        public override void Update()
        {
            // Instantiate enemy attack


            if (anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
                enemy.sm.SetNextState("Idle");
        }

        public override void Exit()
        {
            anim.SetBool("attack", false);
        }
    }
}
