using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyScript : MonoBehaviour
{
    #region Variables
    public enum EnemyType
    {
        NORMAL,
        BOSS
    }

    const float minPathUpdateTime = .2f;
    const float pathUpdateMoveThreshold = .5f;
    public float turnSpeed = 3;
    const float turnDst = 0;
    public float stoppingDst = 10;
    private Coroutine co;
    Path path;
    private bool pathFinding = false;

    protected Player player; //: Transform;
   // public Transform playerTransform;
    public float MoveSpeed = 2;
    public float MaxDist = 15;
    public float MinDist = 1.5f;
    public float MAX_HP = 10;
    protected float HP;
    public int DMG = 1;
    private float dmgMultiplier = 1f;
    //public float movementSpd = 10;
    public bool NEAR_ATTACK = false;
    public float rotSpd = 10;
    public IntRange coin_range = new IntRange(10, 100);

    protected bool starting_done;
    private int gold = 0;
    protected bool pathFind = false;

    public GameObject gold_pile;
    public ParticleSystem particle;

    protected Rigidbody rigid_entity_body;

    Animator animator;

    //[Header("Unity Stuff")]
    public Image health;
    //health = starthealth;
    //when take damage (take damage function)
    //healthBar.fillAmount = health / starthealth ;

    [SerializeField]
    protected Transform model;
    #endregion

    #region Getters/Setters
    public StateMachine sm { get; protected set; }
    public EnemyType enemyType { get; protected set; }
    public Animator GetAnim() { return animator; }
    public Player GetPlayer() { return player; }
    public Vector3 GetPlayerPos() { return player.transform.position; }
    public Transform GetModel() { return model; }
    public void SetPathFind(bool _set) { pathFind = _set; }
    #endregion

    protected virtual void Awake()
    {
        animator = model.GetComponent<Animator>();
        rigid_entity_body = GetComponent<Rigidbody>();

        if (sm == null)
            sm = new StateMachine();

        starting_done = false;
        rigid_entity_body.detectCollisions = false;
        rigid_entity_body.useGravity = false;
    }

    // Use this for initialization
    protected virtual void Start()
    {
        if (!(player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>()))
            Debug.Log(this.GetType() + ".cs : Player not loaded");

        //if (!(camera = GameObject.FindGameObjectWithTag("MainCamera").transform))
        //Debug.Log(this.GetType() + ".cs : Camera not loaded");

        //transform.position = Player.transform.position - Vector3.forward * MoveSpeed;
        //co = StartCoroutine(UpdatePath());
        gold = coin_range.Random;
    }

    protected virtual void FixedUpdate()
    {
    }

    protected virtual void LateUpdate()
    {
        // If enemy HP reaches 0 set gameObject to false
        if (HP <= 0)
        {
            if (gold_pile != null)
            {
                GameObject obj = Instantiate(gold_pile, new Vector3(transform.position.x, 0.05f, transform.position.z), gold_pile.transform.rotation);
                obj.GetComponent<Gold>().SetGoldValue(gold);
            }
          
            transform.position = new Vector3(transform.position.x, -5f, transform.position.z);
            Reset();
        }
    }

    protected virtual void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("BasicEnemyScript.cs : Enemy got hit! <" + gameObject.GetHashCode() + ">");
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player Hitting")
        {
            OnAttacked(player.GetPlayerDamage());
        }
    }

    public virtual void OnAttacked(float _damage)
    {
        HP -= _damage;

        if (health != null)
        {
            health.gameObject.SetActive(true);
            health.fillAmount = HP / MAX_HP;
        }
    }

    #region PathFinding
    public void StartPathFind(bool _in)
    {
        if (_in)
        {
            pathFinding = true;

            if (co == null)
                co = StartCoroutine(UpdatePath());
            else
            {
                StopCoroutine(co);
                co = StartCoroutine(UpdatePath());
            }
        }
        else
        {
            pathFinding = false;

            if (co != null)
                StopCoroutine(co);
        }
    }

    public void OnPathFound(Vector3[] waypoints, bool pathSuccessful)
    {
        if (pathSuccessful)
        {
            path = new Path(waypoints, transform.position, turnDst, stoppingDst);

            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");
        }
    }

    protected IEnumerator UpdatePath()
    {

        if (Time.timeSinceLevelLoad < .5f)
        {
            yield return new WaitForSeconds(.5f);
        }
        PathRequestManager.RequestPath(new PathRequest(transform.position, player.transform.position, OnPathFound));

        float sqrMoveThreshold = pathUpdateMoveThreshold * pathUpdateMoveThreshold;
        Vector3 targetPosOld = player.transform.position;

        while (true)
        {
            yield return new WaitForSeconds(minPathUpdateTime);
            //print(((targeted_player.transform.position - targetPosOld).sqrMagnitude) + "    " + sqrMoveThreshold);
            if ((player.transform.position - targetPosOld).sqrMagnitude > sqrMoveThreshold)
            {
                PathRequestManager.RequestPath(new PathRequest(transform.position, player.transform.position, OnPathFound));
                targetPosOld = player.transform.position;
            }
        }
    }

    IEnumerator FollowPath()
    {

        bool followingPath = true;
        int pathIndex = 0;
        //model.LookAt(path.lookPoints[0]);

        //float speedPercent = 1;

        while (followingPath)
        {
            Vector2 pos2D = new Vector2(transform.position.x, transform.position.z);
            while (path.turnBoundaries[pathIndex].HasCrossedLine(pos2D))
            {
                if (pathIndex == path.finishLineIndex)
                {
                    followingPath = false;
                    break;
                }
                else
                {
                    pathIndex++;
                }
            }

            if (followingPath)
            {

                //if (pathIndex >= path.slowDownIndex && stoppingDst > 0)
                //{
                //    speedPercent = Mathf.Clamp01(path.turnBoundaries[path.finishLineIndex].DistanceFromPoint(pos2D) / stoppingDst);
                //    if (speedPercent < 0.01f)
                //    {
                //        followingPath = false;
                //    }
                //}

                if (pathFinding)
                {
                    //Debug.Log("Moving To: " + pathIndex);
                    float step = turnSpeed * Time.deltaTime;
                    Vector3 newDir = Vector3.RotateTowards(model.forward, path.lookPoints[pathIndex] - transform.position, step, 0f);
                    model.rotation = Quaternion.LookRotation(newDir);
                    //Quaternion targetRotation = Quaternion.LookRotation(path.lookPoints[pathIndex] - transform.position);
                    //model.rotation = targetRotation;
                    transform.Translate(model.forward * MoveSpeed * Time.deltaTime);
                }
            }

            model.rotation = new Quaternion(0f, model.rotation.y, 0f, model.rotation.w);

            yield return null;

        }
    }

    public void SetKnockBack(Vector3 dir)
    {
        transform.position += dir * 1.5f;
    }

    public void OnDrawGizmos()
    {
        if (path != null)
        {
            path.DrawWithGizmos();
        }
    }
    #endregion

    public void Reset()
    {

        DMG = DMG + (int)((player.GetComponent<Player>().GetpStats().level + 1) * 0.5f);
        MAX_HP = MAX_HP + (MAX_HP * (int)(((player.GetComponent<Player>().GetpStats().level + 1) * 20) / 100));

        starting_done = false;
        rigid_entity_body.detectCollisions = false;
        rigid_entity_body.useGravity = false;
        HP = MAX_HP;

        if (health != null)
        {
            //health.fillAmount = HP / MAX_HP;
            health.gameObject.SetActive(false);
        }

        gameObject.SetActive(false);
    }
}