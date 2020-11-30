using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum FarmerTransition
{
    IDLE,
    WANDER,
    WALKTOTARGET,
    CUTTREE,
    FLEE
}

public class IdleState : StateMachineBehaviour
{
    NavMeshAgent navAgent;
    FarmerAI fAI;
    float timer;
    float idleTime = 2f;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (fAI == null)
        {
            fAI = animator.GetComponent<FarmerAI>();
        }

        if (navAgent == null)
        {
            navAgent = animator.GetComponent<NavMeshAgent>();
        }

        navAgent.enabled = true;

        timer = 0f;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer += Time.deltaTime;

        //idle for idletime (2 sec)
        if (timer > idleTime)
        {
            if (fAI.HasTarget)
            {
                animator.SetInteger("State", (int)FarmerTransition.WALKTOTARGET);
            }
            else
            {
                animator.SetInteger("State", (int)FarmerTransition.WANDER);
            }
            
            timer = 0f;
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
