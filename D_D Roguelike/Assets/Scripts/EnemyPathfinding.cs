using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyPathfinding : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector2 targetPosition;
    [SerializeField] private Rigidbody2D rb;
    bool usingTarget;

    [SerializeField] private float moveSpeed;
    [SerializeField] private float nextWaypointDistance = 0.3f;

    private Path path;
    private int currentWaypoint = 0;
    private bool reachedEndOfPath = false;
    public bool isStopped { get { return reachedEndOfPath; } }
    private bool _isAwake = true;
    public bool IsAwake(bool awakeState)=> _isAwake; 

    [SerializeField] private Seeker seeker;

    // Start is called before the first frame update
    void Start()
    {
        moveSpeed = GetComponent<Enemy>().GetMoveSpeed();
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
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
        if(path is null || !_isAwake) return;
        

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
                float distance = Vector2.Distance(transform.position, path.vectorPath[currentWaypoint]);

                if (distance < nextWaypointDistance)
                {
                    currentWaypoint++;
                }


                distance = Vector2.Distance(transform.position, path.vectorPath[currentWaypoint]);
                var step = Mathf.Clamp(moveSpeed * Time.deltaTime, 0, distance);
                var position = Vector2.MoveTowards(transform.position, path.vectorPath[currentWaypoint], step);
                rb.MovePosition(position);

                
            }
            else
            {
                float distance = Vector2.Distance(transform.position, target.position);
                var step = Mathf.Clamp(moveSpeed * Time.deltaTime, 0, distance);
                var position = Vector2.MoveTowards(transform.position, target.position, step);
                rb.MovePosition(position);
            }
        }
        else
        {
            if (Vector2.Distance(transform.position, targetPosition) > 3)
            {
                Debug.Log("long distance");

                float distance = Vector2.Distance(transform.position, path.vectorPath[currentWaypoint]);

                if (distance < nextWaypointDistance)
                {
                    currentWaypoint++;
                }

                distance = Vector2.Distance(transform.position, path.vectorPath[currentWaypoint]);
                var step = Mathf.Clamp(moveSpeed * Time.deltaTime, 0, distance);
                var position = Vector2.MoveTowards(transform.position, path.vectorPath[currentWaypoint], step);
                rb.MovePosition(position);

                
            }
            else
            {
                Debug.Log("short distance");

                float distance = Vector2.Distance(transform.position, targetPosition);
                var step = Mathf.Clamp(moveSpeed * Time.deltaTime, 0, distance);
                var position = Vector2.MoveTowards(transform.position, targetPosition, step);
                rb.MovePosition(position);
            }
        }
    }
}
