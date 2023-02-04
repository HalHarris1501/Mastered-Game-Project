using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolBehaviour : StateMachineBehaviour
{
    private float moveSpeed;
    private float waitTime;
    [SerializeField] private float startWaitTime;
    private float timePatrolling;
    [SerializeField] private float startTimePatrolling;

    private Vector2 moveSpot;
    private float minX, maxX, minY, maxY;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        waitTime = startWaitTime;
        timePatrolling = startTimePatrolling;
        moveSpeed = animator.GetComponent<Enemy>().GetMoveSpeed();
        minX = animator.transform.position.x - 5;
        minY = animator.transform.position.y - 5;
        maxX = animator.transform.position.x + 5;
        maxY = animator.transform.position.y + 5;

        moveSpot = new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));        
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        var step = moveSpeed * Time.deltaTime;
        var position = Vector2.MoveTowards(animator.transform.position, moveSpot, step);
        animator.transform.position = position;

        if(Vector2.Distance(animator.transform.position, moveSpot) < 0.2f)
        {
            if (waitTime <= 0)
            {
                minX = animator.transform.position.x - 5;
                minY = animator.transform.position.y - 5;
                maxX = animator.transform.position.x + 5;
                maxY = animator.transform.position.y + 5;

                moveSpot = new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));
                waitTime = startWaitTime;
            }
            else
            {
                waitTime -= Time.deltaTime;
            }
        }

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
}
