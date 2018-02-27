using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BasicEnemyScript : MonoBehaviour
{
    const float minPathUpdateTime = .2f;
    const float pathUpdateMoveThreshold = .5f;
    public float turnSpeed = 3;
    const float turnDst = 0;
    public float stoppingDst = 10;
    Path path;

    public string currState;
    private Player player; //: Transform;
   // public Transform playerTransform;
    public float MoveSpeed = 2;
    public float MaxDist = 15;
    public float MinDist = 1.5f;
    public float HP = 10;
    public float MAX_HP = 10;
    public int DMG = 1;
    private float dmgMultiplier = 1f;
    //public float movementSpd = 10;
    public bool NEAR_ATTACK = false;
    public float rotSpd = 10;
    public IntRange coin_range = new IntRange(10, 100);

    private bool starting_done;
    private int gold = 0;
    private bool pathFind = false;

    public GameObject gold_pile;
    public ParticleSystem particle;

    private Rigidbody rigid_entity_body;

    Animator animator;

    //[Header("Unity Stuff")]
    public Image health;
    //health = starthealth;
    //when take damage (take damage function)
    //healthBar.fillAmount = health / starthealth ;

    [SerializeField]
    private Transform model;

    public StateMachine sm { get; protected set; }
    public Animator GetAnim() { return animator; }
    public Player GetPlayer() { return player; }
    public Vector3 GetPlayerPos() { return player.transform.position; }
    public Transform GetModel() { return model; }

    private void Awake()
    {
        animator = model.GetComponent<Animator>();
        rigid_entity_body = GetComponent<Rigidbody>();

        if (sm == null)
            sm = new StateMachine();

        sm.AddState(new EnemyStates.Idle(this));
        sm.AddState(new EnemyStates.Movement(this));
        sm.AddState(new EnemyStates.Attack(this));

        starting_done = false;
        rigid_entity_body.detectCollisions = false;
        rigid_entity_body.useGravity = false;
    }

    // Use this for initialization
    void Start()
    {
        if (!(player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>()))
            Debug.Log("BasicEnemyScript.cs : Player not loaded");

        //if (!(camera = GameObject.FindGameObjectWithTag("MainCamera").transform))
            //Debug.Log("BasicEnemyScript.cs : Camera not loaded");

        //transform.position = Player.transform.position - Vector3.forward * MoveSpeed;
        gold = coin_range.Random;

        StartCoroutine(UpdatePath());
        //UpdatePath();

        // HP = MAX_HP;
        health.gameObject.SetActive(false);
    }
    
    private void FixedUpdate()
    {
        if (!starting_done)
        {
            if (transform.position.y >= 1)
            {
                starting_done = true;
                //transform.position = new Vector3(transform.position.x, 1, transform.position.z);
                rigid_entity_body.detectCollisions = true;
                rigid_entity_body.useGravity = true;
            }

            float temp = UnityEngine.Random.Range(.001f, .2f);
            //transform.position =  new Vector3(transform.position.x, transform.position.y + temp, transform.position.z);
            transform.Translate(transform.up * temp);
        }
        else
        {
            if (sm != null)
                sm.Update();
        }
    }

    private void LateUpdate()
    {
        // If enemy HP reaches 0 set gameObject to false
        if (HP <= 0)
        {
            GameObject obj = Instantiate(gold_pile, transform.position, gold_pile.transform.rotation);
            obj.GetComponent<Gold>().SetGoldValue(gold);
          
            transform.position = new Vector3(transform.position.x, -5f, transform.position.z);
            Reset();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("BasicEnemyScript.cs : Enemy got hit! <" + gameObject.GetHashCode() + ">");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player Hitting")
        {
            HP -= player.GetpStats().damage;
            if (health != null)
            {
                health.gameObject.SetActive(true);
                health.fillAmount = HP / MAX_HP;
            }
        }
    }

    public void SetPathFind(bool _set)
    {
        pathFind = _set;
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

    IEnumerator UpdatePath()
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

                if (pathFind)
                {
                    Debug.Log("Moving To: " + pathIndex);
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

    public void OnDrawGizmos()
    {
        if (path != null)
        {
            path.DrawWithGizmos();
        }
    }

    public void Reset()
    {
        starting_done = false;
        rigid_entity_body.detectCollisions = false;
        rigid_entity_body.useGravity = false;
        HP = MAX_HP;
        if (health != null)
        {
            health.fillAmount = HP / MAX_HP;
            health.gameObject.SetActive(false);
        }
        gameObject.SetActive(false);
    }
}