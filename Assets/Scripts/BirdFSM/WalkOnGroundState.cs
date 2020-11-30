using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WalkOnGroundState : StateMachineBehaviour
{
    BirdAI birdAI;
    NavMeshAgent navAgent;
    Vector3 targetDest;
    Vector3 lastPosition = Vector3.zero;
    float timeStuck = 0f;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (birdAI == null)
        {
            birdAI = animator.GetComponent<BirdAI>();
        }
        if (navAgent == null)
        {
            navAgent = animator.GetComponent<NavMeshAgent>();
        }

        navAgent.enabled = true;
        targetDest = birdAI.TrapPosition;
        navAgent.SetDestination(targetDest);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //if (Vector3.Distance(birdAI.transform.position, targetDest) > 0.01f)
        //{
        //    float step = birdAI.speed * Time.deltaTime * 0.6f; // calculate distance to move
        //    birdAI.transform.position = Vector3.MoveTowards(birdAI.transform.position, targetDest, step);
        //    birdAI.transform.rotation = Quaternion.Slerp(birdAI.transform.rotation, Quaternion.LookRotation(targetDest - birdAI.transform.position), 0.05f);
        //}
        //else
        //{
        //    animator.SetInteger("State", (int)BirdTransition.EAT);
        //}

        if (!navAgent.hasPath)
        {
            animator.SetInteger("State", (int)BirdTransition.EAT);
        }
        else
        {
            if (Vector3.Distance(birdAI.transform.position, lastPosition) <= 0f)
                timeStuck += Time.deltaTime;

            lastPosition = birdAI.transform.position;

            if (timeStuck >= 2f)
            {
                navAgent.ResetPath();
                animator.SetInteger("State", (int)BirdTransition.EAT);
            }
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        navAgent.enabled = false;
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
