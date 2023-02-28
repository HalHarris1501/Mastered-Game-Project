using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PatrolBehaviour : StateMachineBehaviour
{
    private float waitTime;
    [SerializeField] private float startWaitTime;
    private float timePatrolling;
    [SerializeField] private float startTimePatrolling;
    [SerializeField] private Tilemap walkable;

    private Vector3 moveSpot;
    private float minX, maxX, minY, maxY;
    private const float patrolRange = 5f;
    private Unit pathfinder;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!GetWalkableTilemap()) return;
        if (!GetPathfinder(animator)) return;
        waitTime = startWaitTime;
        timePatrolling = startTimePatrolling;
        moveSpot = animator.transform.position;
        
        GetPatrolPoint(animator);
    }

    private bool GetWalkableTilemap()
    {
        if (walkable == null)
        {
            Tilemap[] tilemaps = FindObjectsOfType<Tilemap>();
            foreach (Tilemap tilemap in tilemaps)
            {
                if (tilemap.name == "Background")
                {
                    Debug.Log("tilemap found");
                    walkable = tilemap;
                }
            }
        }

        return walkable;
    }

    private bool GetPathfinder(Animator animator)
    {
        if (pathfinder == null)
        {
            Debug.Log("Pathfinder found");
            pathfinder = animator.gameObject.GetComponent<Unit>();
        }

        return pathfinder;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Patrol(animator);        

        if (timePatrolling <= 0f)
        {
            animator.SetBool("isPatrolling", false);
            animator.SetBool("isIdle", true);
        }
        else
        {
            timePatrolling -= Time.deltaTime;
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}

    private void Patrol(Animator animator)
    {        
        //if close enough, get new path position
        if(Vector2.Distance(animator.transform.position, moveSpot) < 0.2f)
        {
            WaitAtPoint(animator);
        }
        bool isStopped = pathfinder.isStopped;

        //update animator states
        animator.SetBool("isPatrolling", !isStopped);
        animator.SetBool("isIdle", isStopped);
    }

    private void WaitAtPoint(Animator animator)
    {
        waitTime -= Time.deltaTime;

        if(waitTime <= 0)
        {
            GetPatrolPoint(animator);
        }
    }

    private void GetPatrolPoint(Animator animator)
    {
        bool validSpot = false;

        while (validSpot == false)
        {
            minX = Mathf.RoundToInt(animator.transform.position.x - patrolRange);
            minY = Mathf.RoundToInt(animator.transform.position.y - patrolRange);
            maxX = Mathf.RoundToInt(animator.transform.position.x + patrolRange);
            maxY = Mathf.RoundToInt(animator.transform.position.y + patrolRange);

            moveSpot = new Vector3(Random.Range(minX, maxX), Random.Range(minY, maxY), 0);
            moveSpot.z = walkable.gameObject.transform.position.z;
            moveSpot.x = Mathf.Floor(moveSpot.x) + 0.5f;
            moveSpot.y = Mathf.Floor(moveSpot.y) + 0.5f;
            Vector3Int testSpot = new Vector3Int(Mathf.FloorToInt(moveSpot.x), Mathf.FloorToInt(moveSpot.y), Mathf.FloorToInt(moveSpot.z));
            validSpot = walkable.HasTile(testSpot);
        }

        waitTime = startWaitTime;
        Debug.Log(moveSpot);
        pathfinder.PathFind(moveSpot);
    }
}
