using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyPathfinding : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector2 targetPosition;
    bool usingTarget;

    [SerializeField] private float moveSpeed;
    [SerializeField] private float nextWaypointDistance = 3f;

    private Path path;
    private int currentWaypoint = 0;
    private bool reachedEndOfPath = false;

    [SerializeField] private Seeker seeker;

    // Start is called before the first frame update
    void Start()
    {
        moveSpeed = GetComponent<Enemy>().GetMoveSpeed();
        seeker = GetComponent<Seeker>();
    }

    public void ChasePlayer()
    {
        usingTarget = true;
        target = Player.Instance.transform;
        if (target != null)
        {
            InvokeRepeating("UpdatePath", 0f, 0.5f);
        }
    }

    public void MoveToTarget(Vector3 newTarget)
    {
        usingTarget = false;
        targetPosition = newTarget;
        if (targetPosition != null)
        {
            InvokeRepeating("UpdatePath", 0f, 0.5f);
        }
    }

    void UpdatePath()
    {
        if (usingTarget)
        {
            if (seeker.IsDone())
            {
                seeker.StartPath(transform.position, target.position, OnPathComplete);
            }
        }
        else
        {
            if (seeker.IsDone())
            {
                seeker.StartPath(transform.position, targetPosition, OnPathComplete);
            }
        }
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(path == null)
        {
            return;
        }

        if(currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            return;
        }
        else
        {
            reachedEndOfPath = false;
        }

        if (usingTarget)
        {
            if (Vector2.Distance(transform.position, target.position) > 3)
            {
                var step = moveSpeed * Time.deltaTime;
                var position = Vector2.MoveTowards(transform.position, path.vectorPath[currentWaypoint], step);
                transform.position = position;

                float distance = Vector2.Distance(transform.position, path.vectorPath[currentWaypoint]);

                if (distance < nextWaypointDistance)
                {
                    currentWaypoint++;
                }
            }
            else
            {
                var step = moveSpeed * Time.deltaTime;
                var position = Vector2.MoveTowards(transform.position, target.position, step);
                transform.position = position;
            }
        }
        else
        {
            if (Vector2.Distance(transform.position, targetPosition) > 3)
            {
                var step = moveSpeed * Time.deltaTime;
                var position = Vector2.MoveTowards(transform.position, path.vectorPath[currentWaypoint], step);
                transform.position = position;

                float distance = Vector2.Distance(transform.position, path.vectorPath[currentWaypoint]);

                if (distance < nextWaypointDistance)
                {
                    currentWaypoint++;
                }
            }
            else
            {
                var step = moveSpeed * Time.deltaTime;
                var position = Vector2.MoveTowards(transform.position, targetPosition, step);
                transform.position = position;
            }
        }
    }
}
