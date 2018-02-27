using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillSummon : SkillScript
{
    public float timer;
    private float max_timer;
    public Vector2 distance_detect;
    private GameObject target;

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
    protected bool starting_done;

    protected bool pathFind = false;

    const float minPathUpdateTime = .2f;
    const float pathUpdateMoveThreshold = .5f;
    public float turnSpeed = 3;
    const float turnDst = 0;
    public float stoppingDst = 10;
    Path path;

    protected Rigidbody rigid_entity_body;

    Animator animator;

    [SerializeField]
    protected Transform model;

    public StateMachine sm { get; protected set; }
    public Animator GetAnim() { return animator; }
    public Transform GetModel() { return model; }
    public void SetPathFind(bool _set) { pathFind = _set; }

    protected override void Awake()
    {
        target = null;

        animator = model.GetComponent<Animator>();
        rigid_entity_body = GetComponent<Rigidbody>();

        if (sm == null)
            sm = new StateMachine();

        starting_done = false;
        rigid_entity_body.detectCollisions = false;
        rigid_entity_body.useGravity = false;
    }

    // Use this for initialization
    protected override void Start()
    {
        max_timer = timer;
        StartCoroutine(UpdatePath());
    }

    protected override void FixedUpdate()
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
            timer -= Time.deltaTime;

            if (target == null || !target.activeSelf)
            {
                GameObject temp = null;
                List<List<GameObject>> list = SpawnerManager.Instance.GetAllEntity();

                foreach (List<GameObject> i in list)
                {
                    foreach (GameObject obj in i)
                    {
                        if (obj.transform.position.x < transform.position.x + distance_detect.x && obj.transform.position.x > transform.position.x - distance_detect.x &&
                    obj.transform.position.z < transform.position.z + distance_detect.y && obj.transform.position.z > transform.position.z - distance_detect.y)
                        {
                            if (!obj.GetInstanceID().Equals(parent_ID))
                            {
                                if (obj.GetComponent<EnemyScript>() != null)
                                {
                                    if (temp == null ||
                                        Vector3.Distance(obj.transform.position, transform.position) < Vector3.Distance(temp.transform.position, transform.position))
                                    {
                                        temp = obj;
                                        Debug.Log("FINDING CLOSEST ENEMY");
                                    }
                                }
                                else if (obj.GetComponent<BossScript>() != null)
                                {
                                    if (temp == null ||
                                        Vector3.Distance(obj.transform.position, transform.position) < Vector3.Distance(temp.transform.position, transform.position))
                                    {
                                        temp = obj;
                                        Debug.Log("FINDING CLOSEST BOSS");

                                    }
                                }
                            }
                        }
                    }
                }
                target = temp;
            }

            if (timer <= 0)
            {
                Reset();
            }
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
        Debug.Log("UPDATING PATH");
        if (target != null)
        {
            if (Time.timeSinceLevelLoad < .5f)
            {
                yield return new WaitForSeconds(.5f);
            }
            PathRequestManager.RequestPath(new PathRequest(transform.position, target.transform.position, OnPathFound));

            float sqrMoveThreshold = pathUpdateMoveThreshold * pathUpdateMoveThreshold;
            Vector3 targetPosOld = target.transform.position;

            while (true)
            {
                yield return new WaitForSeconds(minPathUpdateTime);
                //print(((targeted_player.transform.position - targetPosOld).sqrMagnitude) + "    " + sqrMoveThreshold);
                if ((target.transform.position - targetPosOld).sqrMagnitude > sqrMoveThreshold)
                {
                    PathRequestManager.RequestPath(new PathRequest(transform.position, target.transform.position, OnPathFound));
                    targetPosOld = target.transform.position;
                }
            }
        }
    }

    IEnumerator FollowPath()
    {
        Debug.Log("FOLLOWING PATH");
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
                    //Debug.Log("Moving To: " + pathIndex);
                    float step = turnSpeed * Time.deltaTime;
                    Vector3 newDir = Vector3.RotateTowards(model.forward, path.lookPoints[pathIndex] - transform.position, step, 0f);
                    model.rotation = Quaternion.LookRotation(newDir);
                    //Quaternion targetRotation = Quaternion.LookRotation(path.lookPoints[pathIndex] - transform.position);
                    //model.rotation = targetRotation;
                    transform.Translate(model.forward * MoveSpeed * Time.deltaTime);
                    Debug.Log("MOVING");
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

    protected override void LateUpdate()
    {
    }

    protected override void OnCollisionEnter(Collision collision)
    {
    }

    protected override void OnTriggerEnter(Collider other)
    {
    }

    public virtual void OnAttacked(float _damage)
    {
        HP -= _damage;
    }

    protected override void Reset()
    {
        base.Reset();
        timer = max_timer;

        starting_done = false;
        rigid_entity_body.detectCollisions = false;
        rigid_entity_body.useGravity = false;
        HP = MAX_HP;
    }
}
