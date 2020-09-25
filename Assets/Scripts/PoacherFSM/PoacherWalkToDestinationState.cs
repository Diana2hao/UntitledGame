using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PoacherWalkToDestinationState : StateMachineBehaviour
{
    NavMeshAgent navAgent;
    PoacherAI PAI;
    Vector3 currTargetDest;
    Vector3 lastPosition = Vector3.zero;
    float timeStuck = 0f;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (navAgent == null)
        {
            navAgent = animator.GetComponent<NavMeshAgent>();
        }

        if (PAI == null)
        {
            PAI = animator.GetComponent<PoacherAI>();
        }

        //set the navmesh agent target
        currTargetDest = PAI.CurrTargetDest;
        navAgent.SetDestination(currTargetDest);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //if reached destination, go to next state
        if (!navAgent.hasPath)
        {
            //stop the navmeshagent
            navAgent.ResetPath();
            animator.SetTrigger("ExitWalking");
        }
        else
        {
            if (Vector3.Distance(PAI.transform.position, lastPosition) <= 0.01f)
                timeStuck += Time.deltaTime;

            lastPosition = PAI.transform.position;

            if (timeStuck >= 2f)
            {
                navAgent.ResetPath();
                animator.SetTrigger("ExitWalking");
            }
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

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
