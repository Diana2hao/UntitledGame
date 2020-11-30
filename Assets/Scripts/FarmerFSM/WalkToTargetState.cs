using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WalkToTargetState : StateMachineBehaviour
{
    public float stopDistanceFromTarget;
    NavMeshAgent navAgent;
    FarmerAI fAI;
    GameObject currTarget;
    float distance;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (navAgent == null)
        {
            navAgent = animator.GetComponent<NavMeshAgent>();
        }

        if(fAI == null)
        {
            fAI = animator.GetComponent<FarmerAI>();
        }

        //set the navmesh agent target
        currTarget = fAI.CurrTarget;
        navAgent.SetDestination(currTarget.transform.position);

    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        distance = Vector3.Distance(navAgent.transform.position, currTarget.transform.position);
        if (distance < stopDistanceFromTarget)
        {
            //stop the navmeshagent
            navAgent.ResetPath();
            animator.SetInteger("State", (int)FarmerTransition.CUTTREE);
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
