using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PoacherFleeState : StateMachineBehaviour
{
    public float safeDistance;
    public float fleeDistance;
    public float speedIncrease;

    NavMeshAgent navAgent;
    PoacherAI pAI;

    bool isDestroyed;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (navAgent == null)
        {
            navAgent = animator.GetComponent<NavMeshAgent>();
        }

        if (pAI == null)
        {
            pAI = animator.GetComponent<PoacherAI>();
        }

        navAgent.speed += speedIncrease;

        isDestroyed = false;
        FindFleeDestination(animator);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!isDestroyed && pAI.TrapCon.IsTriggered)
        {
            pAI.GetOnlyTrap();
            isDestroyed = true;
        }

        if (!navAgent.hasPath)
        {
            animator.SetInteger("State", (int)PoacherTransition.IDLE);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        pAI.HasTarget = false;
        pAI.GotWatered = false;
        navAgent.speed -= speedIncrease;
    }

    void FindFleeDestination(Animator anim)
    {
        Vector3 center = pAI.FleeFromPlayer.transform.position;
        Vector3 intendedPosition = anim.transform.position + (anim.transform.position - center).normalized * fleeDistance;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(intendedPosition, out hit, 20f, 1))
        {
            Vector3 target = hit.position;
            navAgent.SetDestination(hit.position);
        }
        else
        {

        }
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
