using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SleepBehaviour : StateMachineBehaviour
{
    private SpriteRenderer spriteRenderer = null;
    private Unit pathfinder = null;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //on sleep trun off the sprite renderer and put pathfinder to sleep
        if (!GetSpriteRenderer(animator)) return;
        if (!GetPathfinder(animator)) return;

        animator.SetBool("isIdle", true);
        spriteRenderer.enabled = false;
        pathfinder.enabled = false;        
    }

    private bool GetSpriteRenderer(Animator animator)
    {
        if(spriteRenderer == null)
        {            
            spriteRenderer = animator.gameObject.GetComponent<SpriteRenderer>();
        }

        return spriteRenderer;
    }
    private bool GetPathfinder(Animator animator)
    {
        if (pathfinder == null)
        {            
            pathfinder = animator.gameObject.GetComponent<Unit>();
        }

        return pathfinder;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!GetSpriteRenderer(animator)) return;
        if (!GetPathfinder(animator)) return;

        spriteRenderer.enabled = true;
        pathfinder.enabled = true;
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
