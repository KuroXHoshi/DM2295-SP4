using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : EnemyScript {

    const float minPathUpdateTime = .2f;
    const float pathUpdateMoveThreshold = .5f;
    public float turnSpeed = 3;
    const float turnDst = 0;
    public float stoppingDst = 10;
    Path path;
    //IEnumerator co;

    protected override void Awake()
    {
        base.Awake();
        
        sm.AddState(new EnemyStates.Idle(this));
        sm.AddState(new EnemyStates.Movement(this));
        sm.AddState(new EnemyStates.Attack(this));

        //co = UpdatePath();
    }

    // Use this for initialization
    protected override void Start()
    {
        base.Start();

        //StartCoroutine(UpdatePath());
        //UpdatePath();

        // HP = MAX_HP;
        health.gameObject.SetActive(false);
    }

    // Update is called once per frame
    protected override void FixedUpdate()
    {
        base.FixedUpdate();

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

    protected override void LateUpdate()
    {
        base.LateUpdate();
    }

    protected override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);
        //Debug.Log("BasicEnemyScript.cs : Enemy got hit! <" + gameObject.GetHashCode() + ">");
    }

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
    }

    //public void OnPathFound(Vector3[] waypoints, bool pathSuccessful)
    //{
    //    if (pathSuccessful)
    //    {
    //        path = new Path(waypoints, transform.position, turnDst, stoppingDst);

    //        StopCoroutine("FollowPath");
    //        StartCoroutine("FollowPath");
    //    }
    //}

    //protected IEnumerator UpdatePath()
    //{

    //    if (Time.timeSinceLevelLoad < .5f)
    //    {
    //        yield return new WaitForSeconds(.5f);
    //    }
    //    PathRequestManager.RequestPath(new PathRequest(transform.position, player.transform.position, OnPathFound));

    //    float sqrMoveThreshold = pathUpdateMoveThreshold * pathUpdateMoveThreshold;
    //    Vector3 targetPosOld = player.transform.position;

    //    while (true)
    //    {
    //        yield return new WaitForSeconds(minPathUpdateTime);
    //        //print(((targeted_player.transform.position - targetPosOld).sqrMagnitude) + "    " + sqrMoveThreshold);
    //        if ((player.transform.position - targetPosOld).sqrMagnitude > sqrMoveThreshold)
    //        {
    //            PathRequestManager.RequestPath(new PathRequest(transform.position, player.transform.position, OnPathFound));
    //            targetPosOld = player.transform.position;
    //        }
    //    }
    //}

    //IEnumerator FollowPath()
    //{

    //    bool followingPath = true;
    //    int pathIndex = 0;
    //    //model.LookAt(path.lookPoints[0]);

    //    //float speedPercent = 1;

    //    while (followingPath)
    //    {
    //        Vector2 pos2D = new Vector2(transform.position.x, transform.position.z);
    //        while (path.turnBoundaries[pathIndex].HasCrossedLine(pos2D))
    //        {
    //            if (pathIndex == path.finishLineIndex)
    //            {
    //                followingPath = false;
    //                break;
    //            }
    //            else
    //            {
    //                pathIndex++;
    //            }
    //        }

    //        if (followingPath)
    //        {

    //            //if (pathIndex >= path.slowDownIndex && stoppingDst > 0)
    //            //{
    //            //    speedPercent = Mathf.Clamp01(path.turnBoundaries[path.finishLineIndex].DistanceFromPoint(pos2D) / stoppingDst);
    //            //    if (speedPercent < 0.01f)
    //            //    {
    //            //        followingPath = false;
    //            //    }
    //            //}

    //            if (pathFind)
    //            {
    //                //Debug.Log("Moving To: " + pathIndex);
    //                float step = turnSpeed * Time.deltaTime;
    //                Vector3 newDir = Vector3.RotateTowards(model.forward, path.lookPoints[pathIndex] - transform.position, step, 0f);
    //                model.rotation = Quaternion.LookRotation(newDir);
    //                //Quaternion targetRotation = Quaternion.LookRotation(path.lookPoints[pathIndex] - transform.position);
    //                //model.rotation = targetRotation;
    //                transform.Translate(model.forward * MoveSpeed * Time.deltaTime);
    //            }
    //        }

    //        model.rotation = new Quaternion(0f, model.rotation.y, 0f, model.rotation.w);

    //        yield return null;

    //    }
    //}

    //public void OnDrawGizmos()
    //{
    //    if (path != null)
    //    {
    //        path.DrawWithGizmos();
    //    }
    //}
}
