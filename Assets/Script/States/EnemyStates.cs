using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStates : MonoBehaviour
{

    public class Idle : State
    {
        private EnemyScript enemy;
        private Animator anim;

        private Vector3 player_pos;
        private Vector3 enemy_pos;

        public Idle(EnemyScript _enemy) : base("Idle")
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

            player_pos = enemy.GetPlayerPos();
            enemy_pos = enemy.transform.position;
            float Distance = Vector3.Distance(player_pos, enemy_pos);

            if (Distance < enemy.MinDist)
            {
                Vector3 target = player_pos - enemy_pos;
                float Angle = Vector3.Angle(enemy.GetModel().forward, target);

                if (Angle < 60f && Angle > -60f)
                {
                    if (enemy.GetComponent<BossMage>() != null)
                    {
                        enemy.sm.SetNextState("BossRangedAttack");
                        Debug.Log("ENTERING ATTACK STATE");
                    }
                    else
                        enemy.sm.SetNextState("Attack");
                }
                else
                    enemy.sm.SetNextState("Movement");
            }
            else if (Distance <= enemy.MaxDist)//Saw player and go to player
            {
                enemy.sm.SetNextState("Movement");
                Debug.Log("ENTERING MOVEMENT STATE");

            }
        }

        public override void Exit()
        {

        }
    }

    public class Movement : State
    {
        private EnemyScript enemy;
        private Animator anim;

        private Vector3 player_pos;
        private Vector3 enemy_pos;

        public Movement(EnemyScript _enemy) : base("Movement")
        {
            enemy = _enemy;
            anim = enemy.GetAnim();
        }

        public override void Enter()
        {
            anim.SetBool("walk", true);

            if (enemy.enemyType == EnemyScript.EnemyType.NORMAL)
                enemy.StartPathFind(true);
        }

        public override void Update()
        {
            player_pos = enemy.GetPlayerPos();
            enemy_pos = enemy.transform.position;

            if (enemy.enemyType == EnemyScript.EnemyType.BOSS)
            {
                Vector3 target_player_DIR = player_pos - enemy_pos;
                float step = enemy.rotSpd * Time.deltaTime;
                Vector3 newDir = Vector3.RotateTowards(enemy.GetModel().forward, target_player_DIR, step, 0.0f);
                enemy.GetModel().rotation = Quaternion.LookRotation(newDir);
                enemy.GetModel().rotation = new Quaternion(0f, enemy.GetModel().rotation.y, 0f, enemy.GetModel().rotation.w);
                enemy.transform.Translate(enemy.GetModel().forward * enemy.MoveSpeed * Time.deltaTime);
            }

            float Distance = Vector3.Distance(enemy_pos, player_pos);

            if (Distance <= enemy.MinDist)//distance reachable,attack
            {
                Vector3 target = player_pos - enemy_pos;
                float Angle = Vector3.Angle(enemy.GetModel().forward, target);

                if (Angle < 60f && Angle > -60f)
                {
                    if (enemy.GetComponent<BossMage>() != null)
                    {
                        enemy.sm.SetNextState("BossRangedAttack");
                       // Debug.Log("ENTERING ATTACK STATE");

                    }
                    else
                        enemy.sm.SetNextState("Attack");
                }
            }
            else if (Distance > enemy.MaxDist)
            {
                enemy.sm.SetNextState("Idle");
            }
        }

        public override void Exit()
        {
            anim.SetBool("walk", false);

            if (enemy.enemyType == EnemyScript.EnemyType.NORMAL)
                enemy.StartPathFind(false);
        }
    }

    public class Attack : State
    {
        private EnemyScript enemy;
        private Animator anim;

        private Vector3 player_pos;
        private Vector3 enemy_pos;

        private float particleDelay;

        public Attack(EnemyScript _enemy) : base("Attack")
        {
            enemy = _enemy;
            anim = enemy.GetAnim();
        }

        public override void Enter()
        {
            particleDelay = 0.4f;
            anim.SetBool("attack", true);
        }

        public override void Update()
        {
            if (particleDelay <= 0f)
            {
                player_pos = enemy.GetPlayerPos();
                enemy_pos = enemy.transform.position;
                float Distance = Vector3.Distance(player_pos, enemy_pos);

                if (Distance < enemy.MinDist + 0.5f)
                {
                    Vector3 target = player_pos - enemy_pos;
                    float Angle = Vector3.Angle(enemy.GetModel().forward, target);

                    if (Angle < 60f && Angle > -60f)
                    {
                        enemy.GetPlayer().TakeDamage(enemy.DMG, enemy_pos);
                        Instantiate(enemy.particle, new Vector3(player_pos.x, player_pos.y + 0.5f, player_pos.z), Quaternion.LookRotation(enemy_pos - player_pos));
                    }
                }

                enemy.sm.SetNextState("Idle");
            }

            particleDelay -= Time.deltaTime;
        }

        public override void Exit()
        {
            anim.SetBool("attack", false);
        }
    }

    public class BossRangedAttack : State
    {
        private EnemyScript enemy;
        private Animator anim;

        private Vector3 player_pos;
        private Vector3 enemy_pos;

        private float particleDelay;

        public BossRangedAttack(EnemyScript _enemy) : base("BossRangedAttack")
        {
            enemy = _enemy;
            anim = enemy.GetAnim();
        }

        public override void Enter()
        {
            particleDelay = 0.4f;
            anim.SetBool("attack", true);
        }

        public override void Update()
        {
            if (particleDelay <= 0f)
            {
               // Debug.Log("ATTACKING");
                player_pos = enemy.GetPlayerPos();
                enemy_pos = enemy.transform.position;
                float Distance = Vector3.Distance(player_pos, enemy_pos);

                if (Distance < enemy.MinDist + 0.5f)
                {
                   // Debug.Log("SHOOTING");
                    Vector3 target = player_pos - enemy_pos;
                    float Angle = Vector3.Angle(enemy.GetModel().forward, target);

                    if (Angle < 60f && Angle > -60f)
                    {
                       // Debug.Log("SHOT");
                        GameObject obj = SpawnerManager.Instance.GetSkillEntityObjectFromPool("Projectile");
                        obj.GetComponent<SkillFireProjectile>().SetParent(enemy.gameObject.GetInstanceID());
                        obj.transform.position = new Vector3(enemy_pos.x, enemy_pos.y + 1, enemy_pos.z);
                        obj.transform.forward = (enemy_pos - player_pos).normalized;
                    }
                }

                enemy.sm.SetNextState("Idle");
            }

            particleDelay -= Time.deltaTime;
        }

        public override void Exit()
        {
            anim.SetBool("attack", false);
        }
    }

    public class StateSkillFireStrike : State
    {
        private EnemyScript enemy;
        private Animator anim;

        private Vector3 player_pos;
        private Vector3 enemy_pos;


        public StateSkillFireStrike(EnemyScript _enemy) : base("SkillFireStrike")
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
            player_pos = enemy.GetPlayerPos();
            enemy_pos = enemy.transform.position;
            float Distance = Vector3.Distance(player_pos, enemy_pos);

            IntRange temp = new IntRange(-4, 4);

            for (int i = 0; i < 5; ++i)
            {              

                GameObject obj = SpawnerManager.Instance.GetSkillEntityObjectFromPool("FireBomb");
                obj.GetComponent<SkillFireBomb>().SetParent(enemy.gameObject.GetInstanceID());
                obj.transform.position = new Vector3(enemy_pos.x + temp.Random, enemy_pos.y + 0.5f, enemy_pos.z + temp.Random);
            }

            for (int i = 0; i < 3; ++i)
            {

                GameObject obj = SpawnerManager.Instance.GetSkillEntityObjectFromPool("FireBomb");
                obj.GetComponent<SkillFireBomb>().SetParent(enemy.gameObject.GetInstanceID());
                obj.transform.position = new Vector3(player_pos.x + temp.Random, enemy_pos.y + 0.5f, player_pos.z + temp.Random);
            }

            enemy.sm.SetNextState("Idle");

        }

        public override void Exit()
        {
            anim.SetBool("attack", false);
        }
    }
    public class StateSkillMine : State
    {
        private EnemyScript enemy;
        private Animator anim;

        private Vector3 player_pos;
        private Vector3 enemy_pos;


        public StateSkillMine(EnemyScript _enemy) : base("SkillMine")
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
            player_pos = enemy.GetPlayerPos();
            enemy_pos = enemy.transform.position;
            float Distance = Vector3.Distance(player_pos, enemy_pos);

            IntRange temp = new IntRange(-4, 4);

            for (int i = 0; i < 5; ++i)
            {
                GameObject obj = SpawnerManager.Instance.GetSkillEntityObjectFromPool("Mine");
                obj.GetComponent<SkillMine>().SetParent(enemy.gameObject.GetInstanceID());
                obj.transform.position = new Vector3(enemy_pos.x + temp.Random, enemy_pos.y + 0.5f, enemy_pos.z + temp.Random);
            }
          
            enemy.sm.SetNextState("Idle");
        }
        

        public override void Exit()
        {
            anim.SetBool("attack", false);
        }
    }


}
